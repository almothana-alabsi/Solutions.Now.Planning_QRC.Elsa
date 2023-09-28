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
       Category = "Update Signal",
       DisplayName = "Update Signal",
       Description = "Update Signal in ApprovalHistory Table",
       Outcomes = new[] { OutcomeNames.Done }
   )]
    public class updateSignalForManagerOfDevelopmentCoordinationPlanning : Activity
    {
        private readonly PlanningDBContext _planningDBContext;
        public IConfiguration Configuration { get; }
        public updateSignalForManagerOfDevelopmentCoordinationPlanning(PlanningDBContext planningDBContext)
        {
            _planningDBContext = planningDBContext;
        }


        [ActivityInput(Hint = "Enter an expression that evaluates to the Request serial.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public int RequestSerial { get; set; }

        [ActivityInput(Hint = "Enter an expression that evaluates to the Request Type.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public int RequestType { get; set; }

        [ActivityInput(Hint = "Enter an expression that evaluates to the Step.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public int Step { get; set; }

        [ActivityInput(Hint = "Enter an expression that evaluates to URL.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public string URL { get; set; }

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            try
            {

               var approvalHistory = await _planningDBContext.ApprovalHistory.AsQueryable().Where(x => x.requestSerial == RequestSerial
                && x.requestType == RequestType && x.step == Step && x.actionDate == null).OrderByDescending(s => s.serial).ToListAsync<ApprovalHistory>();
                if (approvalHistory.Count > 0) 
                {
                    approvalHistory[0].URL= URL;
                    await _planningDBContext.SaveChangesAsync();
                }
        


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message.ToString());
            }
            return Done();
        }
    }
}
