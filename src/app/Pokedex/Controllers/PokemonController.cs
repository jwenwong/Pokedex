using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Services.Exceptions;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokedex.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;
        private readonly ILogger<PokemonController> _logger;
        public PokemonController(IPokemonService pokemonService, ILogger<PokemonController> logger)
        {
            _pokemonService = pokemonService;
            _logger = logger;
        }

        [HttpGet("{pokemon}")]
        public async Task<ActionResult<PokemonResponse>> GetPokemon(string pokemon)
        {
            try
            {
                return await _pokemonService.GetPokemon(pokemon);
            }
            catch(PokemonNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
            
        }
    }
}
