using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pokedex.Exceptions;
using Pokedex.Models;
using Pokedex.Services;

namespace Pokedex.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [Consumes(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;
        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        /// <summary>Returns basic details of a pokemon</summary>
        /// <param name="name">Name of pokemon to retrieve</param>
        /// <returns>PokemonResponse -
        ///     Description of the pokemon</returns>
        /// <remarks>Sample request:
        /// 
        ///     Get /pokemon/mewtwo</remarks>
        ///
        /// <response code="200">If pokemon is found</response>
        /// <response code="404">If no pokemon found with that name</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{name}", Name = "GetPokemon")]
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


        /// <summary>Returns basic details of a pokemon with a fun translation of its description.</summary>
        /// <param name="name">Name of pokemon to retrieve</param>
        /// <returns>PokemonResponse -
        ///     Description of the pokemon</returns>
        /// <remarks>
        /// - If the pokemon is a Legendary or has a habitat of 'Cave' the description will be Yoda translated.<br/>
        /// - If not, the description will be Shakespeare translated.<br/>
        /// - If the description cannot be translated, it will return the normal description.<br/>
        /// 
        ///Sample request:
        /// 
        ///     Get /pokemon/translated/mewtwo </remarks>
        /// <response code="200">If pokemon is found</response>
        /// <response code="404">If no pokemon found with that name</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
