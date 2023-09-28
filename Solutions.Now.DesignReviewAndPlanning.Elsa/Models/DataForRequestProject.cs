using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa.Models
{
    public class DataForRequestProject
    {
        [Display(Name = "requestSerial")]
        public int? requestSerial { get; set; }

        [Display(Name = "userName")]
        public string? userName { get; set; }

        [Display(Name = "Name")]
        public List<string?> Name { get; set; }

        [Display(Name = "Email")]
        public List<string?> Email { get; set; }
        
        [Display(Name = "Steps")]
        public List<int?> Steps { get; set; }

        [Display(Name = "major")]
        public int? major { get; set; }

        [Display(Name = "Screens")]
        public List<string?> Screens { get; set; }
        [Display(Name = "Stages")]
        public int? Stages { get; set; }
    }
}
