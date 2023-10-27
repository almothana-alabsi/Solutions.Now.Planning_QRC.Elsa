using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa.Models
{
    public class MasterData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int serial { get; set; }
        public int? masterSerial { get; set; }
        public string? descAR { get; set; }
        public string? descEN { get; set; }
    }
}
