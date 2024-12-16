using System.Collections.Generic;

namespace Model
{
    public class Technology : ItemWithNameAndDescriptionBase
    {
        public List<Technology> RequiredTechnologies { get; set; }
        public List<TechnologyAbilityImpact> AbilityImpacts { get; set; }

        // Reverse navigation
        public virtual List<BuildingPrototype> AvailableBuildings { get; set; } = new List<BuildingPrototype>();
    }

    public class TechnologyTreeCreator
    {
        public List<Technology> Technologies { get; private set; }

        public TechnologyTreeCreator()
        {

        }

        private void AddTechnology()
        {

        }

    }
}
