using System;
using Proxies;
using Services.Models;
using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using Services.Exceptions;
using System.Net.Http;
using Proxies.Models;
using Refit;
using Microsoft.Extensions.Logging;

namespace Services
{
    public interface IPokemonService
    {
        Task<PokemonResponse> GetPokemon(string name);
        Task<PokemonResponse> GetTranslatedPokemon(string name);
    }
    public class PokemonService : IPokemonService
    {
        private readonly PokeApiClient _pokeApiClient;
        private readonly IFunTranslationsProxy _translationsProxy;
        private readonly ILogger<PokemonService> _logger;

        public PokemonService(PokeApiClient pokeApiClient, IFunTranslationsProxy translationsProxy, ILogger<PokemonService> logger)
        {
            _pokeApiClient = pokeApiClient;
            _translationsProxy = translationsProxy;
            _logger = logger;
        }
        public async Task<PokemonResponse> GetPokemon(string name)
        {
            try
            {
                var pokemon = await _pokeApiClient.GetResourceAsync<Pokemon>(name);
                var response = await _pokeApiClient.GetResourceAsync(pokemon.Species);
                return new PokemonResponse
                {
                    Name = response.Name,
                    Description = response.FlavorTextEntries.FirstOrDefault(x => x.Language.Name == "en")?.FlavorText.Replace("\n", " ").Replace("\f", " "),
                    Habitat = response.Habitat.Name,
                    IsLegendary = response.IsLegendary
                };
            }
            catch(HttpRequestException ex)
            {
                _logger.LogError($"[ERROR] {DateTime.Now} - Message: {ex.Message}");
                throw new PokemonNotFoundException(name);
            }
        }

        public async Task<PokemonResponse> GetTranslatedPokemon(string name)
        {
            var pokemon = await GetPokemon(name);

            try
            {
                var request = new TranslationRequest { Text = pokemon.Description };
                var translation = (pokemon.IsLegendary || pokemon.Habitat == "cave") ? await _translationsProxy.TranslateYoda(request).ConfigureAwait(false) : await _translationsProxy.TranslateShakespeare(request).ConfigureAwait(false);
                if (translation.Success.Total == 1)
                {
                    pokemon.Description = translation.Contents.Translated;
                }
            }
            catch(ApiException ex)
            {
                _logger.LogError($"[ERROR] {DateTime.Now} - Message: {ex.Message}");
            }
            return pokemon;
        }
    }
}
