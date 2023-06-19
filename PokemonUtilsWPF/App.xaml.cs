using lib.Models;
using lib.Services;
using lib.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PokemonUtilsWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            ITypeCalculator typeCalculator = new TypeCalculator();

            var primaryType = Pokemon.PokeType.FAIRY;

            // Get score of each secondary type when paired with the primary type
            Dictionary<Pokemon.PokeType, float> scoredSecondaryTypes = typeCalculator.ScoreComplimentaryDefensiveTypings(primaryType);

            // Sort the results by score
            var sortedResult = from entry in scoredSecondaryTypes 
                               orderby entry.Value 
                               descending 
                               select entry;


            Console.ReadLine();
        }
    }
}
