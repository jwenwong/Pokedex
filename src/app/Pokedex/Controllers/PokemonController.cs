using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
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

        [HttpGet("{pokemon}")]
        public async Task<ActionResult<PokemonResponse>> GetPokemon(string pokemon)
        {
            return await _pokemonService.GetPokemon(pokemon);
        }
    }
}
