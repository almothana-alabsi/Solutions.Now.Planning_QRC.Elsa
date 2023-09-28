    using Elsa;
    using Elsa.ActivityResults;
    using Elsa.Attributes;
    using Elsa.Expressions;
    using Elsa.Services;
    using Elsa.Services.Models;
using Microsoft.EntityFrameworkCore;
using Solutions.Now.DesignReviewAndPlanning.Elsa.Models;
    using Solutions.Now.DesignReviewAndPlanning.Elsa.Models.DTOs;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa.Activities
{

    [Activity(
         Category = "Approval",
         DisplayName = "Add Engineers of Planning",
         Description = "Add Engineers of Planning",
         Outcomes = new[] { OutcomeNames.Done }
     )]
    public class EngineeringOfPlanning : Activity
    {
        private readonly PlanningDBContext _PlanningDBContext;
        private readonly SsoDBContext _SsoDBContext;


        public EngineeringOfPlanning(PlanningDBContext planningDBContext,SsoDBContext ssoDBContext )
        {
            _PlanningDBContext = planningDBContext;
            _SsoDBContext= ssoDBContext;
             
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
                MainProject_Planning getData = await _PlanningDBContext.MainProject_Planning.FirstOrDefaultAsync(x => x.Serial == RequestSerial);
                EngineeringOfPlanningDTO engineeringOfPlanning = new EngineeringOfPlanningDTO
                {
                    Donor = getData.Donor,
                    EngOfAcquisition= getData.EngOfAcquisition,
                    EngOfDecentralization= getData.EngOfDecentralization,
                    EngOfDevelopmentCoordination= getData.EngOfDevelopmentCoordination,
                    EngOfHr = getData.EngOfHr   ,
                    EngOfPolicy= getData.EngOfPolicy ,
                    EngOfStatistics = getData.EngOfStatistics   ,
                    Audit = getData.Audit 
                };
                context.Output = engineeringOfPlanning;


            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

     
            return Done();



        }
    }
}
