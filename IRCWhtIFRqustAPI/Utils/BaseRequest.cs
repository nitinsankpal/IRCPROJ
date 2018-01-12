using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRCWhatIFRequestAPI.Utils
{
    public class BaseRequest
    {
        public int id;
        public string GenericRequestDetails;
        public string templateName;
        public DateTime requestActivationTime;
        public int liveverify;
        public int templateID;
        public int templateAutoCompletionFlag;
        public int templateAutoApprovalFlag;
        public string lastUpdatedBy;
        public DateTime lastUpdatedOn;
        public DateTime requestedOn;
        public int requestorID;
        public string requestor;
        public TimeSpan age;
        public DateTime expiration;
        public DateTime lastApprovedOn;
        public string approvalTemplateDetails;
        public string RequestDetails;
        public string applicationScope;
        public RequestFormat requestFormat;
        public int ApplicationID;
        public RequestPriority requestPriority;
        public RequestStatus requestStatus;
        public string sRequestType;
        public int nRequestType;
        public RequestType requestType;
        public int currentStage;
        public int totalStages;
        public string description;
        public string name;
        public string applicationname;

        public BaseRequest() { }
    }
}
