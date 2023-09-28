using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa.Models.DTOs
{
    public class RejectToSectionsFlagsPlanningDTO
    {
        [Display(Name = "RejectFlagHR")]
        public int? RejectFlagHR { get; set; }
        [Display(Name = "RejectFlagAcquisition")]
        public int? RejectFlagAcquisition { get; set; }
        [Display(Name = "RejectFlagMap")]
        public int? RejectFlagMap { get; set; }
        [Display(Name = "RejectFromDesignReview")]
        public int? RejectFromDesignReview { get; set; }


    }
}
