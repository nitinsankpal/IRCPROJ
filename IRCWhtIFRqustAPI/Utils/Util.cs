using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using IRCWhatIFRequestAPI.Models;
using System.Xml.XPath;
using System.IO;
using System.Collections;
using System.Threading;

namespace IRCWhatIFRequestAPI.Utils
{
    public class Util
    {
        public const string _whatifNs = "http://www.approva.net/ApprovalRequest.xsd";
        public static string BuildPayLoadXML(WhatIFReuestPayload Payload)
        {

            XmlDocument doc = new XmlDocument();
            int revokedCnt = 0;
            string command = string.Empty;
            try
            {
                string connXML = string.Empty;
                string connName = string.Empty;
                string profileIDConnOwnersXml = string.Empty;
                string profileIDConnUsersXml = string.Empty;

                string analysisType = string.Empty;
                string whatifType = string.Empty;
                string WhatIfOPtype = string.Empty;
                string GUID = string.Empty;

                if (Payload.isUC)
                {
                    analysisType = "WhatIf_ION_UserCreation";
                    whatifType = "ION - User Creation";
                }
                else
                {
                    analysisType = "WhatIf_ION_RoleAssignment";
                    whatifType = "ION - Role Assignment Management";
                }
                // Here we can Get connection param  xml from master connection table
                connXML = "<section name=\"infor_coneectionConnectionParams\" displayname=\"infor_coneection\"> </ section >";

                XmlDocument DocConn = new XmlDocument();
                DocConn.LoadXml(connXML);
                connName = "Ion_Connection";//DocConn.SelectSingleNode("section").Attributes["displayname"].Value;
                XmlElement RootTag = doc.CreateElement("approvalrequests", _whatifNs);
                doc.AppendChild(RootTag);

                doc.AppendChild(RootTag);
                XmlElement AppReq = XMLHelper.CreateAndAttachElement(ref doc, ref RootTag, "approvalrequest", _whatifNs);

                XMLHelper.CreateAndAttachElement(ref doc, ref AppReq, "name", _whatifNs, Payload.GenericFields.name);
                XMLHelper.CreateAndAttachElement(ref doc, ref AppReq, "type", _whatifNs, analysisType);
                XMLHelper.CreateAndAttachElement(ref doc, ref AppReq, "status", _whatifNs, "SubmittedForWhatIf");
                XMLHelper.CreateAndAttachElement(ref doc, ref AppReq, "priority", _whatifNs, "Medium");
                XMLHelper.CreateAndAttachElement(ref doc, ref AppReq, "applicationid", _whatifNs, Payload.GenericFields.connectionID.ToString());
                XMLHelper.CreateAndAttachElement(ref doc, ref AppReq, "applicationname", _whatifNs, connName);
                XMLHelper.CreateAndAttachElement(ref doc, ref AppReq, "format", _whatifNs, "NoConstraints");


                XmlElement GenReqDetails = XMLHelper.CreateAndAttachElement(ref doc, ref AppReq, "genericrequestdetails", _whatifNs);

                XmlElement analysisparams = XMLHelper.CreateAndAttachElement(ref doc, ref GenReqDetails, "analysisparams", _whatifNs);

                XMLHelper.CreateAndAttachElement(ref doc, ref analysisparams, "analysistype", _whatifNs, "WhatIfAnalysis");
                XMLHelper.CreateAndAttachElement(ref doc, ref analysisparams, "syncmode", _whatifNs, "true");
                XMLHelper.CreateAndAttachElement(ref doc, ref analysisparams, "taskid", _whatifNs, "0");

                XMLHelper.CreateAndAttachElement(ref doc, ref analysisparams, "analysisowner", _whatifNs, Payload.GenericFields.ProfileID);
                XMLHelper.CreateAndAttachElement(ref doc, ref analysisparams, "analysisname", _whatifNs, Payload.GenericFields.name);

                XmlElement rulebooks = XMLHelper.CreateAndAttachElement(ref doc, ref analysisparams, "rulebooks", _whatifNs);
                if (Payload.GenericFields.rulebooksInfo != null)
                {
                    for (int rbcnt = 0; rbcnt < Payload.GenericFields.rulebooksInfo.Length; rbcnt++)
                    {
                        string RuleBookId = Payload.GenericFields.rulebooksInfo[rbcnt].id.ToString();
                        string RuleBookName = Payload.GenericFields.rulebooksInfo[rbcnt].name;

                        //Here we can get rulebook name from rulebook ID and Profile ID.
                        //string rulebookName = GetRuleBook(Payload.GenericFields.ProfileID, Convert.ToInt32(RuleBookId))

                        XmlElement rulebook = XMLHelper.CreateAndAttachElement(ref doc, ref rulebooks, "rulebook", _whatifNs);
                        XMLHelper.CreateAndAttachAttribute(ref doc, ref rulebook, "id", RuleBookId);
                        XMLHelper.CreateAndAttachAttribute(ref doc, ref rulebook, "name", RuleBookName);
                        XmlElement clientinfo = XMLHelper.CreateAndAttachElement(ref doc, ref rulebook, "clientinfo", _whatifNs);
                        XmlElement client = XMLHelper.CreateAndAttachElement(ref doc, ref clientinfo, "client", _whatifNs);
                        XMLHelper.CreateAndAttachAttribute(ref doc, ref client, "id", Payload.GenericFields.connectionID.ToString());

                    }
                }
                else
                {
                    XmlElement rulebook = XMLHelper.CreateAndAttachElement(ref doc, ref rulebooks, "rulebook", _whatifNs);
                    XMLHelper.CreateAndAttachAttribute(ref doc, ref rulebook, "id", "-1");
                    XMLHelper.CreateAndAttachAttribute(ref doc, ref rulebook, "name", "");
                    XmlElement clientinfo = XMLHelper.CreateAndAttachElement(ref doc, ref rulebook, "clientinfo", _whatifNs);
                    XmlElement client = XMLHelper.CreateAndAttachElement(ref doc, ref clientinfo, "client", _whatifNs);
                    XMLHelper.CreateAndAttachAttribute(ref doc, ref client, "id", Payload.GenericFields.connectionID.ToString());
                }

                //Additional Scope Filters	
                AddScopeFiltersInfo(ref doc, ref analysisparams, analysisType);
                WhatIfOPtype = "114";
                GUID = "689c48ab-10d0-4c4a-b230-cdfe391a1bbf";
                XmlElement wiparams = XMLHelper.CreateAndAttachElement(ref doc, ref analysisparams, "wiparams", _whatifNs, "");
                XMLHelper.CreateAndAttachElement(ref doc, ref wiparams, "wioperationtype", _whatifNs, WhatIfOPtype);
                XMLHelper.CreateAndAttachElement(ref doc, ref wiparams, "insightguid", _whatifNs, GUID);
                XMLHelper.CreateAndAttachElement(ref doc, ref wiparams, "requestid", _whatifNs, "");


                XmlElement wiopdetails = XMLHelper.CreateAndAttachElement(ref doc, ref wiparams, "wiopdetails", _whatifNs, "");
                XmlElement connection = XMLHelper.CreateAndAttachElement(ref doc, ref wiopdetails, "connection", _whatifNs);
                XMLHelper.CreateAndAttachAttribute(ref doc, ref connection, "conn_name", connName);
                connection.InnerText = Payload.GenericFields.connectionID.ToString();

                XmlElement user = XMLHelper.CreateAndAttachElement(ref doc, ref wiopdetails, "user", _whatifNs);
                XMLHelper.CreateAndAttachAttribute(ref doc, ref user, "user_name", Payload.UserName);
                XMLHelper.CreateAndAttachAttribute(ref doc, ref user, "firstname", Payload.FirstName);
                XMLHelper.CreateAndAttachAttribute(ref doc, ref user, "lastname", Payload.LastName);
                //XMLHelper.CreateAndAttachAttribute(ref doc, ref user, "description", "");
                XMLHelper.CreateAndAttachAttribute(ref doc, ref user, "startdate", DateTime.Now.ToString("dd/MM/yyyy")); //Current Date Set
                XMLHelper.CreateAndAttachAttribute(ref doc, ref user, "enddate", "01/01/1900");
                XMLHelper.CreateAndAttachAttribute(ref doc, ref user, "assigneeID", "-1");
                XMLHelper.CreateAndAttachAttribute(ref doc, ref user, "asanotheruser", "");
                user.InnerText = "-1";

                if (!Payload.isUC)
                {
                    //get existing roles from user

                    XMLHelper.CreateAndAttachAttribute(ref doc, ref user, "user_name", "S1_U1");
                    XMLHelper.CreateAndAttachAttribute(ref doc, ref user, "startdate", DateTime.Now.ToString("dd/MM/yyyy")); //TODO: Hardcoded Start Date
                    XMLHelper.CreateAndAttachAttribute(ref doc, ref user, "enddate", "01/01/1900");//TODO: Hardcoded End Date                    
                    XMLHelper.CreateAndAttachAttribute(ref doc, ref user, "modified", "false");
                    XMLHelper.CreateAndAttachAttribute(ref doc, ref user, "assigneeID", "");
                    XMLHelper.CreateAndAttachAttribute(ref doc, ref user, "asanotheruser", "");
                    XMLHelper.CreateAndAttachAttribute(ref doc, ref user, "firstname", "Joan");
                    XMLHelper.CreateAndAttachAttribute(ref doc, ref user, "lastname", "Smith");

                    //user.InnerText = dsUserDetails.Tables["object"].Rows[0]["id"].ToString();
                    user.InnerText = "7687";

                    XmlElement existingrolelist = XMLHelper.CreateAndAttachElement(ref doc, ref wiopdetails, "existingrolelist", _whatifNs, "");
                    XmlElement revokedrolelist = XMLHelper.CreateAndAttachElement(ref doc, ref wiopdetails, "revokedrolelist", _whatifNs, "");

                    //Get Roles for existing user

                    string existingRoles = string.Empty;
                    XmlDocument docExRoles = new XmlDocument();
                    docExRoles.LoadXml(existingRoles);
                    XmlNodeList lstExRoles = docExRoles.SelectNodes("roles/object");

                    ArrayList arrRevokedRoles = new ArrayList();

                    if (Payload.RevokedRoles != null)
                    {
                        for (int i = 0; i < Payload.RevokedRoles.Length; i++)
                            arrRevokedRoles.Add(Payload.RevokedRoles[i].ToUpper());
                            arrRevokedRoles.Sort();
                    }

                    for (int i = 0; i < lstExRoles.Count; i++)
                    {
                        string roleID = "S1_R1"; //lstExRoles[i].SelectSingleNode("roleid").InnerText;
                        string roleKey = "Scenario1_Role1";//lstExRoles[i].SelectSingleNode("rolename").InnerText;
                        string friendlyName = "Scenario1_Role1 MasterDescription";//(lstExRoles[i].SelectSingleNode("desc") != null) ? lstExRoles[i].SelectSingleNode("desc").InnerText : "";
                        string startDate = "01/02/1900";//(lstExRoles[i].SelectSingleNode("startdate") != null) ? lstExRoles[i].SelectSingleNode("startdate").InnerText : "";
                        string endDate = "01/02/1900";// (lstExRoles[i].SelectSingleNode("enddate") != null) ? lstExRoles[i].SelectSingleNode("enddate").InnerText : "";
                        string appID = "3";//lstExRoles[i].SelectSingleNode("appid").InnerText;
                        string appName = "infor_coneection"; // lstExRoles[i].SelectSingleNode("appname").InnerText;
                        string rID ="1"; // lstExRoles[i].SelectSingleNode("id").InnerText;

                        if (arrRevokedRoles.BinarySearch(roleID.ToUpper()) >= 0)
                        {
                            revokedCnt++;
                            XmlElement revokedrole = XMLHelper.CreateAndAttachElement(ref doc, ref revokedrolelist, "revokedrole", _whatifNs);
                            XMLHelper.CreateAndAttachAttribute(ref doc, ref revokedrole, "id", roleID);
                            XMLHelper.CreateAndAttachAttribute(ref doc, ref revokedrole, "appid", appID);
                            XMLHelper.CreateAndAttachAttribute(ref doc, ref revokedrole, "role_key", roleKey);
                            XMLHelper.CreateAndAttachAttribute(ref doc, ref revokedrole, "friendlyname", friendlyName);
                            XMLHelper.CreateAndAttachAttribute(ref doc, ref revokedrole, "appname", appName);
                            XMLHelper.CreateAndAttachAttribute(ref doc, ref revokedrole, "startdate", startDate);
                            XMLHelper.CreateAndAttachAttribute(ref doc, ref revokedrole, "enddate", endDate);
                            revokedrole.InnerText = rID;
                        }

                        XmlElement existingrole = XMLHelper.CreateAndAttachElement(ref doc, ref existingrolelist, "existingrole", _whatifNs);
                        XMLHelper.CreateAndAttachAttribute(ref doc, ref existingrole, "id", roleID);
                        XMLHelper.CreateAndAttachAttribute(ref doc, ref existingrole, "appid", appID);
                        XMLHelper.CreateAndAttachAttribute(ref doc, ref existingrole, "role_key", roleKey);
                        XMLHelper.CreateAndAttachAttribute(ref doc, ref existingrole, "friendlyname", friendlyName);
                        XMLHelper.CreateAndAttachAttribute(ref doc, ref existingrole, "appname", appName);
                        XMLHelper.CreateAndAttachAttribute(ref doc, ref existingrole, "startdate", startDate);
                        XMLHelper.CreateAndAttachAttribute(ref doc, ref existingrole, "enddate", endDate);
                        existingrole.InnerText = rID;

                    }

                }

                XmlElement newrolelist = XMLHelper.CreateAndAttachElement(ref doc, ref wiopdetails, "newrolelist", _whatifNs, "");
                XmlElement newrole;

                if (Payload.NewRoles != null && Payload.NewRoles.Length > 0)
                {
                    for (int j = 0; j < Payload.NewRoles.Length; j++)
                    {
                        //Here we can get new roles for uc creation
                        //XmlNode ndRole = GetNewRoleAttributes(Payload.GenericFields.connectionID, Payload.UserName, Payload.NewRoles[j], Payload.isUC);
                        newrole = XMLHelper.CreateAndAttachElement(ref doc, ref newrolelist, "newrole", _whatifNs);

                        string rId = "2"; //ndRole.SelectSingleNode("id").InnerText;
                        string roleID = "S1_R2"; //lstExRoles[i].SelectSingleNode("roleid").InnerText;
                        string roleKey = "Scenario1_Role2";//lstExRoles[i].SelectSingleNode("rolename").InnerText;
                        string friendlyName = "Scenario2_Role2 MasterDescription";//(lstExRoles[i].SelectSingleNode("desc") != null) ? lstExRoles[i].SelectSingleNode("desc").InnerText : "";
                        string startDate = "01/02/1900";//(lstExRoles[i].SelectSingleNode("startdate") != null) ? lstExRoles[i].SelectSingleNode("startdate").InnerText : "";
                        string endDate = "01/02/1900";// (lstExRoles[i].SelectSingleNode("enddate") != null) ? lstExRoles[i].SelectSingleNode("enddate").InnerText : "";
                        string appID = "3";//lstExRoles[i].SelectSingleNode("appid").InnerText;
                        string appName = "infor_coneection"; // lstExRoles[i].SelectSingleNode("appname").InnerText;

                        XMLHelper.CreateAndAttachAttribute(ref doc, ref newrole, "id", roleID);
                        XMLHelper.CreateAndAttachAttribute(ref doc, ref newrole, "appid", appID);
                        XMLHelper.CreateAndAttachAttribute(ref doc, ref newrole, "role_key", roleKey);
                        XMLHelper.CreateAndAttachAttribute(ref doc, ref newrole, "friendlyname", friendlyName);
                        XMLHelper.CreateAndAttachAttribute(ref doc, ref newrole, "appname", appName);
                        XMLHelper.CreateAndAttachAttribute(ref doc, ref newrole, "startdate", startDate);
                        XMLHelper.CreateAndAttachAttribute(ref doc, ref newrole, "enddate", endDate);

                        newrole.InnerText = rId;
                    }
                }
                //requestor
                XmlElement GRrequestor = XMLHelper.CreateAndAttachElement(ref doc, ref analysisparams, "requestor", _whatifNs, "");
                XMLHelper.CreateAndAttachElement(ref doc, ref GRrequestor, "id", _whatifNs, "1");
                XMLHelper.CreateAndAttachElement(ref doc, ref GRrequestor, "userid", _whatifNs, Payload.GenericFields.ProfileName);
                XMLHelper.CreateAndAttachElement(ref doc, ref GRrequestor, "lastname", _whatifNs, "");
                XMLHelper.CreateAndAttachElement(ref doc, ref GRrequestor, "firstname", _whatifNs, "");
                XMLHelper.CreateAndAttachElement(ref doc, ref GRrequestor, "displayname", _whatifNs, "a Administrator");

               // ThreadPool.QueueUserWorkItem(new WaitCallback(CreateRequestInBackGround), objReq);

            }

            catch (Exception ex)
            {

            }
            doc.InnerXml = doc.InnerXml.Replace("xmlns=\"\"", string.Empty);// Static empty string is inserted in XML 
            return doc.InnerXml;
        }
        private static void AddScopeFiltersInfo(ref XmlDocument doc, ref XmlElement AnalysisParams, string WhatifType)
        {
            //Change: Here We can get xml file from Database Without getting from local path;
            string Filename = Path.Combine("C:\\Program Files\\Approva\\BizRights\\BizRightsPresentation\\xml\\WhatIfAnalysisScopeFilters.xml");
            if (File.Exists(Filename))
            {
                XmlDocument ScopeFilterDoc = new XmlDocument();
                ScopeFilterDoc.Load(Filename);
                XPathNavigator xNav = ScopeFilterDoc.CreateNavigator();
                string selectNodeQuery = "whatifanalysisscopefilters/type[@name = '" + WhatifType + "']/ScopingValues";
                XPathExpression exprXPath = xNav.Compile(selectNodeQuery);
                XPathNavigator scopingValuesNode = xNav.SelectSingleNode(exprXPath);
                if (scopingValuesNode != null)
                {
                    XmlElement eleScopeValues = XMLHelper.CreateAndAttachElement(ref doc, ref AnalysisParams, "ScopingValues", _whatifNs);
                    eleScopeValues.InnerXml = scopingValuesNode.InnerXml; //AppScan:: setted value is from template xml
                }

            }
        }
    }
    internal class XMLHelper
    {
        public static XmlElement CreateAndAttachElement(ref XmlDocument doc, ref XmlElement parent, string localName, string nsUri)
        {
            XmlElement tempElement = doc.CreateElement(localName, nsUri);
            parent.AppendChild(tempElement);
            return tempElement;
        }

