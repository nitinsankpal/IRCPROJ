using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRCWhatIFRequestAPI.Models
{
    public class ApprovalTemplate
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Application { get; set; }
        public string Operation { get; set; }
    }
    public class ApprovalTemplateStage
    {
        public int ID { get; set; }
        public int ProfileID { get; set; }
        public int ApproverID { get; set; }
        public string OperationStatus { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ApproversRequired { get; set; }
        public string ApproversRequiredFlag { get; set; }
        public string SerialNo { get; set; }
        public string StageType { get; set; }
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MitigationStatus { get; set; }
        public string BypassForNoViolations { get; set; }
        public string BypassForNoApprover { get; set; }
        public string BypassForNoUnMitigatedViolations { get; set; }
        public string AllowRoleOwner { get; set; }
        public string ReviewAfter { get; set; }
        public string EscalationAfter { get; set; }
        public bool IsEscalated { get; set; }
        public bool IsEscalationApprover { get; set; }
        public string ApproverComments { get; set; }
        public string GroupName { get; set; }
        public string ApprovalSetting { get; set; }
        public string ResolveFQNonly { get; set; }
    }
}
