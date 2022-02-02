using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using Pokedex.Exceptions;
using Pokedex.Models;
using Pokedex.Proxies;
using Refit;

namespace Pokedex.Services
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
                var response = await _pokeApiClient.GetResourceAsync<PokemonSpecies>(name);
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
                LogError(ex.Message);
                throw new PokemonNotFoundException(name);
            }
        }

        public async Task<PokemonResponse> GetTranslatedPokemon(string name)
        {
            var pokemon = await GetPokemon(name);

            try
            {
                var request = new TranslationRequest {Text = pokemon.Description};
                var translation = (pokemon.IsLegendary || pokemon.Habitat == "cave")
                    ? await _translationsProxy.TranslateYoda(request).ConfigureAwait(false)
                    : await _translationsProxy.TranslateShakespeare(request).ConfigureAwait(false);
                if (translation.Success.Total == 1)
                {
                    pokemon.Description = translation.Contents.Translated;
                }
            }
            catch (ApiException ex)
            {
                LogError(ex.Message);
            }
            return pokemon;
        }

        private void LogError(string message)
        {
            _logger.LogError($"[ERROR] {DateTime.Now} - Message: {message}");
        }
    }
}
