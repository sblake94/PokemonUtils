using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using PokeApiNet;

namespace lib.Models
{
    public partial class Pokemon
    {
        public Pokemon()
        {
            
        }

        public static IEnumerable<PokeType> GetAllTypes()
        {
            return Enum.GetValues(typeof(PokeType)) as IEnumerable<PokeType>;
        }
    }
}