        public static XmlElement CreateAndAttachElement(ref XmlDocument doc, ref XmlElement parent, string localName, string nsUri, string elementValue)
        {
            XmlElement tempElement = doc.CreateElement(localName, nsUri);
            tempElement.InnerText = elementValue;
            parent.AppendChild(tempElement);
            return tempElement;
        }

        public static XmlElement CreateAndAttachElement(ref XmlDocument doc, ref XmlNode parent, string localName, string nsUri)
        {
            XmlElement tempElement = doc.CreateElement(localName, nsUri);
            parent.AppendChild(tempElement);
            return tempElement;
        }

        public static XmlElement CreateAndAttachElement(ref XmlDocument doc, ref XmlNode parent, string localName, string nsUri, string elementValue)
        {
            XmlElement tempElement = doc.CreateElement(localName, nsUri);
            tempElement.InnerText = elementValue;
            parent.AppendChild(tempElement);
            return tempElement;
        }

        public static void CreateAndAttachAttribute(ref XmlDocument doc, ref XmlElement parent, string attributelName, string attributeValue)
        {
            XmlAttribute attribute = doc.CreateAttribute(attributelName);
            attribute.Value = attributeValue;
            parent.Attributes.Append(attribute);
        }

        public static void CreateAndAttachAttribute(ref XmlDocument doc, ref XmlNode parent, string attributelName, string attributeValue)
        {
            XmlAttribute attribute = doc.CreateAttribute(attributelName);
            attribute.Value = attributeValue;
            parent.Attributes.Append(attribute);
        }

