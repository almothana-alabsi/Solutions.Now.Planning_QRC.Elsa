using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using Solutions.Now.DesignReviewAndPlanning.Elsa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa.Activities
{

    [Activity(
         Category = "Approval",
         DisplayName = "Add Main Project Planning",
         Description = "Add Main Project Planning",
         Outcomes = new[] { OutcomeNames.Done }
     )]
    public class MainProjectPlanningUsers : Activity
    {
        private readonly PlanningDBContext _PlanningDBContext;
        private readonly SsoDBContext _SsoDBContext;
        private readonly ILogger<MainProjectPlanningUsers> _logger;


        public MainProjectPlanningUsers(PlanningDBContext planningDBContext, SsoDBContext ssoDBContext, ILogger<MainProjectPlanningUsers> logger)
        {
            _PlanningDBContext = planningDBContext;
            _SsoDBContext = ssoDBContext;
            _logger = logger;


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
                TblUsers user = await _SsoDBContext.TblUsers.FirstOrDefaultAsync(X => X.Permission != 0 && X.position.Equals(SectionHead) && X.Section.Equals(MapSection));
                userNameDB[0] = userNameDB[2] = user.username;
                // user = await _PlanningDBContext.TblUsers.FirstOrDefaultAsync(X => X.Permission != 0 && X.username == getData.EngOfMap);
                userNameDB[1] = getData.EngOFMap;
                user = await _SsoDBContext.TblUsers.FirstOrDefaultAsync(X => X.Permission != 0 && X.position.Equals(directorateDirector) && X.Section.Equals(null) && X.Administration.Equals(29) && X.organization.Equals(2) && X.Directorate.Equals(null));
                userNameDB[3] = userNameDB[14] = user.username;
                user = await _SsoDBContext.TblUsers.FirstOrDefaultAsync(X => X.Permission != 0 && X.position.Equals(administrationDirector) && X.Section.Equals(null) && X.Administration.Equals(29) && X.organization.Equals(MOEOrganization) && X.Directorate.Equals(null));
                userNameDB[4] = userNameDB[15] = user.username;
                user = await _SsoDBContext.TblUsers.FirstOrDefaultAsync(X => X.Permission != 0 && X.position.Equals(SectionHead) && X.Section.Equals(4668));
                userNameDB[5] = userNameDB[7] = user.username;
                user = await _SsoDBContext.TblUsers.FirstOrDefaultAsync(X => X.Permission != 0 && X.position.Equals(SectionHead) && X.Section.Equals(4667));
                userNameDB[8] = userNameDB[10] = user.username;
                user = await _SsoDBContext.TblUsers.FirstOrDefaultAsync(X => X.Permission != 0 && X.position.Equals(Eng) && X.Section.Equals(4668));
                userNameDB[6] = user.username;
                user = await _SsoDBContext.TblUsers.FirstOrDefaultAsync(X => X.Permission != 0 && X.position.Equals(Eng) && X.Section.Equals(4667));
                userNameDB[9] = user.username;
                user = await _SsoDBContext.TblUsers.FirstOrDefaultAsync(X => X.Permission != 0 && X.position.Equals(SectionHead) && X.Section.Equals(HR));
                userNameDB[11] = userNameDB[13] = user.username;
                user = await _SsoDBContext.TblUsers.FirstOrDefaultAsync(X => X.Permission != 0 && X.position.Equals(Eng) && X.Section.Equals(HR));
                userNameDB[12] = user.username;
                user = await _SsoDBContext.TblUsers.FirstOrDefaultAsync(X => X.Permission != 0 && X.position.Equals(directorateDirector) && X.Section == null && X.Administration.Equals(39) && X.Directorate.Equals(43));
                userNameDB[16] = userNameDB[23] = user.username;

                user = await _SsoDBContext.TblUsers.FirstOrDefaultAsync(X => X.Permission != 0 && X.position.Equals(SectionHead) && X.Section.Equals(AcquisitionSection));
                userNameDB[17] = userNameDB[19] = user.username;
                user = await _SsoDBContext.TblUsers.FirstOrDefaultAsync(X => X.Permission != 0 && X.position.Equals(Eng) && X.Section.Equals(AcquisitionSection));
                userNameDB[18] = user.username;
                user = await _SsoDBContext.TblUsers.FirstOrDefaultAsync(X => X.Permission != 0 && X.position.Equals(administrationDirector) && X.Administration.Equals(DevelopmentCoordinationUnit));
                userNameDB[24]= userNameDB[26] = user.username;

                user = await _SsoDBContext.TblUsers.FirstOrDefaultAsync(X => X.position.Equals(SectionHead) && X.Section.Equals(102) && X.Permission != 0);
                userNameDB[20] = userNameDB[22] = user.username;
                user = await _SsoDBContext.TblUsers.FirstOrDefaultAsync(X => X.position.Equals(Eng) && X.Section.Equals(102) && X.Permission != 0);
                userNameDB[21] = user.username;
                //user = await _PlanningDBContext.TblUsers.FirstOrDefaultAsync(X => X.Administration == 30 && X.position == 3577 && X.Permission != 0);
                //userNameDB[24] = userNameDB[26] = user.username;
            
                user = await _SsoDBContext.TblUsers.FirstOrDefaultAsync(X => X.Administration == 39 && X.position == 3579 && X.Permission != 0 && X.Directorate == 41);
                userNameDB[28] = userNameDB[32] = user.username;
                user = await _SsoDBContext.TblUsers.FirstOrDefaultAsync(X => X.position.Equals(3577) && X.Administration.Equals(39)&& X.Directorate == 41 && X.Section == 125 && X.Permission != 0);
                userNameDB[27] = user.username;
                //userNameDB[18]=  user.username;
                //user = await _PlanningDBContext.TblUsers.FirstOrDefaultAsync(X=>X.Permission != 0 &&  X.position.Equals(administrationDirector) && X.Section.Equals(HR));
                //userNameDB[19]= user.username;
                //user = await _PlanningDBContext.TblUsers.FirstOrDefaultAsync(X=>X.Permission != 0 &&  X.position.Equals(SectionHead)&& X.Section.Equals(AcquisitionSection));
                //userNameDB[20]= userNameDB[22]= user.username;
                //user = await _PlanningDBContext.TblUsers.FirstOrDefaultAsync(X=>X.Permission != 0 &&  X.position.Equals(EngOfMap11) && X.Section.Equals(AcquisitionSection));
                //userNameDB[21] = user.username;
                //user = await _PlanningDBContext.TblUsers.FirstOrDefaultAsync(X => X.Permission != 0 && X.position.Equals(directorateDirector) && X.Section.Equals(AcquisitionSection));
                //userNameDB[23] = user.username;
                //user = await _PlanningDBContext.TblUsers.FirstOrDefaultAsync(X=>X.Permission != 0 && X.position.Equals(administrationDirector) && X.Section.Equals(AcquisitionSection));
                //userNameDB[24]=  user.username;
                //   user = await _PlanningDBContext.TblUsers.FirstOrDefaultAsync(X=>X.Permission != 0 &&  X.position.Equals(administrationDirector) && X.Section.Equals(AcquisitionSection));
                //   userNameDB[26] = user.username;
                user = await _SsoDBContext.TblUsers.FirstOrDefaultAsync(X => X.Permission != 0 && X.position == 3581 && X.Section == 200);
                userNameDB[29] = userNameDB[31] = user.username;
                
              /*  user = await _PlanningDBContext.TblUsers.FirstOrDefaultAsync(X => X.Permission != 0 && X.Administration == 31 && X.Directorate == 59 && X.Section == null);
                userNameDB[33] = user.username;*/
                //user = await _PlanningDBContext.TblUsers.FirstOrDefaultAsync(X => X.Permission != 0 && X.position == 3579 && X.Administration == 39 && X.Directorate == 41);
                //userNameDB[33] = user.username;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the MainProjectPlanningUsers activity.");
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
