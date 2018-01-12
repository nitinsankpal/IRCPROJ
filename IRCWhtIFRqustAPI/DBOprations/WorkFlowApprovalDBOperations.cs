using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRCWhatIFRequestAPI.Models;


namespace IRCWhatIFRequestAPI.DBOprations
{
    public class WorkFlowApprovalDBOperations
    {
        public List<ApprovalTemplate> getApprovalTeamplates(CommonWhatifRequestQueryModel model)
        {
            List<ApprovalTemplate> Approvaltemplates = new List<ApprovalTemplate>();

            Approvaltemplates.Add(new ApprovalTemplate
            {
                ID = "1",
                Name = "Template ION RAM",
                Description = "Template Desc ION RAM",
                Status = "0",
                Application = "All",
                Operation = "Infor Request - Role Assignment Management"
            });
            Approvaltemplates.Add(new ApprovalTemplate
            {
                ID = "1",
                Name = "Template ION UC",
                Description = "Template Desc User Creation",
                Status = "0",
                Application = "All",
                Operation = "Infor Request - User Creation Management"
            });

            return Approvaltemplates;
        }
        public List<ApprovalTemplateStage> getApprovalTeamplateStage(int TemplateID)
        {
            List<ApprovalTemplateStage> ApprovaltemplatesStage = new List<ApprovalTemplateStage>();

            ApprovaltemplatesStage.Add(new ApprovalTemplateStage
            {
                ID = 1,
                ProfileID = 1,
                ApproverID = 1,
                OperationStatus = "0",
                Name = "Primary Approval",
                Description = "This is default stage shipped with Default Template",
                ApproversRequired = "1",
                ApproversRequiredFlag = "any",

                SerialNo = "1",

                StageType = "0",

                UserID = "Manager of User",

                FirstName = "",
                LastName = "",

                MitigationStatus = "0",

                BypassForNoViolations = "0",

                BypassForNoApprover = "0",

                BypassForNoUnMitigatedViolations = "0",

                AllowRoleOwner = "0",

                ReviewAfter = "0",

                EscalationAfter = "0",

                IsEscalated = true,

                IsEscalationApprover = false,

                ApproverComments = "NO",

                GroupName = "",

                ApprovalSetting = "",

                ResolveFQNonly = ""

            });

            return ApprovaltemplatesStage;

        }

    }
}