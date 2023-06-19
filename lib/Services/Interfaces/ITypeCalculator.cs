using lib.Models;
using System.Collections.Generic;

namespace lib.Services.Interfaces
{
    public interface ITypeCalculator
    {
        Dictionary<Pokemon.PokeType, float> ScoreComplimentaryDefensiveTypings(Pokemon.PokeType input);
    }
}