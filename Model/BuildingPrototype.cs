namespace Model
{
    public class BuildingPrototype : ItemWithNameAndDescriptionBase
    {
        public BuildingType BuildingType { get; set; }

        public int RequiredArchitectureLevel { get; set; }

        public List<Technology> RequiredTechnologies { get; } = new List<Technology>();
        
        public Chest BuildingCost { get; set; }

        public Chest BuildingUpkeep { get; set; }

        public List<TechnologyAbilityImpact> AbilityImpacts { get; set; }
    }
}
