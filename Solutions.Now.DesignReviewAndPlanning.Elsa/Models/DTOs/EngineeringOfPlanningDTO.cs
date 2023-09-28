using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa.Models.DTOs
{
    public class EngineeringOfPlanningDTO
    {
        [Display(Name = "EngOfStatistics")]
        public string? EngOfStatistics { get; set; }
        [Display(Name = "EngOfAcquisition")]
        public string? EngOfAcquisition { get; set; }
        [Display(Name = "EngOfDecentralization")]
        public string? EngOfDecentralization { get; set; }

        [Display(Name = "EngOfHr")]
        public string? EngOfHr { get; set; }

        [Display(Name = "EngOfDevelopmentCoordination")]
        public string? EngOfDevelopmentCoordination { get; set; }

        [Display(Name = "EngOfPolicy")]
        public string? EngOfPolicy { get; set; }

            [Display(Name = "Donor")]
            public int? Donor { get; set; }

        [Display(Name = "Audit")]
        public string? Audit { get; set; }
    }
}
