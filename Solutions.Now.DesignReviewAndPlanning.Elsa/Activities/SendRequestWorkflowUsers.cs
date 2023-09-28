using Amazon.ElasticMapReduce.Model;
using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Solutions.Now.DesignReviewAndPlanning.Elsa.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa.Activities
{
    [Activity(
   Category = "Approval",
   DisplayName = "Send Request",
   Description = "Send Request to Workflow Table",
   Outcomes = new[] { OutcomeNames.Done }
)]
    public class SendRequestWorkflowUsers : Activity
    {


        private readonly IConfiguration _configuration;
        public SendRequestWorkflowUsers(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [ActivityInput(Hint = "Enter an expression that evaluates to the WorkFlow Signal.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public string WorkFlowSignal { get; set; }

        [ActivityInput(Hint = "Enter an expression that evaluates to the Request Serial.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public int RequestSerial { get; set; }
        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            // string connectionString = _configuration.GetConnectionString("DefaultConnection");
      
            string connectionString = _configuration.GetValue<string>("Elsa:Server:BaseUrl");

            try
            {
                string URL = connectionString+"/api/WorkFlows/Request/" + WorkFlowSignal + "/" + RequestSerial.ToString();
                context.Output = URL;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message.ToString());
            }
            return Done();
        }
    }
}
