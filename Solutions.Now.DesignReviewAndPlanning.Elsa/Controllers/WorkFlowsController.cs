using Elsa.Activities.Signaling.Services;
using Elsa.Attributes;
using Elsa.Models;
using Elsa.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Solutions.Now.DesignReviewAndPlanning.Elsa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solutions.Now.DesignReviewAndPlanning.Elsa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkFlowsController : ControllerBase
    {
        ISignaler _signaler;
        public WorkFlowsController(ISignaler signaler)
        {
            _signaler = signaler;
        }

        [Route("Request/{workflowName}/{id}")]
        [HttpGet]
        public async Task<IActionResult> Requset(string workflowName,int id)
        {
            OutputActivityData data = new OutputActivityData { requestSerial = id };
            await _signaler.TriggerSignalAsync(workflowName, input:data);
            return Ok();
        }
        [Route("Request/{workflowName}/{id}/{userName:alpha}")]
        [HttpGet]
        public async Task<IActionResult> Requset(string workflowName, int id,string userName)
        {
            var data = new DataForRequestProject
            {
                requestSerial = id,
                userName = userName
            };
            await _signaler.TriggerSignalAsync(workflowName, input: data);
            return Ok();
        }
        [Route("Request/{workflowName}/{id}/{major}/{userName}")]
        [HttpGet]
        public async Task<IActionResult> Requset(string workflowName, int id, string userName, int major)
        {
            var data = new DataForRequestProject
            {
                requestSerial = id,
                userName = userName,
                major = major
            };
            await _signaler.TriggerSignalAsync(workflowName, input: data);
            return Ok();
        }
        [Route("Request/{workflowName}/{id}/{major:int}")]
        [HttpGet]
        public async Task<IActionResult> Requset(string workflowName, int id, int major)
        {
            var data = new DataForRequestProject
            {
                requestSerial = id,
                major = major
            };
            await _signaler.TriggerSignalAsync(workflowName, input: data);
            return Ok();
        }
        [Route("RequestStage/{workflowName}/{id}/{stage:int}")]
        [HttpGet]
        public async Task<IActionResult> RequsetStage(string workflowName, int id, int stage)
        {
            var data = new DataForRequestProject
            {
                requestSerial = id,
                Stages = stage
            };
            await _signaler.TriggerSignalAsync(workflowName, input: data);
            return Ok();
        }

    }
}
