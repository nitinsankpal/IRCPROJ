using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IRCWhatIFRequestAPI.Repositories.Workflow;
using IRCWhatIFRequestAPI.Models;
using IRCWhatIFRequestAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IRCWhatIFRequestAPI.Controllers
{
   
    public class InforWorkflowController : Controller
    {
        [HttpPost]
        [Route("GetApprovalTemplate")]
        public IEnumerable<ApprovalTemplate> getApprovalTeamplates([FromBody]CommonWhatifRequestQueryModel Queryparam)
        {
            IInforWorkFlow Obj = new InforWorkFlow();
            return Obj.getApprovalTeamplates(Queryparam);
        }

        [HttpPost]
        [Route("GetApprovalTemplatestage")]
        public IEnumerable<ApprovalTemplateStage> getApprovalTeamplateStage([FromBody]int TemplateID)
        {
            IInforWorkFlow Obj = new InforWorkFlow();
            return Obj.getApprovalTeamplateStage(TemplateID);
        }
    }
}
