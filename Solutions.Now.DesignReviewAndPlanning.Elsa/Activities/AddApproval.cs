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
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Amazon.SimpleEmail.Model;
using System.Text;
using System.Net;
using Solutions.Now.DesignReviewAndPlanning.Elsa.Integrations;
using System.Security.Policy;
using PhoneNumbers;
using Microsoft.Extensions.Logging;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa.Activities
{
    [Activity(
       Category = "Approval",
       DisplayName = "Add approval",
       Description = "Add approval in ApprovalHistory Table",
       Outcomes = new[] { OutcomeNames.Done }
   )]
    public class AddApproval : Activity
    {
        private readonly PlanningDBContext _planningDBContext;
        private  Email _email;
        private readonly SsoDBContext _ssoDBContext;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ILogger<MainProjectPlanningUsers> _logger;



        public IConfiguration Configuration { get; }
        public AddApproval(IConfiguration configuration, PlanningDBContext planningDBContext,SsoDBContext ssoDBContext,Email email, ILogger<MainProjectPlanningUsers> logger)
        {
            _planningDBContext = planningDBContext;
            _configuration = configuration;
            _ssoDBContext = ssoDBContext;
            _email = email;
            _logger = logger;

        }


        [ActivityInput(Hint = "Enter an expression that evaluates to the Request serial.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public int RequestSerial { get; set; }

        [ActivityInput(Hint = "Enter an expression that evaluates to the Request Type.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public int RequestType { get; set; }

        [ActivityInput(Hint = "Enter an expression that evaluates to the Step.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public int Step { get; set; }

        [ActivityInput(Hint = "Enter an expression that evaluates to the Name.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public string ActionBy { get; set; }

        [ActivityInput(Hint = "Enter an expression that evaluates to URL.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public string URL { get; set; }

        [ActivityInput(Hint = "Enter an expression that evaluates to the Form.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public string Form { get; set; }
        [ActivityInput(Hint = "Enter an expression that evaluates to the refserial.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public int? refSerial { get; set; }
        [ActivityInput(Hint = "Enter an expression that evaluates to the Major.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public int? Major { get; set; }
        [ActivityInput(Hint = "Enter an expression that evaluates to the Status.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public int? Status { get; set; }
        [ActivityInput(Hint = "Enter an expression that evaluates to the CreatedBy.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public string? createdBy { get; set; }

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                if (Status == null) { if (RequestType == 4666) { Status = 387; } else { Status = 385; } };
                ApprovalHistory approvalHistory = new ApprovalHistory
                {
                    requestSerial = RequestSerial,
                    requestType = RequestType,
                    createdDate = DateTime.Now,
                    actionBy = ActionBy,
                    expireDate = DateTime.Today.AddDays(10),
                    step = Step,
                    URL = URL,
                    Form = Form + RequestSerial.ToString(),
                    status = Status,
                    ActionDetails = (Major != null? Major:-1),
                    refSerial = refSerial,
                    createdBy = createdBy
                };
                //await _cmis2DbContext.ApprovalHistory.AddAsync(approvalHistory);
                // await _cmis2DbContext.SaveChangesAsync();
              //  var @connectionString = Configuration.GetConnectionString("Server=207.180.223.162;Uid=Sa;Pwd=SolNowDev23;Database=DesignReview");
                SqlConnection connection = new SqlConnection(connectionString);
    
                    string query = "INSERT INTO [MOE-planning-DB].[Plan].[ApprovalHistory] ([requestserial] ,[requestType] ,[createdDate],[actionBy],[expireDate],[status],[URL],[Form],[step],[ActionDetails],createdBy) ";
                    query = query + " values (" + approvalHistory.requestSerial + ", " + approvalHistory.requestType + ",  GETDATE(), '" + approvalHistory.actionBy + "', GETDATE()+10 , " + approvalHistory.status + ", '" + approvalHistory.URL + "', '" + approvalHistory.Form + "', " + approvalHistory.step + "," + approvalHistory.ActionDetails + ",'"+approvalHistory.createdBy+"');";
                    SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Records Inserted Successfully");
                   
                }


                catch (SqlException e)
                {
                    _logger.LogError(e.InnerException.Message, "An error occurred while executing the Add Approval activity.");
                }
                finally
                {
                    connection.Close();
                }
                var user = await _ssoDBContext.TblUsers.OrderBy(x => x.serial).FirstOrDefaultAsync(y => y.username.Equals(approvalHistory.actionBy));
                if (Int32.Parse(_configuration["SMS:flag"]) == 1)
                {
                    if (user != null)
                    {
                        if (user.phoneNumber != null)
                        {
                            if (user.phoneNumber.Length == 12 && user.phoneNumber.StartsWith("962"))
                            {
                                string apiUrlSMS = _configuration["SMS:URL"];
                                string url = apiUrlSMS + user.phoneNumber.ToString() + "&createdBy="+approvalHistory.actionBy.ToString()+"&requsetType=4666&requestSerial=" + approvalHistory.requestSerial.ToString() + "&lang=ar&isFYI=0";
                                  System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                  HttpClientHandler handler = new HttpClientHandler
                                  {
                                      ServerCertificateCustomValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; },
                                  };

                                using (var httpClient = new HttpClient(handler))
                                {
                                    HttpResponseMessage response = await httpClient.GetAsync(url);
                                    if (response.IsSuccessStatusCode)
                                    {

                                        Console.WriteLine("Successfully send");

                                    }
                                    else
                                    {

                                        _logger.LogError(response.Content.ToString(), "An error occurred while executing the SMS activity.");

                                    }
                                }
                            }
                        }
                        if (Int32.Parse(_configuration["EmailApi:flag"]) == 1)
                        {
                            if (user.email != null)
                            {
                                if (_email.IsValidEmail(user.email))
                                {
                                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                    HttpClientHandler handler = new HttpClientHandler
                                    {
                                        ServerCertificateCustomValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; },

                                    };


                                    using (var httpClient = new HttpClient(handler))
                                    {
                                        string url = await _email.SendEmail(approvalHistory.actionBy, 4666, approvalHistory.requestSerial, "ar",0);
                                        _logger.LogError(url, "URL");

                                        HttpResponseMessage response = await httpClient.GetAsync(url);
                                        if (response.IsSuccessStatusCode)
                                        {
                                            Console.WriteLine("Successfully send");
                                        }
                                        else
                                        {
                                            _logger.LogError(response.Content.ToString(), "An error occurred while executing the Email activity.");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message.ToString(), "An error occurred while executing the Add Approval activity.");
            }
            return Done();
        }
    }
}
