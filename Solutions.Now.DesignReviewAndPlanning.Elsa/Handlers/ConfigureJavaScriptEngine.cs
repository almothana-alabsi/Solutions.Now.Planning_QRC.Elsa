using System.Threading.Tasks;
using System.Threading;
using Fluid;
using MediatR;
using Solutions.Now.DesignReviewAndPlanning.Elsa.Models;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Scripting.JavaScript.Options;
using Elsa.Scripting.JavaScript.Extensions;
using Elsa.Scripting.JavaScript.Events;
using Solutions.Now.DesignReviewAndPlanning.Elsa.Models.DTOs;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa.Handlers
{
    public class ConfigureJavaScriptEngine : INotificationHandler<EvaluatingJavaScriptExpression>
    {
        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var engine = notification.Engine;
            engine.RegisterType<OutputActivityData>();
            engine.RegisterType<RejectToSectionsFlagsPlanningDTO>();
            engine.RegisterType<EngineeringOfPlanningDTO>();
            engine.RegisterType<OutputActivityDataWithName>();

            engine.RegisterType<string>();
            engine.RegisterType<int>();

            return Task.CompletedTask;
        }

    }
}