        public static void RemoveNodes(ref XmlNodeList list)
        {
            try
            {
                foreach (XmlNode node in list)
                {
                    XmlNode parent = node.ParentNode;
                    parent.RemoveChild(node);
                }
            }
            catch (Exception ex)
            {
                // IONAIRequestImplementation.greReqObj.WriteLog(System.DateTime.Now, "RemoveNodes(..)", "Error removing element from XmlDocument.");
                throw new Exception("Error removing element from XmlDocument.", ex);
            }
        }


    }
    public enum RequestStatus
    {
        Draft = 0,
        PendingApproval = 1,
        PendingAssignment = 2,
        PendingApplicationUpdate = 3,
        Approved = 4,
        Rejected = 5,
        PartiallyApproved = 6,
        Failed = 7,
        CheckedOut = 8,
        CheckedIn = 9,
        AutoCancelled = 10,
        Cancelled = 11,
        SubmittedForWhatIf = 12,
        WhatIfFailed = 13,
        WhatIfCompletedWithViolations = 14,
        WhatIfCompletedWithNoViolations = 15,
        WhatIfMitigated = 16,
        Reviewed = 17,
        ForcedCompleted = 18,
        WhatIfPartiallyCompleted = 19
    }
    public enum RequestPriority
    {
        High = 0,
        Medium = 1,
        Low = 2,
        Normal = 3,
        Informational = 4,
        None = 5
    }
    public enum RequestFormat
    {
        ByFAandCC = 0,
        AsAnotherUser = 1,
        NoConstraints = 2,
        ByScopes = 3
    }
    public enum RequestType
    {
        None = 0,
        ChangeAssignment = 1,
        RoleAssignment = 2,
        RoleRevocation = 3,
        OnlyWhatIfAnalysis = 4,
        WhatIfAnalysis = 5,
        RequestRoleChange = 6,
        RoleAssignmentToPosition = 7,
        WhatIfRoleChange = 8,
        AccessOnDemand = 9
    }
}
