using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRCWhatIFRequestAPI.Repositories.Workflow;
using IRCWhatIFRequestAPI.Models;
using IRCWhatIFRequestAPI.DBOprations;
using IRCWhatIFRequestAPI.Services;

namespace IRCWhatIFRequestAPI.Repositories.Workflow
{
    public class InforWorkFlow : IInforWorkFlow 
    {
        public List<ApprovalTemplate>getApprovalTeamplates(CommonWhatifRequestQueryModel model)
        {
            try
            {
                WorkFlowApprovalDBOperations Approvaltemplate = new WorkFlowApprovalDBOperations();
                return Approvaltemplate.getApprovalTeamplates(model);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ApprovalTemplateStage> getApprovalTeamplateStage(int TemplateID)
        {
            try
            {
                WorkFlowApprovalDBOperations ApprovaltemplateStage = new WorkFlowApprovalDBOperations();
                return ApprovaltemplateStage.getApprovalTeamplateStage(TemplateID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
