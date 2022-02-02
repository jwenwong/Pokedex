using System;

namespace Services.Exceptions
{
    public class PokemonNotFoundException : Exception
    {
        public PokemonNotFoundException(string name) : base($"Cannot find pokemon with name: {name}.")
        {

        }
    }
}
