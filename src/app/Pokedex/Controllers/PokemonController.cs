using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Exceptions;
using Services.Models;
using System.Threading.Tasks;

namespace Pokedex.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;
        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<PokemonResponse>> GetPokemon(string name)
        {
            try
            {
                return await _pokemonService.GetPokemon(name);
            }
            catch(PokemonNotFoundException)
            {
                return NotFound();
            }
            
        }

        [HttpGet("translated/{name}")]
        public async Task<ActionResult<PokemonResponse>> GetTranslatedPokemon(string name)
        {
            try
            {
                return await _pokemonService.GetTranslatedPokemon(name);
            }
            catch (PokemonNotFoundException)
            {
                return NotFound();
            }

        }
    }
}
