using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRCWhatIFRequestAPI.DBOprations
{
    public class ApprovalStageDBOperations
    {
        private bool callFromRequest = false;
        public string stagesNS = "";
        private const string requestNS = "http://www.approva.net/ApprovalRequest.xsd";
        private const string approvalTemplateNS = "http://www.approva.net/ApprovalTemplate.xsd";

        public ApprovalStageDBOperations()
        {
            callFromRequest = false;
            TemplateDBOperations tm = new TemplateDBOperations();
            stagesNS = tm.approvalTemplateNS;
        }
        public ApprovalStageDBOperations(bool falseFlag)
        {
            callFromRequest = falseFlag;// true;
            TemplateDBOperations tm = new TemplateDBOperations();
            stagesNS = tm.approvalTemplateNS;
        }
    }
}
