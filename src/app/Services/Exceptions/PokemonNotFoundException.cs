using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exceptions
{
    public class PokemonNotFoundException : Exception
    {
        public PokemonNotFoundException(string name) : base($"Cannot find pokemon with name: {name}.")
        {

        }
    }
}
