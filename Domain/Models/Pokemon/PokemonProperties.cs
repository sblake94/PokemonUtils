using System;

namespace lib.Models
{
    public partial class Pokemon
    {
        public PokeType PokeType1 
        { 
            get { return _pokeType1; }
            set 
            { 
                if (value == PokeType.NONE)
                {
                    throw new ArgumentException("PokeType1 cannot be NONE");
                }

                _pokeType1 = value; 
            }
        }
        public PokeType PokeType2 
        {
            get { return _pokeType2; }
            private set { _pokeType2 = value; } 
        } 
    }
}
