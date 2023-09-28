using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using Microsoft.EntityFrameworkCore;
using Solutions.Now.DesignReviewAndPlanning.Elsa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa.Activities
{

    [Activity(
         Category = "Approval",
         DisplayName = "Fyi For Sections Head Planning",
         Description = "Fyi For Sections Head Planning",
         Outcomes = new[] { OutcomeNames.Done }
     )]
    public class FyiForSectionsHeadPlanning : Activity
    {
        private readonly PlanningDBContext _PlanningDBContext;
        private readonly SsoDBContext _SsoDBContext;


        public FyiForSectionsHeadPlanning(PlanningDBContext planningDBContext, SsoDBContext ssoDBContext)
        {
            _PlanningDBContext = planningDBContext;
            _SsoDBContext = ssoDBContext;

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
            const int Eng = 3578;
            const int SectionHead = 3581;
            const int directorateDirector = 3579;
            const int administrationDirector = 3577;
            const int MOEOrganization = 2;
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
                if (getData.Donor == 1292 || getData.Donor ==  4674 || getData.Donor ==  4673) {
                    TblUsers user = await _SsoDBContext.TblUsers.FirstOrDefaultAsync(X =>  X.position.Equals(SectionHead) && X.Section.Equals(getData.Donor));
                    userNameDB[33] = user.username;
                }else
                {
                    userNameDB[33] = "sectionHead";
                }
                       
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            OutputActivityData infoX = new OutputActivityData
            {
                requestSerial = RequestSerial,
                steps = steps,
                names = userNameDB,
                screens = forms

            };
            context.Output = infoX;
            return Done();




        }

    }
}
