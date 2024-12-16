using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    /// <summary>
    /// Teknolojinin yeteneklere etkisi
    /// </summary>
    public class TechnologyAbilityImpact
    {
        [Key]
        public int Id { get; set; }

        public AbilityType AbilityType { get; set; }
        public double Impact { get; set; }
    }
}
