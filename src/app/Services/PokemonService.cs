using Proxies;
using Services.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using Services.Exceptions;
using System.Net.Http;

namespace Services
{
    public interface IPokemonService
    {
        Task<PokemonResponse> GetPokemon(string name);
    }
    public class PokemonService : IPokemonService
    {
        private readonly PokeApiClient _pokeApiClient;

        public PokemonService(PokeApiClient pokeApiClient)
        {
            _pokeApiClient = pokeApiClient;
        }
        public async Task<PokemonResponse> GetPokemon(string name)
        {
            try
            {
                var pokemon = await _pokeApiClient.GetResourceAsync<Pokemon>(name);
                var response = await _pokeApiClient.GetResourceAsync<PokemonSpecies>(pokemon.Species);
                return new PokemonResponse
                {
                    Name = response.Name,
                    Description = response.FlavorTextEntries.Where(x => x.Language.Name == "en").FirstOrDefault().FlavorText.Replace("\n", " ").Replace("\f", " "),
                    Habitat = response.Habitat.Name,
                    IsLegendary = response.IsLegendary
                };
            }
            catch(HttpRequestException ex)
            {
                throw new PokemonNotFoundException(name);
            }
        }
    }
}
