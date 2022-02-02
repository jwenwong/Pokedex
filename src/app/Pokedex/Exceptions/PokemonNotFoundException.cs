using System;

namespace Pokedex.Exceptions
{
    public class PokemonNotFoundException : Exception
    {
        public PokemonNotFoundException(string name) : base($"Cannot find pokemon with name: {name}.")
        {

        }
    }
}
