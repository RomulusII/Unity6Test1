using System.Collections.Generic;

namespace Model
{
    public class AbilityPrototype 
    {
        public static List<double> BaseMaxProficiencyList { get; } = new List<double> { 100, 300, 1000 };
        public static List<double> BaseLearnRateList { get; } = new List<double> { 3, 2, 1 };

        public AbilityType AbilityType { get; set; }

        public int Tier { get; set; }

        public double BaseMaxProficiency { get; set; } 

        public double BaseLearnRate { get; set; }

        public AbilityPrototype()
        {
        }

        public AbilityPrototype(AbilityType abilityType, Tier tier)
        {
            AbilityType = abilityType;
            Tier = (int)tier;
            BaseMaxProficiency = BaseMaxProficiencyList[(int)tier];
            BaseLearnRate = BaseLearnRateList[(int)tier];
        }
    }


}