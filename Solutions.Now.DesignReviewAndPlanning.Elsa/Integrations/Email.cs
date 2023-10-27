using Amazon.Glacier;
using Amazon.IdentityManagement.Model;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using MimeKit;
using Solutions.Now.DesignReviewAndPlanning.Elsa.Models;
using System.Net.Http;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa.Integrations
{
    public class Email
    {
        private readonly PlanningDBContext _planningDBContext;
        private readonly SsoDBContext _ssoDBContext;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public Email(IConfiguration configuration, PlanningDBContext planningDBContext, SsoDBContext ssoDBContext)
        {
            _planningDBContext = planningDBContext;
            _configuration = configuration;
            _ssoDBContext = ssoDBContext;
        }
        public async Task<string> SendEmail(string actionBy, int requsetType, int? requestSerial, string lang) {
            string URL = _configuration["EmailApi:URL"];
            string descEn = _configuration["EmailApi:descEn"];
            string descAr = _configuration["EmailApi:descAr"];
            string urlEmail = "";


            try
            {
                var langFilter = lang.ToLower();
                string? descMSG = "";
                string? projectData = "";
                var desc = await _ssoDBContext.MasterData.OrderBy(y => y.serial).FirstOrDefaultAsync(x => x.serial == requsetType);
                if (requestSerial != null) { 
                if (requsetType == 4666)
                {
                    projectData = _planningDBContext.MainProject_Planning.OrderBy(x => x.Serial == requestSerial).FirstOrDefault(y => y.Serial == requestSerial).ProjectName;


                    if (langFilter == "en")
                    {
                        descMSG = descEn + " " + desc.descEN + " / " + projectData;
                    }
                    else
                    {
                        descMSG = descAr + " " + desc.descAR + " / " + projectData;

                    }
                }
                else
                {
                    if (langFilter == "en")
                    {
                        descMSG = descEn + " " + desc.descEN;
                    }
                    else
                    {
                        descMSG = descAr + " " + desc.descAR;

                    }
                }
                 var user = await _ssoDBContext.TblUsers.OrderBy(x => x.serial).FirstOrDefaultAsync(y => y.username.Equals(actionBy));
                    if (user != null)
                    {
                        if (user.email != null)
                        {
                             urlEmail = URL + user.email.ToString() + "&messege=" + descMSG;
                        }
                    }

                }
                return urlEmail;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return urlEmail;
            }

        }

            public bool IsValidEmail(string email)
            {
                var trimmedEmail = email.Trim();

                if (trimmedEmail.EndsWith("."))
                {
                    return false;
                }
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    return addr.Address == trimmedEmail;
                }
                catch
                {
                    return false;
                }
            }     
    }
}
