using lib.Models;
using lib.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace lib.Services
{
    public class TypeCalculator
        : ITypeCalculator
    {
        public Dictionary<Pokemon.PokeType, float> ScoreComplimentaryDefensiveTypings(Pokemon.PokeType primaryDefensiveType)
        {
            // Start with a dictionary of all types and a score of 1
            var result = new Dictionary<Pokemon.PokeType, float>
            {
                { Pokemon.PokeType.NONE, 1f },
                { Pokemon.PokeType.NORMAL, 1f },
                { Pokemon.PokeType.FIGHTING, 1f },
                { Pokemon.PokeType.FLYING, 1f },
                { Pokemon.PokeType.POISON, 1f },
                { Pokemon.PokeType.GROUND, 1f },
                { Pokemon.PokeType.ROCK, 1f },
                { Pokemon.PokeType.BUG, 1f },
                { Pokemon.PokeType.GHOST, 1f },
                { Pokemon.PokeType.STEEL, 1f },
                { Pokemon.PokeType.FIRE, 1f },
                { Pokemon.PokeType.WATER, 1f },
                { Pokemon.PokeType.GRASS, 1f },
                { Pokemon.PokeType.ELECTRIC, 1f },
                { Pokemon.PokeType.PSYCHIC, 1f },
                { Pokemon.PokeType.ICE, 1f },
                { Pokemon.PokeType.DRAGON, 1f },
                { Pokemon.PokeType.DARK, 1f },
                { Pokemon.PokeType.FAIRY, 1f }
            };

            var allMonoTypes = Pokemon.GetAllTypes();

            foreach (var attackingType in allMonoTypes)
            {
                if ((Pokemon.PokeType) attackingType == Pokemon.PokeType.NONE) continue;

                Interaction interactionA = new Interaction
                {
                    AttackingType = (Pokemon.PokeType)attackingType,
                    DefendingType = (Pokemon.PokeType)primaryDefensiveType
                };

                float componentA = GetDamageMultiplier(interactionA);

                foreach (var secondaryDefensiveType in allMonoTypes)
                {
                    if ((Pokemon.PokeType)secondaryDefensiveType == primaryDefensiveType) continue;
                    Interaction interactionB = new Interaction
                    {
                        AttackingType = (Pokemon.PokeType) attackingType,
                        DefendingType = (Pokemon.PokeType) secondaryDefensiveType
                    };

                    float componentB = GetDamageMultiplier(interactionB);

                    float totalDamageMultiplier = componentA * componentB;

                    result[(Pokemon.PokeType)secondaryDefensiveType] *= totalDamageMultiplier;
                }
            }

            return result;
        }
                
        public class Interaction
        {
            public Pokemon.PokeType AttackingType { get; set; }
            public Pokemon.PokeType DefendingType { get; set; }

            public static bool operator ==(Interaction a, Interaction b)
            {
                return a.AttackingType == b.AttackingType && a.DefendingType == b.DefendingType;
            }

            public static bool operator !=(Interaction a, Interaction b)
            {
                return !(a == b);
            }

            public override bool Equals(object obj)
            {
                if (obj is Interaction)
                {
                    return this == (Interaction)obj;
                }

                return false;
            }

            public override int GetHashCode()
            {
                return AttackingType.GetHashCode() ^ DefendingType.GetHashCode();
            }
        }

        float GetDamageMultiplier(Interaction interaction)
        {
            if (TypeChart.ContainsKey(interaction))
            {
                return TypeChart[interaction];
            }

            return 1f;
        }

        // const version of the type chart
        public static Dictionary<Interaction, float> TypeChart = new Dictionary<Interaction, float>
        {
            // Normal Attacks
            { new Interaction { AttackingType = Pokemon.PokeType.NORMAL, DefendingType = Pokemon.PokeType.ROCK }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.NORMAL, DefendingType = Pokemon.PokeType.STEEL }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.NORMAL, DefendingType = Pokemon.PokeType.GHOST }, 0f },

            // Fire Attacks
            { new Interaction { AttackingType = Pokemon.PokeType.FIRE, DefendingType = Pokemon.PokeType.FIRE }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.FIRE, DefendingType = Pokemon.PokeType.WATER }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.FIRE, DefendingType = Pokemon.PokeType.ROCK }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.FIRE, DefendingType = Pokemon.PokeType.DRAGON }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.FIRE, DefendingType = Pokemon.PokeType.BUG }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.FIRE, DefendingType = Pokemon.PokeType.STEEL }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.FIRE, DefendingType = Pokemon.PokeType.GRASS }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.FIRE, DefendingType = Pokemon.PokeType.ICE }, 2f },

            // Water Attacks
            { new Interaction { AttackingType = Pokemon.PokeType.WATER, DefendingType = Pokemon.PokeType.WATER }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.WATER, DefendingType = Pokemon.PokeType.GRASS }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.WATER, DefendingType = Pokemon.PokeType.DRAGON }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.WATER, DefendingType = Pokemon.PokeType.FIRE }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.WATER, DefendingType = Pokemon.PokeType.GROUND }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.WATER, DefendingType = Pokemon.PokeType.ROCK }, 2f },

            // Electric Attacks
            { new Interaction { AttackingType = Pokemon.PokeType.ELECTRIC, DefendingType = Pokemon.PokeType.GROUND }, 0f },
            { new Interaction { AttackingType = Pokemon.PokeType.ELECTRIC, DefendingType = Pokemon.PokeType.ELECTRIC }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.ELECTRIC, DefendingType = Pokemon.PokeType.GRASS }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.ELECTRIC, DefendingType = Pokemon.PokeType.DRAGON }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.ELECTRIC, DefendingType = Pokemon.PokeType.FLYING }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.ELECTRIC, DefendingType = Pokemon.PokeType.WATER }, 2f },

            // Grass Attacks
            { new Interaction { AttackingType = Pokemon.PokeType.GRASS, DefendingType = Pokemon.PokeType.FIRE }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.GRASS, DefendingType = Pokemon.PokeType.GRASS }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.GRASS, DefendingType = Pokemon.PokeType.POISON }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.GRASS, DefendingType = Pokemon.PokeType.FLYING }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.GRASS, DefendingType = Pokemon.PokeType.BUG }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.GRASS, DefendingType = Pokemon.PokeType.DRAGON }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.GRASS, DefendingType = Pokemon.PokeType.STEEL }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.GRASS, DefendingType = Pokemon.PokeType.GROUND }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.GRASS, DefendingType = Pokemon.PokeType.ROCK }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.GRASS, DefendingType = Pokemon.PokeType.WATER }, 2f },

            // Ice Attacks
            { new Interaction { AttackingType = Pokemon.PokeType.ICE, DefendingType = Pokemon.PokeType.STEEL }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.ICE, DefendingType = Pokemon.PokeType.FIRE }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.ICE, DefendingType = Pokemon.PokeType.WATER }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.ICE, DefendingType = Pokemon.PokeType.ICE }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.ICE, DefendingType = Pokemon.PokeType.GRASS }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.ICE, DefendingType = Pokemon.PokeType.GROUND }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.ICE, DefendingType = Pokemon.PokeType.FLYING }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.ICE, DefendingType = Pokemon.PokeType.DRAGON }, 2f },

            // Fighting Attacks
            { new Interaction { AttackingType = Pokemon.PokeType.FIGHTING, DefendingType = Pokemon.PokeType.NORMAL }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.FIGHTING, DefendingType = Pokemon.PokeType.FLYING }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.FIGHTING, DefendingType = Pokemon.PokeType.POISON }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.FIGHTING, DefendingType = Pokemon.PokeType.BUG }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.FIGHTING, DefendingType = Pokemon.PokeType.PSYCHIC }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.FIGHTING, DefendingType = Pokemon.PokeType.FAIRY }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.FIGHTING, DefendingType = Pokemon.PokeType.GHOST }, 0f },
            { new Interaction { AttackingType = Pokemon.PokeType.FIGHTING, DefendingType = Pokemon.PokeType.ROCK }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.FIGHTING, DefendingType = Pokemon.PokeType.STEEL }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.FIGHTING, DefendingType = Pokemon.PokeType.ICE }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.FIGHTING, DefendingType = Pokemon.PokeType.DARK }, 2f },

            // Poison Attacks
            { new Interaction { AttackingType = Pokemon.PokeType.POISON, DefendingType = Pokemon.PokeType.POISON }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.POISON, DefendingType = Pokemon.PokeType.GROUND }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.POISON, DefendingType = Pokemon.PokeType.ROCK }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.POISON, DefendingType = Pokemon.PokeType.GHOST }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.POISON, DefendingType = Pokemon.PokeType.STEEL }, 0f },
            { new Interaction { AttackingType = Pokemon.PokeType.POISON, DefendingType = Pokemon.PokeType.GRASS }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.POISON, DefendingType = Pokemon.PokeType.FAIRY }, 2f },
            
            // Ground Attacks
            { new Interaction { AttackingType = Pokemon.PokeType.GROUND, DefendingType = Pokemon.PokeType.FLYING }, 0f },
            { new Interaction { AttackingType = Pokemon.PokeType.GROUND, DefendingType = Pokemon.PokeType.GRASS }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.GROUND, DefendingType = Pokemon.PokeType.BUG }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.GROUND, DefendingType = Pokemon.PokeType.POISON }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.GROUND, DefendingType = Pokemon.PokeType.ROCK }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.GROUND, DefendingType = Pokemon.PokeType.STEEL }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.GROUND, DefendingType = Pokemon.PokeType.FIRE }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.GROUND, DefendingType = Pokemon.PokeType.ELECTRIC }, 2f },
            
            // Flying Attacks
            { new Interaction { AttackingType = Pokemon.PokeType.FLYING, DefendingType = Pokemon.PokeType.ROCK }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.FLYING, DefendingType = Pokemon.PokeType.STEEL }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.FLYING, DefendingType = Pokemon.PokeType.ELECTRIC }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.FLYING, DefendingType = Pokemon.PokeType.BUG }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.FLYING, DefendingType = Pokemon.PokeType.FIGHTING }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.FLYING, DefendingType = Pokemon.PokeType.GRASS }, 2f },

            // Psychic Attacks
            { new Interaction { AttackingType = Pokemon.PokeType.PSYCHIC, DefendingType = Pokemon.PokeType.DARK }, 0f },
            { new Interaction { AttackingType = Pokemon.PokeType.PSYCHIC, DefendingType = Pokemon.PokeType.STEEL }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.PSYCHIC, DefendingType = Pokemon.PokeType.PSYCHIC }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.PSYCHIC, DefendingType = Pokemon.PokeType.FIGHTING }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.PSYCHIC, DefendingType = Pokemon.PokeType.POISON }, 2f },

            // Bug Attacks
            { new Interaction { AttackingType = Pokemon.PokeType.BUG, DefendingType = Pokemon.PokeType.FIGHTING }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.BUG, DefendingType = Pokemon.PokeType.FLYING }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.BUG, DefendingType = Pokemon.PokeType.POISON }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.BUG, DefendingType = Pokemon.PokeType.GHOST }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.BUG, DefendingType = Pokemon.PokeType.STEEL }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.BUG, DefendingType = Pokemon.PokeType.FIRE }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.BUG, DefendingType = Pokemon.PokeType.FAIRY }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.BUG, DefendingType = Pokemon.PokeType.PSYCHIC }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.BUG, DefendingType = Pokemon.PokeType.DARK }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.BUG, DefendingType = Pokemon.PokeType.GRASS }, 2f },
            
            // Rock Attacks
            { new Interaction { AttackingType = Pokemon.PokeType.ROCK, DefendingType = Pokemon.PokeType.FIGHTING }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.ROCK, DefendingType = Pokemon.PokeType.GROUND }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.ROCK, DefendingType = Pokemon.PokeType.STEEL }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.ROCK, DefendingType = Pokemon.PokeType.FLYING }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.ROCK, DefendingType = Pokemon.PokeType.BUG }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.ROCK, DefendingType = Pokemon.PokeType.FIRE }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.ROCK, DefendingType = Pokemon.PokeType.ICE }, 2f },

            // Ghost Attacks
            { new Interaction { AttackingType = Pokemon.PokeType.GHOST, DefendingType = Pokemon.PokeType.NORMAL }, 0f },
            { new Interaction { AttackingType = Pokemon.PokeType.GHOST, DefendingType = Pokemon.PokeType.DARK }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.GHOST, DefendingType = Pokemon.PokeType.PSYCHIC }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.GHOST, DefendingType = Pokemon.PokeType.GHOST }, 2f },

            // Dragon Attacks
            { new Interaction { AttackingType = Pokemon.PokeType.DRAGON, DefendingType = Pokemon.PokeType.DRAGON }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.DRAGON, DefendingType = Pokemon.PokeType.STEEL }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.DRAGON, DefendingType = Pokemon.PokeType.FAIRY }, 0f },

            // Dark Attacks
            { new Interaction { AttackingType = Pokemon.PokeType.DARK, DefendingType = Pokemon.PokeType.FIGHTING }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.DARK, DefendingType = Pokemon.PokeType.DARK }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.DARK, DefendingType = Pokemon.PokeType.FAIRY }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.DARK, DefendingType = Pokemon.PokeType.GHOST }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.DARK, DefendingType = Pokemon.PokeType.PSYCHIC }, 2f },

            // Steel Attacks
            { new Interaction { AttackingType = Pokemon.PokeType.STEEL, DefendingType = Pokemon.PokeType.STEEL }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.STEEL, DefendingType = Pokemon.PokeType.FIRE }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.STEEL, DefendingType = Pokemon.PokeType.WATER }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.STEEL, DefendingType = Pokemon.PokeType.ELECTRIC }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.STEEL, DefendingType = Pokemon.PokeType.ROCK }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.STEEL, DefendingType = Pokemon.PokeType.ICE }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.STEEL, DefendingType = Pokemon.PokeType.FAIRY }, 2.0f },

            // Fairy Attacks
            { new Interaction { AttackingType = Pokemon.PokeType.FAIRY, DefendingType = Pokemon.PokeType.POISON }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.FAIRY, DefendingType = Pokemon.PokeType.STEEL }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.FAIRY, DefendingType = Pokemon.PokeType.FIRE }, 0.5f },
            { new Interaction { AttackingType = Pokemon.PokeType.FAIRY, DefendingType = Pokemon.PokeType.FIGHTING }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.FAIRY, DefendingType = Pokemon.PokeType.DARK }, 2f },
            { new Interaction { AttackingType = Pokemon.PokeType.FAIRY, DefendingType = Pokemon.PokeType.DRAGON }, 2f },
        };




        public static string TypeChartJson()
        {
            return "{\r\n\tbug: {\r\n\t\tdamageTaken: {\r\n\t\t\tBug: 0,\r\n\t\t\tDark: 0,\r\n\t\t\tDragon: 0,\r\n\t\t\tElectric: 0,\r\n\t\t\tFairy: 0,\r\n\t\t\tFighting: 2,\r\n\t\t\tFire: 1,\r\n\t\t\tFlying: 1,\r\n\t\t\tGhost: 0,\r\n\t\t\tGrass: 2,\r\n\t\t\tGround: 2,\r\n\t\t\tIce: 0,\r\n\t\t\tNormal: 0,\r\n\t\t\tPoison: 0,\r\n\t\t\tPsychic: 0,\r\n\t\t\tRock: 1,\r\n\t\t\tSteel: 0,\r\n\t\t\tWater: 0,\r\n\t\t},\r\n\t\tHPivs: {atk: 30, def: 30, spd: 30},\r\n\t\tHPdvs: {atk: 13, def: 13},\r\n\t},\r\n\tdark: {\r\n\t\tdamageTaken: {\r\n\t\t\tprankster: 3,\r\n\t\t\tBug: 1,\r\n\t\t\tDark: 2,\r\n\t\t\tDragon: 0,\r\n\t\t\tElectric: 0,\r\n\t\t\tFairy: 1,\r\n\t\t\tFighting: 1,\r\n\t\t\tFire: 0,\r\n\t\t\tFlying: 0,\r\n\t\t\tGhost: 2,\r\n\t\t\tGrass: 0,\r\n\t\t\tGround: 0,\r\n\t\t\tIce: 0,\r\n\t\t\tNormal: 0,\r\n\t\t\tPoison: 0,\r\n\t\t\tPsychic: 3,\r\n\t\t\tRock: 0,\r\n\t\t\tSteel: 0,\r\n\t\t\tWater: 0,\r\n\t\t},\r\n\t\tHPivs: {},\r\n\t},\r\n\tdragon: {\r\n\t\tdamageTaken: {\r\n\t\t\tBug: 0,\r\n\t\t\tDark: 0,\r\n\t\t\tDragon: 1,\r\n\t\t\tElectric: 2,\r\n\t\t\tFairy: 1,\r\n\t\t\tFighting: 0,\r\n\t\t\tFire: 2,\r\n\t\t\tFlying: 0,\r\n\t\t\tGhost: 0,\r\n\t\t\tGrass: 2,\r\n\t\t\tGround: 0,\r\n\t\t\tIce: 1,\r\n\t\t\tNormal: 0,\r\n\t\t\tPoison: 0,\r\n\t\t\tPsychic: 0,\r\n\t\t\tRock: 0,\r\n\t\t\tSteel: 0,\r\n\t\t\tWater: 2,\r\n\t\t},\r\n\t\tHPivs: {atk: 30},\r\n\t\tHPdvs: {def: 14},\r\n\t},\r\n\telectric: {\r\n\t\tdamageTaken: {\r\n\t\t\tpar: 3,\r\n\t\t\tBug: 0,\r\n\t\t\tDark: 0,\r\n\t\t\tDragon: 0,\r\n\t\t\tElectric: 2,\r\n\t\t\tFairy: 0,\r\n\t\t\tFighting: 0,\r\n\t\t\tFire: 0,\r\n\t\t\tFlying: 2,\r\n\t\t\tGhost: 0,\r\n\t\t\tGrass: 0,\r\n\t\t\tGround: 1,\r\n\t\t\tIce: 0,\r\n\t\t\tNormal: 0,\r\n\t\t\tPoison: 0,\r\n\t\t\tPsychic: 0,\r\n\t\t\tRock: 0,\r\n\t\t\tSteel: 2,\r\n\t\t\tWater: 0,\r\n\t\t},\r\n\t\tHPivs: {spa: 30},\r\n\t\tHPdvs: {atk: 14},\r\n\t},\r\n\tfairy: {\r\n\t\tdamageTaken: {\r\n\t\t\tBug: 2,\r\n\t\t\tDark: 2,\r\n\t\t\tDragon: 3,\r\n\t\t\tElectric: 0,\r\n\t\t\tFairy: 0,\r\n\t\t\tFighting: 2,\r\n\t\t\tFire: 0,\r\n\t\t\tFlying: 0,\r\n\t\t\tGhost: 0,\r\n\t\t\tGrass: 0,\r\n\t\t\tGround: 0,\r\n\t\t\tIce: 0,\r\n\t\t\tNormal: 0,\r\n\t\t\tPoison: 1,\r\n\t\t\tPsychic: 0,\r\n\t\t\tRock: 0,\r\n\t\t\tSteel: 1,\r\n\t\t\tWater: 0,\r\n\t\t},\r\n\t},\r\n\tfighting: {\r\n\t\tdamageTaken: {\r\n\t\t\tBug: 2,\r\n\t\t\tDark: 2,\r\n\t\t\tDragon: 0,\r\n\t\t\tElectric: 0,\r\n\t\t\tFairy: 1,\r\n\t\t\tFighting: 0,\r\n\t\t\tFire: 0,\r\n\t\t\tFlying: 1,\r\n\t\t\tGhost: 0,\r\n\t\t\tGrass: 0,\r\n\t\t\tGround: 0,\r\n\t\t\tIce: 0,\r\n\t\t\tNormal: 0,\r\n\t\t\tPoison: 0,\r\n\t\t\tPsychic: 1,\r\n\t\t\tRock: 2,\r\n\t\t\tSteel: 0,\r\n\t\t\tWater: 0,\r\n\t\t},\r\n\t\tHPivs: {def: 30, spa: 30, spd: 30, spe: 30},\r\n\t\tHPdvs: {atk: 12, def: 12},\r\n\t},\r\n\tfire: {\r\n\t\tdamageTaken: {\r\n\t\t\tbrn: 3,\r\n\t\t\tBug: 2,\r\n\t\t\tDark: 0,\r\n\t\t\tDragon: 0,\r\n\t\t\tElectric: 0,\r\n\t\t\tFairy: 2,\r\n\t\t\tFighting: 0,\r\n\t\t\tFire: 2,\r\n\t\t\tFlying: 0,\r\n\t\t\tGhost: 0,\r\n\t\t\tGrass: 2,\r\n\t\t\tGround: 1,\r\n\t\t\tIce: 2,\r\n\t\t\tNormal: 0,\r\n\t\t\tPoison: 0,\r\n\t\t\tPsychic: 0,\r\n\t\t\tRock: 1,\r\n\t\t\tSteel: 2,\r\n\t\t\tWater: 1,\r\n\t\t},\r\n\t\tHPivs: {atk: 30, spa: 30, spe: 30},\r\n\t\tHPdvs: {atk: 14, def: 12},\r\n\t},\r\n\tflying: {\r\n\t\tdamageTaken: {\r\n\t\t\tBug: 2,\r\n\t\t\tDark: 0,\r\n\t\t\tDragon: 0,\r\n\t\t\tElectric: 1,\r\n\t\t\tFairy: 0,\r\n\t\t\tFighting: 2,\r\n\t\t\tFire: 0,\r\n\t\t\tFlying: 0,\r\n\t\t\tGhost: 0,\r\n\t\t\tGrass: 2,\r\n\t\t\tGround: 3,\r\n\t\t\tIce: 1,\r\n\t\t\tNormal: 0,\r\n\t\t\tPoison: 0,\r\n\t\t\tPsychic: 0,\r\n\t\t\tRock: 1,\r\n\t\t\tSteel: 0,\r\n\t\t\tWater: 0,\r\n\t\t},\r\n\t\tHPivs: {hp: 30, atk: 30, def: 30, spa: 30, spd: 30},\r\n\t\tHPdvs: {atk: 12, def: 13},\r\n\t},\r\n\tghost: {\r\n\t\tdamageTaken: {\r\n\t\t\ttrapped: 3,\r\n\t\t\tBug: 2,\r\n\t\t\tDark: 1,\r\n\t\t\tDragon: 0,\r\n\t\t\tElectric: 0,\r\n\t\t\tFairy: 0,\r\n\t\t\tFighting: 3,\r\n\t\t\tFire: 0,\r\n\t\t\tFlying: 0,\r\n\t\t\tGhost: 1,\r\n\t\t\tGrass: 0,\r\n\t\t\tGround: 0,\r\n\t\t\tIce: 0,\r\n\t\t\tNormal: 3,\r\n\t\t\tPoison: 2,\r\n\t\t\tPsychic: 0,\r\n\t\t\tRock: 0,\r\n\t\t\tSteel: 0,\r\n\t\t\tWater: 0,\r\n\t\t},\r\n\t\tHPivs: {def: 30, spd: 30},\r\n\t\tHPdvs: {atk: 13, def: 14},\r\n\t},\r\n\tgrass: {\r\n\t\tdamageTaken: {\r\n\t\t\tpowder: 3,\r\n\t\t\tBug: 1,\r\n\t\t\tDark: 0,\r\n\t\t\tDragon: 0,\r\n\t\t\tElectric: 2,\r\n\t\t\tFairy: 0,\r\n\t\t\tFighting: 0,\r\n\t\t\tFire: 1,\r\n\t\t\tFlying: 1,\r\n\t\t\tGhost: 0,\r\n\t\t\tGrass: 2,\r\n\t\t\tGround: 2,\r\n\t\t\tIce: 1,\r\n\t\t\tNormal: 0,\r\n\t\t\tPoison: 1,\r\n\t\t\tPsychic: 0,\r\n\t\t\tRock: 0,\r\n\t\t\tSteel: 0,\r\n\t\t\tWater: 2,\r\n\t\t},\r\n\t\tHPivs: {atk: 30, spa: 30},\r\n\t\tHPdvs: {atk: 14, def: 14},\r\n\t},\r\n\tground: {\r\n\t\tdamageTaken: {\r\n\t\t\tsandstorm: 3,\r\n\t\t\tBug: 0,\r\n\t\t\tDark: 0,\r\n\t\t\tDragon: 0,\r\n\t\t\tElectric: 3,\r\n\t\t\tFairy: 0,\r\n\t\t\tFighting: 0,\r\n\t\t\tFire: 0,\r\n\t\t\tFlying: 0,\r\n\t\t\tGhost: 0,\r\n\t\t\tGrass: 1,\r\n\t\t\tGround: 0,\r\n\t\t\tIce: 1,\r\n\t\t\tNormal: 0,\r\n\t\t\tPoison: 2,\r\n\t\t\tPsychic: 0,\r\n\t\t\tRock: 2,\r\n\t\t\tSteel: 0,\r\n\t\t\tWater: 1,\r\n\t\t},\r\n\t\tHPivs: {spa: 30, spd: 30},\r\n\t\tHPdvs: {atk: 12},\r\n\t},\r\n\tice: {\r\n\t\tdamageTaken: {\r\n\t\t\thail: 3,\r\n\t\t\tfrz: 3,\r\n\t\t\tBug: 0,\r\n\t\t\tDark: 0,\r\n\t\t\tDragon: 0,\r\n\t\t\tElectric: 0,\r\n\t\t\tFairy: 0,\r\n\t\t\tFighting: 1,\r\n\t\t\tFire: 1,\r\n\t\t\tFlying: 0,\r\n\t\t\tGhost: 0,\r\n\t\t\tGrass: 0,\r\n\t\t\tGround: 0,\r\n\t\t\tIce: 2,\r\n\t\t\tNormal: 0,\r\n\t\t\tPoison: 0,\r\n\t\t\tPsychic: 0,\r\n\t\t\tRock: 1,\r\n\t\t\tSteel: 1,\r\n\t\t\tWater: 0,\r\n\t\t},\r\n\t\tHPivs: {atk: 30, def: 30},\r\n\t\tHPdvs: {def: 13},\r\n\t},\r\n\tnormal: {\r\n\t\tdamageTaken: {\r\n\t\t\tBug: 0,\r\n\t\t\tDark: 0,\r\n\t\t\tDragon: 0,\r\n\t\t\tElectric: 0,\r\n\t\t\tFairy: 0,\r\n\t\t\tFighting: 1,\r\n\t\t\tFire: 0,\r\n\t\t\tFlying: 0,\r\n\t\t\tGhost: 3,\r\n\t\t\tGrass: 0,\r\n\t\t\tGround: 0,\r\n\t\t\tIce: 0,\r\n\t\t\tNormal: 0,\r\n\t\t\tPoison: 0,\r\n\t\t\tPsychic: 0,\r\n\t\t\tRock: 0,\r\n\t\t\tSteel: 0,\r\n\t\t\tWater: 0,\r\n\t\t},\r\n\t},\r\n\tpoison: {\r\n\t\tdamageTaken: {\r\n\t\t\tpsn: 3,\r\n\t\t\ttox: 3,\r\n\t\t\tBug: 2,\r\n\t\t\tDark: 0,\r\n\t\t\tDragon: 0,\r\n\t\t\tElectric: 0,\r\n\t\t\tFairy: 2,\r\n\t\t\tFighting: 2,\r\n\t\t\tFire: 0,\r\n\t\t\tFlying: 0,\r\n\t\t\tGhost: 0,\r\n\t\t\tGrass: 2,\r\n\t\t\tGround: 1,\r\n\t\t\tIce: 0,\r\n\t\t\tNormal: 0,\r\n\t\t\tPoison: 2,\r\n\t\t\tPsychic: 1,\r\n\t\t\tRock: 0,\r\n\t\t\tSteel: 0,\r\n\t\t\tWater: 0,\r\n\t\t},\r\n\t\tHPivs: {def: 30, spa: 30, spd: 30},\r\n\t\tHPdvs: {atk: 12, def: 14},\r\n\t},\r\n\tpsychic: {\r\n\t\tdamageTaken: {\r\n\t\t\tBug: 1,\r\n\t\t\tDark: 1,\r\n\t\t\tDragon: 0,\r\n\t\t\tElectric: 0,\r\n\t\t\tFairy: 0,\r\n\t\t\tFighting: 2,\r\n\t\t\tFire: 0,\r\n\t\t\tFlying: 0,\r\n\t\t\tGhost: 1,\r\n\t\t\tGrass: 0,\r\n\t\t\tGround: 0,\r\n\t\t\tIce: 0,\r\n\t\t\tNormal: 0,\r\n\t\t\tPoison: 0,\r\n\t\t\tPsychic: 2,\r\n\t\t\tRock: 0,\r\n\t\t\tSteel: 0,\r\n\t\t\tWater: 0,\r\n\t\t},\r\n\t\tHPivs: {atk: 30, spe: 30},\r\n\t\tHPdvs: {def: 12},\r\n\t},\r\n\trock: {\r\n\t\tdamageTaken: {\r\n\t\t\tsandstorm: 3,\r\n\t\t\tBug: 0,\r\n\t\t\tDark: 0,\r\n\t\t\tDragon: 0,\r\n\t\t\tElectric: 0,\r\n\t\t\tFairy: 0,\r\n\t\t\tFighting: 1,\r\n\t\t\tFire: 2,\r\n\t\t\tFlying: 2,\r\n\t\t\tGhost: 0,\r\n\t\t\tGrass: 1,\r\n\t\t\tGround: 1,\r\n\t\t\tIce: 0,\r\n\t\t\tNormal: 2,\r\n\t\t\tPoison: 2,\r\n\t\t\tPsychic: 0,\r\n\t\t\tRock: 0,\r\n\t\t\tSteel: 1,\r\n\t\t\tWater: 1,\r\n\t\t},\r\n\t\tHPivs: {def: 30, spd: 30, spe: 30},\r\n\t\tHPdvs: {atk: 13, def: 12},\r\n\t},\r\n\tsteel: {\r\n\t\tdamageTaken: {\r\n\t\t\tpsn: 3,\r\n\t\t\ttox: 3,\r\n\t\t\tsandstorm: 3,\r\n\t\t\tBug: 2,\r\n\t\t\tDark: 0,\r\n\t\t\tDragon: 2,\r\n\t\t\tElectric: 0,\r\n\t\t\tFairy: 2,\r\n\t\t\tFighting: 1,\r\n\t\t\tFire: 1,\r\n\t\t\tFlying: 2,\r\n\t\t\tGhost: 0,\r\n\t\t\tGrass: 2,\r\n\t\t\tGround: 1,\r\n\t\t\tIce: 2,\r\n\t\t\tNormal: 2,\r\n\t\t\tPoison: 3,\r\n\t\t\tPsychic: 2,\r\n\t\t\tRock: 2,\r\n\t\t\tSteel: 2,\r\n\t\t\tWater: 0,\r\n\t\t},\r\n\t\tHPivs: {spd: 30},\r\n\t\tHPdvs: {atk: 13},\r\n\t},\r\n\twater: {\r\n\t\tdamageTaken: {\r\n\t\t\tBug: 0,\r\n\t\t\tDark: 0,\r\n\t\t\tDragon: 0,\r\n\t\t\tElectric: 1,\r\n\t\t\tFairy: 0,\r\n\t\t\tFighting: 0,\r\n\t\t\tFire: 2,\r\n\t\t\tFlying: 0,\r\n\t\t\tGhost: 0,\r\n\t\t\tGrass: 1,\r\n\t\t\tGround: 0,\r\n\t\t\tIce: 2,\r\n\t\t\tNormal: 0,\r\n\t\t\tPoison: 0,\r\n\t\t\tPsychic: 0,\r\n\t\t\tRock: 0,\r\n\t\t\tSteel: 2,\r\n\t\t\tWater: 2,\r\n\t\t},\r\n\t\tHPivs: {atk: 30, def: 30, spa: 30},\r\n\t\tHPdvs: {atk: 14, def: 13},\r\n\t},\r\n};";
        }
    }
}
