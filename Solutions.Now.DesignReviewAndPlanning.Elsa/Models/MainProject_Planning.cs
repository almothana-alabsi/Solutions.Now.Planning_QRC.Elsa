using System.ComponentModel.DataAnnotations;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa.Models
{
    public class MainProject_Planning
    {

        [Key]
        public int Serial { get; set; }
        public int? Donor { get; set; }
        public string? EngOFMap { get; set; }
        public string? EngOfStatistics { get; set; }
        public string? EngOfPolicy { get; set; }
        public string? EngOfHr { get; set; }
        public string? EngOfDecentralization { get; set; }
        public string? EngOfDevelopmentCoordination { get; set; }
        public string? Audit { get; set; }
        public string EngOfAcquisition { get; set; }
        public int? RejectFlagAcquisition { get; set; }
        public int? RejectFlagHR { get; set; }
        public int? RejectFlagMap { get; set; }
        public int? RejectFromDesignReview { get; set; }
    }
}
