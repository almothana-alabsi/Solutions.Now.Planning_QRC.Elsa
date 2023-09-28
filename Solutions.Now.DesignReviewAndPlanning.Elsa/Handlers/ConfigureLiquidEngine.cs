using System.Threading.Tasks;
using System.Threading;
using Fluid;
using MediatR;
using Solutions.Now.DesignReviewAndPlanning.Elsa.Models;
using Elsa.Scripting.Liquid.Messages;
using Solutions.Now.DesignReviewAndPlanning.Elsa.Models.DTOs;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa.Handlers
{
    public class ConfigureLiquidEngine : INotificationHandler<EvaluatingLiquidExpression>
    {
        public Task Handle(EvaluatingLiquidExpression notification, CancellationToken cancellationToken)
        {
            notification.TemplateContext.Options.MemberAccessStrategy.Register<OutputActivityData>();
            notification.TemplateContext.Options.MemberAccessStrategy.Register<EngineeringOfPlanningDTO>();
            notification.TemplateContext.Options.MemberAccessStrategy.Register<DataForRequestProject>();
            notification.TemplateContext.Options.MemberAccessStrategy.Register<RejectToSectionsFlagsPlanningDTO>();

            notification.TemplateContext.Options.MemberAccessStrategy.Register<string>();
            return Task.CompletedTask;
        }
    }
}
