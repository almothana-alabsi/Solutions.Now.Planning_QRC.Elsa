using Elsa;
using Elsa.Services;
using Elsa.Attributes;
using Solutions.Now.DesignReviewAndPlanning.Elsa.Models;
using Microsoft.Data.SqlClient;
using System;
using Elsa.Expressions;
using Elsa.ActivityResults;
using System.Threading.Tasks;
using Elsa.Services.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa.Activities
{
    [Activity(
         Category = "Flags",
         DisplayName = "Flag For Check Update Signal Planning",
         Description = "Flag For Check Update Signal Planning",
         Outcomes = new[] { OutcomeNames.Done }
     )]
    public class FlagForCheckUpdateSignal : Activity
    {
        private readonly PlanningDBContext _planningDBContext;
        public IConfiguration Configuration { get; }
        public FlagForCheckUpdateSignal(PlanningDBContext planningDBContext)
        {
            _planningDBContext = planningDBContext;
        }


        [ActivityInput(Hint = "Enter an expression that evaluates to the Request serial.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public int RequestSerial { get; set; }

        [ActivityInput(Hint = "Enter an expression that evaluates to the Request Type.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public int RequestType { get; set; }

    


        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            int flag = 0;
            try
            {
               
                    var approvalHistory = await _planningDBContext.ApprovalHistory.AsQueryable().Where(x => x.requestSerial == RequestSerial
                     && x.requestType == RequestType && x.step == 24 && x.actionDate == null).OrderByDescending(s => s.serial).ToListAsync<ApprovalHistory>();
                    if (approvalHistory.Count > 0)
                    {
                        flag = 1;
                    }
                
                var approvalHistoryForEngDCU = await _planningDBContext.ApprovalHistory.AsQueryable().Where(x => x.requestSerial == RequestSerial
                 && x.requestType == RequestType && x.step == 25 && x.actionDate == null).OrderByDescending(s => s.serial).ToListAsync<ApprovalHistory>();
                if (approvalHistoryForEngDCU.Count > 0)
                {
                    flag = 2;
                }
        
                    var approvalHistoryForSecionHead26DCU = await _planningDBContext.ApprovalHistory.AsQueryable().Where(x => x.requestSerial == RequestSerial
                && x.requestType == RequestType && x.step == 26 && x.actionDate == null).OrderByDescending(s => s.serial).ToListAsync<ApprovalHistory>();
                    if (approvalHistoryForSecionHead26DCU.Count > 0)
                    {
                        flag = 3;
                    }
                
                context.Output = flag;




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message.ToString());
            }
            return Done();
        }
    }
}
