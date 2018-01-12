using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRCWhatIFRequestAPI.Repositories.Workflow;
using IRCWhatIFRequestAPI.Models;

namespace IRCWhatIFRequestAPI.Repositories.Workflow
{
    interface IInforWorkFlow
    {
        List<ApprovalTemplate> getApprovalTeamplates(CommonWhatifRequestQueryModel model);
        List<ApprovalTemplateStage> getApprovalTeamplateStage(int TemplateID);
    }
}
