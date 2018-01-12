using IRCWhatIFRequestAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace IRCWhatIFRequestAPI.Services
{
    public class WhatIFRequestImplementaionService
    {

        #region Supporting Enums and Classes
        public class RequestObject
        {
            private XmlNode _RequestNode;

            public XmlNode RequestNode
            {
                get { return _RequestNode; }
                set { _RequestNode = value; }
            }
            private int _CreatedBy;

            public int createdBy
            {
                get { return _CreatedBy; }
                set { _CreatedBy = value; }
            }
            private int _directManagerID;

            public int directManagerID
            {
                get { return _directManagerID; }
                set { _directManagerID = value; }
            }
            private int _createUserAsID;

            public int createUserAsID
            {
                get { return _createUserAsID; }
                set { _createUserAsID = value; }
            }
            private int _ProfileID;

            public int ProfileID
            {
                get { return _ProfileID; }
                set { _ProfileID = value; }
            }
            private string _templateXML;

            public string templateXML
            {
                get { return _templateXML; }
                set { _templateXML = value; }
            }
            private bool _IsRemediated = false;

            public bool IsRemediated
            {
                get { return _IsRemediated; }
                set { _IsRemediated = value; }
            }
            private bool _modificationsOver = false;

            public bool modificationsOver
            {
                get { return _modificationsOver; }
                set { _modificationsOver = value; }
            }
            private int _actionTakenBy;

            public int actionTakenBy
            {
                get { return _actionTakenBy; }
                set { _actionTakenBy = value; }
            }
            private BaseRequest _reqInfo = null;

            public BaseRequest reqInfo
            {
                get { return _reqInfo; }
                set { _reqInfo = value; }
            }

            private string _CallingClient;

            public string CallingClient
            {
                get { return _CallingClient; }
                set { _CallingClient = value; }
            }

            private bool _isSDK = false;

            public bool isSDK
            {
                get { return _isSDK; }
                set { _isSDK = value; }
            }
            private bool _isInline = false;

            public bool isInline
            {
                get { return _isInline; }
                set { _isInline = value; }
            }
        }

        public class RequestInformationObject
        {
            private int _requestID;

            public int requestID
            {
                get { return _requestID; }
                set { _requestID = value; }
            }
            private int _createdBy;

            public int createdBy
            {
                get { return _createdBy; }
                set { _createdBy = value; }
            }
            private int _templateID;

            public int templateID
            {
                get { return _templateID; }
                set { _templateID = value; }
            }
            private string _templateXml;

            public string templateXml
            {
                get { return _templateXml; }
                set { _templateXml = value; }
            }
            private BaseRequest _reqInfo = null;

            public BaseRequest reqInfo
            {
                get { return _reqInfo; }
                set { _reqInfo = value; }
            }
            private XmlDocument _xDocApprovalSettings = null;

            public XmlDocument xDocApprovalSettings
            {
                get { return _xDocApprovalSettings; }
                set { _xDocApprovalSettings = value; }
            }
            private bool _HasGenericRequestDetails = false;

            public bool HasGenericRequestDetails
            {
                get { return _HasGenericRequestDetails; }
                set { _HasGenericRequestDetails = value; }
            }
        }
        #endregion Supporting Enums and Classes

        #region Member variables

        private string _approvalRequestNS = "http://www.approva.net/ApprovalRequest.xsd";

        protected string approvalRequestNS
        {
            get { return _approvalRequestNS; }
            set { _approvalRequestNS = value; }
        }
        private string _approvalTemplateNS = "http://www.approva.net/ApprovalTemplate.xsd";

        protected string approvalTemplateNS
        {
            get { return _approvalTemplateNS; }
            set { _approvalTemplateNS = value; }
        }
        private bool _IsVirtualAppsEnabled = true;

        protected bool IsVirtualAppsEnabled
        {
            get { return _IsVirtualAppsEnabled; }
            set { _IsVirtualAppsEnabled = value; }
        }
        private XmlDocument _xmlDocReq = null;

        protected XmlDocument xmlDocReq
        {
            get { return _xmlDocReq; }
            set { _xmlDocReq = value; }
        }
        private const string _NEVEREXPIRES = "1/1/1900";

        protected string NEVEREXPIRES
        {
            get { return _NEVEREXPIRES; }
        }

        private const int _TIMEOUT_ERROR_CODE = -2147023436; //Time out occured

        protected int TIMEOUT_ERROR_CODE
        {
            get { return _TIMEOUT_ERROR_CODE; }
        }

        private const int _WAIT_FAILED = -20;

        protected int WAIT_FAILED
        {
            get { return _WAIT_FAILED; }
        }

        private const int _WAIT_ABANDONED = -30;

        protected int WAIT_ABANDONED
        {
            get { return _WAIT_ABANDONED; }
        }

        private const int _S_OK = 0;

        protected int S_OK
        {
            get { return _S_OK; }
        }

        private string _sCallingModule = "Platform Request";

        protected string sCallingModule
        {
            get { return _sCallingModule; }
            set { _sCallingModule = value; }
        }

        private static object _objShared = new object();

        protected static object objShared
        {
            get { return WhatIFRequestImplementaionService._objShared; }
            set { WhatIFRequestImplementaionService._objShared = value; }
        }
        //public  string  callingClient;
        #endregion
        public static bool CreateTemplate(bool IsSDK, string xmlRequest, int createdBy)
        {
            return true;
        }
        public void EncodeUserInput(ref string xmlRequest)
        {
            XmlDocument docRequestXML = new XmlDocument();
            docRequestXML.LoadXml(xmlRequest);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(docRequestXML.NameTable);
            nsmgr.AddNamespace("apreq", "http://www.approva.net/ApprovalRequest.xsd");

            XmlNode ndName = docRequestXML.SelectSingleNode("apreq:approvalrequests/apreq:approvalrequest/apreq:name", nsmgr);
            string strName = HttpUtility.HtmlEncode(ndName.InnerText);

            ndName.InnerText = strName;
            xmlRequest = docRequestXML.OuterXml;
        }
        protected void GetRequestInfoFromRequestXml(XmlNode xmlRequestNode, out BaseRequest reqInfo)
        {
            XmlNodeList nodeList = xmlRequestNode.ChildNodes;
            reqInfo = new BaseRequest();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlRequestNode.OwnerDocument.NameTable);
            nsmgr.AddNamespace("apns", xmlRequestNode.NamespaceURI);
            foreach (XmlNode node in nodeList)
            {
                switch (node.Name.ToLower())
                {
                    case "id":
                        reqInfo.id = System.Convert.ToInt32(node.InnerText);
                        break;
                    case "name":
                        reqInfo.name = HttpUtility.HtmlEncode((string)node.InnerText);
                        break;
                    case "description":
                        reqInfo.description = HttpUtility.HtmlEncode((string)node.InnerText);
                        break;
                    case "type":
                        reqInfo.sRequestType = node.InnerText;
                        reqInfo.nRequestType = Convert.ToInt32(XmlDocumentHelper.GetNodeValue(xmlDocReq, null, string.Format("/approvalrequests/request[@type=\"{0}\"]/typevalue", node.InnerText)));
                        break;
                    case "status":
                        reqInfo.requestStatus = RequestStatus.Draft;
                        if ((string)node.InnerText != String.Empty)
                        {
                            RequestStatus reqStatus = (RequestStatus)(Enum.Parse(reqInfo.requestStatus.GetType(), (string)node.InnerText));
                            reqInfo.requestStatus = reqStatus;
                        }
                        break;
                    case "priority":
                        reqInfo.requestPriority = RequestPriority.Normal;
                        if ((string)node.InnerText != String.Empty)
                        {
                            RequestPriority reqPriority = (RequestPriority)(Enum.Parse(reqInfo.requestPriority.GetType(), (string)node.InnerText));
                            reqInfo.requestPriority = reqPriority;
                        }
                        break;
                    case "applicationid":
                        reqInfo.ApplicationID = System.Convert.ToInt32(node.InnerText);
                        break;
                    case "applicationname":
                        reqInfo.applicationname = node.InnerText;
                        break;
                    case "format":
                        reqInfo.requestFormat = RequestFormat.NoConstraints;
                        if ((string)node.InnerText != String.Empty)
                        {
                            RequestFormat reqFormat = (RequestFormat)(Enum.Parse(reqInfo.requestFormat.GetType(), (string)node.InnerText));
                            reqInfo.requestFormat = reqFormat;
                        }
                        break;
                    case "applicationscope":
                        reqInfo.applicationScope = (string)node.OuterXml;
                        if (reqInfo.applicationname == null || reqInfo.applicationname == String.Empty)
                        {
                            reqInfo.applicationname = node.SelectSingleNode("apns:scope[@name='firstlevelscope']/apns:scopevalue", nsmgr).InnerText;
                        }
                        break;
                    case "requestdetails":
                        reqInfo.RequestDetails = (string)node.OuterXml;
                        break;
                    case "approvaltemplatedetails":
                        reqInfo.approvalTemplateDetails = (string)node.OuterXml;
                        break;
                    case "lastapprovedon":
                        reqInfo.lastApprovedOn = System.Convert.ToDateTime((string)node.InnerText);
                        break;
                    case "expiration":
                        reqInfo.expiration = System.Convert.ToDateTime((string)node.InnerText);
                        break;
                    case "requestedon":
                        if (node.InnerText != String.Empty)
                            reqInfo.requestedOn = System.Convert.ToDateTime(node.InnerText);
                        break;
                    case "lastupdatedon":
                        if (node.InnerText != String.Empty)
                            reqInfo.lastUpdatedOn = System.Convert.ToDateTime(node.InnerText);
                        break;
                    case "lastupdatedby":
                        reqInfo.lastUpdatedBy = (string)node.InnerText;
                        break;
                    case "totalstages":
                        reqInfo.totalStages = System.Convert.ToInt32(node.InnerText);
                        break;
                    case "currentstage":
                        reqInfo.currentStage = System.Convert.ToInt32(node.InnerText);
                        break;
                    case "templateautoapprovalflag":
                        reqInfo.templateAutoApprovalFlag = System.Convert.ToInt32(node.InnerText);
                        break;
                    case "templateautocompletionflag":
                        reqInfo.templateAutoCompletionFlag = System.Convert.ToInt32(node.InnerText);
                        break;
                    case "templateid":
                        reqInfo.templateID = System.Convert.ToInt32(node.InnerText);
                        break;
                    case "liveverify":
                        reqInfo.liveverify = System.Convert.ToInt32(node.InnerText);
                        break;
                    case "requestactivationtime":
                        reqInfo.requestActivationTime = System.Convert.ToDateTime((string)node.InnerText);
                        break;

                    //START: Code change to support Generic Request
                    /* Fill the 'GenericRequestDetails' with the content of <genericrequestdetails>
                    */
                    case "genericrequestdetails":
                        reqInfo.GenericRequestDetails = (string)node.OuterXml;

                        break;
                    default: break;
                }
            }
        }
        protected virtual void SelectTemplate(BaseRequest reqInfo, bool bIsInline, int nTemplateID, int createdBy, ref int ProfileID, ref string templateXML, ref int directManagerID, ref int createUserAsID, ref int nReqID)
        {
            return;
        }
    }
}
