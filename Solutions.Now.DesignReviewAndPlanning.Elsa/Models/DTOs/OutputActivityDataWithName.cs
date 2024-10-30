using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa.Models.DTOs
{
    public class OutputActivityDataWithName
    {
        [Display(Name = "requestserial")]
        public int? requestSerial { get; set; }
        [Display(Name = "refRequestserial")]
        public int? refRequestSerial { get; set; }
        [Display(Name = "steps")]
        public IList<int?> steps { get; set; }
        [Display(Name = "names")]
        public IList<string?> names { get; set; }
        [Display(Name = "screens")]
        public IList<string?> screens { get; set; }
        [Display(Name = "Sender")]
        public string? Sender { get; set; }
    }
}
