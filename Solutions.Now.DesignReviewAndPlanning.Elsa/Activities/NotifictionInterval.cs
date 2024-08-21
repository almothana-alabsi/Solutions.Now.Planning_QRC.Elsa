using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Solutions.Now.DesignReviewAndPlanning.Elsa.Integrations;
using Solutions.Now.DesignReviewAndPlanning.Elsa.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa.Activities
{
    [Activity(
      Category = "Notifiction",
      DisplayName = "Notifiction Interval",
      Description = "Notifiction in ApprovalHistory Table",
      Outcomes = new[] { OutcomeNames.Done }
  )]
    public class NotifictionInterval : Activity
    {

        private readonly PlanningDBContext _planningDBContext;
        private readonly IConfiguration _configuration;
        private readonly SsoDBContext _ssoDBContext;
        private readonly HttpClient _httpClient;
        private Email _email;
        private readonly ILogger<NotifictionInterval> _logger;

        public NotifictionInterval(IConfiguration configuration, PlanningDBContext planningDBContext , SsoDBContext ssoDBContext , Email email, ILogger<NotifictionInterval> logger)
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

        [ActivityInput(Hint = "Enter an expression that evaluates to the Steps.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public object steps { get; set; }

        [ActivityInput(Hint = "Enter an expression that evaluates to the Names.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public object userNameDB { get; set; }

        [ActivityInput(Hint = "Enter an expression that evaluates to the Forms.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public object forms { get; set; }
        [ActivityInput(Hint = "Enter an expression that evaluates to Status.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public int Status { get; set; }
        [ActivityInput(Hint = "Enter an expression that evaluates to From.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public int from { get; set; }
        [ActivityInput(Hint = "Enter an expression that evaluates to To.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public int to { get; set; }
        [ActivityInput(Hint = "Enter an expression that evaluates to the refserial.", DefaultSyntax = SyntaxNames.Literal, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public int? refSerial { get; set; }

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            IList<int> _stepsList = new List<int>();
            IList<string> _userNameList = new List<string>();
            IList<string> _formsList = new List<string>();
            string word = "", _steps = (string)steps, _userName = (string)userNameDB, _forms = (string)forms;
            for (int i = 0; i < _steps.Length; i++)
            {
                if (_steps[i].ToString().Equals(","))
                {
                    _stepsList.Add(Int32.Parse(word));
                    word = "";
                }
                else
                {
                    word += _steps[i];
                }
                if (i == _steps.Length - 1)
                {
                    _stepsList.Add(Int32.Parse(word));
                    word = "";
                }
            }
            for (int i = 0; i < _userName.Length; i++)
            {
                if (_userName[i].ToString().Equals(","))
                {
                    _userNameList.Add(word);
                    word = "";
                }
                else
                {
                    word += _userName[i];
                }
                if (i == _userName.Length - 1)
                {
                    _userNameList.Add(word);
                    word = "";
                }
            }
            for (int i = 0; i < _forms.Length; i++)
            {
                if (_forms[i].ToString().Equals(","))
                {
                    _formsList.Add(word);
                    word = "";
                }
                else
                {
                    word += _forms[i];
                }
                if (i == _forms.Length - 1)
                {
                    _formsList.Add(word);
                    word = "";
                }
            }

            for (int i = from; i < Math.Min(to + 1, _userNameList.Count); i++)
            {
                int mn = Math.Min(i, _stepsList.Count - 1);
                var approvalHistory = new ApprovalHistory
                {
                    step = _stepsList[mn],
                    requestSerial = RequestSerial,
                    requestType = RequestType,
                    createdDate = DateTime.Now,
                    actionBy = _userNameList[i],
                    actionDate = DateTime.Now,
                    expireDate = DateTime.Today.AddDays(10),
                    Form = _formsList[0] + RequestSerial,
                    status = Status,
                    seen = null,
                    refSerial = refSerial
                };
                try
                {
                   
                   // var @connectionString = "Server=207.180.223.162;Uid=Sa;Pwd=SolNowDev23;Database=DesignReview";
                    SqlConnection connection = new SqlConnection(connectionString);
                    if (refSerial != null)
                    {
                        string query = "INSERT INTO [MOE-planning-DB].[Plan].[ApprovalHistory] ([requestserial] ,[requestType] ,[createdDate],[actionBy],[actionDate],[expireDate],[status],[URL],[Form],[step],[refserial]) ";
                        query = query + " values (" + approvalHistory.requestSerial + ", " + approvalHistory.requestType + ",  GETDATE(), '" + approvalHistory.actionBy + "', GETDATE(), GETDATE() , " + approvalHistory.status + ", '" + approvalHistory.URL + "', '" + approvalHistory.Form + "', " + approvalHistory.step + "," + approvalHistory.refSerial + ");";
                        SqlCommand command = new SqlCommand(query, connection);
                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            Console.WriteLine("Records Inserted Successfully");
                        }
                        catch (SqlException e)
                        {
                            Console.WriteLine("Error Generated. Details: " + e.ToString());
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                    else {
                        string query = "INSERT INTO [MOE-planning-DB].[Plan].[ApprovalHistory] ([requestserial] ,[requestType] ,[createdDate],[actionBy],[actionDate],[expireDate],[status],[URL],[Form],[step]) ";
                        query = query + " values (" + approvalHistory.requestSerial + ", " + approvalHistory.requestType + ",  GETDATE(), '" + approvalHistory.actionBy + "', GETDATE(), GETDATE() , " + approvalHistory.status + ", '" + approvalHistory.URL + "', '" + approvalHistory.Form + "', " + approvalHistory.step + ");";
                        SqlCommand command = new SqlCommand(query, connection);
                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            Console.WriteLine("Records Inserted Successfully");
                        }
                        catch (SqlException e)
                        {
                            Console.WriteLine("Error Generated. Details: " + e.ToString());
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                    var user = await _ssoDBContext.TblUsers.OrderBy(x => x.serial).FirstOrDefaultAsync(y => y.username.Equals(approvalHistory.actionBy));
                    if (Int32.Parse(_configuration["SMS:flagFYI"]) == 1)
                    {
                        if (user != null)
                        {
                            if (user.phoneNumber != null)
                            {
                                if (user.phoneNumber.Length == 12 && user.phoneNumber.StartsWith("962"))
                                {
                                    string apiUrlSMS = _configuration["SMS:URL"];
                                    string url = apiUrlSMS + user.phoneNumber.ToString() + "&createdBy=" + approvalHistory.actionBy.ToString()+"&requsetType=4666&requestSerial=" + approvalHistory.requestSerial.ToString() + "&lang=ar&isFYI=1";
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
                            if (Int32.Parse(_configuration["EmailApi:flagFYI"]) == 1)
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
                                            string url = await _email.SendEmail(approvalHistory.actionBy, 4666, approvalHistory.requestSerial, "ar",1);
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
                    _logger.LogError(ex.Message, "An error occurred while executing the Add Approval activity.");
                }
            }
            return Done();


        }
    }
}
