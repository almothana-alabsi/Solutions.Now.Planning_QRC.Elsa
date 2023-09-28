using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using Solutions.Now.DesignReviewAndPlanning.Elsa.Models;
using Solutions.Now.DesignReviewAndPlanning.Elsa.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa.Activities
{
    [Activity(
         Category = "Flags",
         DisplayName = "Reject To Sections Flags Planning",
         Description = "Reject To Sections Flags Planning",
         Outcomes = new[] { OutcomeNames.Done }
     )]
    public class RejectToSectionsFlagsPlanning : Activity
    {
        private readonly PlanningDBContext _PlanningDBContext;
         

        public RejectToSectionsFlagsPlanning(PlanningDBContext planningDBContext )
        {
            _PlanningDBContext = planningDBContext;
             
        }
        [ActivityInput(Hint = "Enter an expression that evaluates to the Request serial.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public int RequestSerial { get; set; }


        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            const int Administration = 29;
            const int PolicyandPlanningDirectorate = 42;
            const int DirectorateOfRealEstateAndAcquisition = 43;
            const int MapSection = 201;
            const int HumanResourcePlanningSection = 202;
            const int AcquisitionSection = 101;
            const int HR = 202;
            const int DevelopmentCoordinationUnit = 30;
            const int EngOfMap11 = 3578;
            const int SectionHead = 3581;
            const int directorateDirector = 3579;
            const int administrationDirector = 3577;
            const int PlanningWorkFlowserial = 4666;

            IList<int?> steps = new List<int?>();
            IList<string> userNameDB = new List<string>();
            IList<string> forms = new List<string>();
            List<WorkFlowRules> workFlowRules = _PlanningDBContext.WorkFlowRules.AsQueryable().Where(w => w.workflow == PlanningWorkFlowserial).OrderBy(s => s.step).ToList<WorkFlowRules>();

            for (int i = 0; i < workFlowRules.Count; i++)
            {
                userNameDB.Add(workFlowRules[i].username);
                steps.Add(workFlowRules[i].step);
                forms.Add(workFlowRules[i].screen);

            }

            try
            {
                MainProject_Planning getData = await _PlanningDBContext.MainProject_Planning.OrderBy(y=>y.Serial).FirstOrDefaultAsync(x => x.Serial == RequestSerial);
                RejectToSectionsFlagsPlanningDTO rejectToSectionsFlagsPlanning = new RejectToSectionsFlagsPlanningDTO
                {
                    RejectFlagHR = getData.RejectFlagHR,
                    RejectFlagAcquisition = getData.RejectFlagAcquisition,
                    RejectFlagMap = getData.RejectFlagMap,
                    RejectFromDesignReview = getData.RejectFromDesignReview

                };
                context.Output = rejectToSectionsFlagsPlanning;


            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }


            return Done();



        }
    }
}