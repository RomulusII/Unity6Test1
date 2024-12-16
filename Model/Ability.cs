namespace Model
{
    public class Ability
    {
        public AbilityPrototype AbilityPrototype { get; set; }
        
        /// <summary>
        /// Yetenek ne kadar calisilmis?
        /// </summary>
        public double Proficiency { get; set; }

        public Ability(AbilityPrototype abilityPrototype, double proficiency) => (AbilityPrototype, Proficiency) = (abilityPrototype, proficiency);
    }
}