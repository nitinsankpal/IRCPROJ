using IRCWhatIFRequestAPI.DBOprations;
using IRCWhatIFRequestAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace IRCWhatIFRequestAPI.Services
{
    public class AMIRequestService: WhatIFRequestImplementaionService
    {
        protected override void SelectTemplate(BaseRequest reqInfo, bool bIsInline, int nTemplateID, int createdBy, ref int ProfileID, ref string templateXML, ref int directManagerID, ref int createUserAsID, ref int nReqID)
        {
            #region Select approval template Details
            TemplateDBOperations templateOps = new TemplateDBOperations();
            bool PersistObjectOwner = true;
            string strOperation = "Role Assignment/Revocation";
            string reqPayload = string.Empty;
            try
            {
                strOperation = XmlDocumentHelper.GetNodeValue(xmlDocReq, null, string.Format("/approvalrequests/request[@type=\"{0}\"]/templateselection[@required=\"yes\"]/operation", reqInfo.sRequestType));
            }
            catch (Exception)
            {
            }
            if (reqInfo.GenericRequestDetails == null || reqInfo.GenericRequestDetails == string.Empty)
            {
                reqPayload = reqInfo.RequestDetails;
            }
            else
            {
                reqPayload = reqInfo.GenericRequestDetails;
            }

            if (bIsInline)
            {
                templateOps.SelectTemplate(reqInfo.applicationScope, reqPayload, strOperation, false, false, out templateXML);
            }
            else
            {
                templateOps.SelectTemplateForRequest(reqInfo.applicationScope, createdBy, reqPayload, strOperation, nTemplateID, PersistObjectOwner, true, out templateXML);
            }

            try
            {
                #region Get Manager and User ID
                GetManagerAndUserId(reqInfo, ref ProfileID, ref directManagerID, ref createUserAsID);
                #endregion Get Manager and User ID
                #region Validate Request Completion               
                  ValidateGenericRequestCompletion(reqInfo.ApplicationID, templateXML, reqInfo.GenericRequestDetails, reqInfo.nRequestType);             
                #endregion

            }
            catch (Exception Bex)
            {
                throw Bex;
            }

            #endregion
        }
        private void GetManagerAndUserId(BaseRequest reqInfo, ref int ProfileID, ref int directManagerID, ref int createUserAsID)
        {
            #region Get Manager and User ID
            //Retrieve User and Manager IDs
            directManagerID = -1;
            ProfileID = 0;
            string requestNS = "http://www.approva.net/ApprovalRequest.xsd";
            string sRequestDetailsXml = String.Empty;
            if (reqInfo.GenericRequestDetails == null || reqInfo.GenericRequestDetails == string.Empty)
            {
                sRequestDetailsXml = reqInfo.RequestDetails;
            }
            else
            {
                sRequestDetailsXml = reqInfo.GenericRequestDetails;
            }
            if (sRequestDetailsXml != null && sRequestDetailsXml != String.Empty)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(sRequestDetailsXml);

                XmlNamespaceManager nsmgr1 = new XmlNamespaceManager(xmlDoc.NameTable);
                nsmgr1.AddNamespace("apns", requestNS);

                string sRequestRootTagName = String.Empty;
                bool bIsGeneric = IsGeneric(reqInfo.sRequestType, "apns", out sRequestRootTagName);

                try
                {
                    string sAssigneeProfileIDXPath = "requestdetails/roleassignment/users/user/id";
                    string sTmp = XmlDocumentHelper.GetNodeValue(base.xmlDocReq, null, string.Format("/approvalrequests/request[@type=\"{0}\"]/templateselection[@required=\"yes\"]/assigneeProfileIDXPath", reqInfo.sRequestType));
                    if (sTmp != String.Empty)
                    {
                        sAssigneeProfileIDXPath = sTmp;
                    }
                    sAssigneeProfileIDXPath = "apns:" + sAssigneeProfileIDXPath.Replace("/", "/apns:");
                    sAssigneeProfileIDXPath = sAssigneeProfileIDXPath.Replace("/apns:@", "/@");
                    //XML come from directly Database Table  which is validated with Xslt template before inserting into the  Database table . So no need to change the code.-CWE-643- Not a Issue-Appscan
                    ProfileID = Convert.ToInt32(xmlDoc.SelectSingleNode(sAssigneeProfileIDXPath, nsmgr1).InnerText);
                }
                catch { }
                //get the node from all possible cases and update database where profile id = -1
                //XML come from directly Database Table  which is validated with Xslt template before inserting into the  Database table . So no need to change the code.-CWE-643- Not a Issue-Appscan
                XmlNode directManagerNode = xmlDoc.SelectSingleNode(string.Format("{0}/apns:userstoadd/apns:user/apns:directmanager", sRequestRootTagName), nsmgr1);
                if (directManagerNode == null)
                    directManagerNode = xmlDoc.SelectSingleNode(string.Format("{0}/apns:userstomodify/apns:user/apns:directmanager", sRequestRootTagName), nsmgr1);
                if (directManagerNode == null)
                    directManagerNode = xmlDoc.SelectSingleNode(string.Format("{0}/apns:createusersas/apns:user/apns:directmanager", sRequestRootTagName), nsmgr1);
                if (directManagerNode == null)
                    directManagerNode = xmlDoc.SelectSingleNode(string.Format("{0}/apns:roleassignment/apns:users/apns:user/apns:directmanager", sRequestRootTagName), nsmgr1);

                if (directManagerNode != null)
                {
                    directManagerID = System.Convert.ToInt32(directManagerNode.InnerText);
                    
                }
                //XML come from directly Database Table  which is validated with Xslt template before inserting into the  Database table . So no need to change the code.-CWE-643- Not a Issue-Appscan
                XmlNode createUserAsNode = xmlDoc.SelectSingleNode(string.Format("{0}/apns:createusersas/apns:user/apns:id", sRequestRootTagName), nsmgr1);
                //select Create User As userid for create request as option
                if (createUserAsNode != null)
                {
                    try
                    {
                        GetAppPrincipalIDForBRID(reqInfo.ApplicationID, System.Convert.ToInt32(createUserAsNode.InnerText), out createUserAsID);
                    }
                    catch
                    {
                        createUserAsNode.InnerText = "-1";
                        //CWE-643- Not a Issue-Appscan
                        XmlNode roleassignmentNode = xmlDoc.SelectSingleNode(string.Format("{0}/apns:roleassignment", sRequestRootTagName), nsmgr1);
                        if (roleassignmentNode != null)
                        {
                            XmlNode userstoaddNode = xmlDoc.CreateNode(XmlNodeType.Element, "userstoadd", requestNS);
                            userstoaddNode.InnerXml = createUserAsNode.ParentNode.ParentNode.InnerXml;
                            roleassignmentNode.ParentNode.InsertAfter(userstoaddNode, roleassignmentNode);
                            roleassignmentNode.ParentNode.RemoveChild(createUserAsNode.ParentNode.ParentNode);

                            if (reqInfo.GenericRequestDetails == null || reqInfo.GenericRequestDetails == string.Empty)
                            {
                                reqInfo.RequestDetails = xmlDoc.OuterXml;
                            }
                            else
                            {
                                reqInfo.GenericRequestDetails = xmlDoc.OuterXml;
                            }
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            #endregion Get Manager and User ID
        }

        protected bool IsGeneric(string sRequestType, string sNameSpacePrefix, out string sRequestRootTagName)
        {
            return IsGeneric(sRequestType, -1, sNameSpacePrefix, out sRequestRootTagName);
        }
        protected bool IsGeneric(int nRequestType, string sNameSpacePrefix, out string sRequestRootTagName)
        {
            return IsGeneric(String.Empty, nRequestType, sNameSpacePrefix, out sRequestRootTagName);
        }
        private bool IsGeneric(string sRequestType, int nRequestType, string sNameSpacePrefix, out string sRequestRootTagName)
        {
            sRequestRootTagName = "requestdetails";
            bool bIsGeneric = false;
            try
            {
                string sIsGeneric = String.Empty;
                if (nRequestType == -1)
                {
                    sIsGeneric = XmlDocumentHelper.GetNodeValue(xmlDocReq, null, string.Format("/approvalrequests/request[@type=\"{0}\"]/@isgeneric", sRequestType));
                }
                else
                {
                    sIsGeneric = XmlDocumentHelper.GetNodeValue(xmlDocReq, null, string.Format("/approvalrequests/request[typevalue=\"{0}\"]/@isgeneric", nRequestType));
                }
                bIsGeneric = (sIsGeneric.ToLower() == "yes" || sIsGeneric.ToLower() == "true");
                sRequestRootTagName = bIsGeneric ? "genericrequestdetails/analysisparams" : "requestdetails";
            }
            catch (Exception ex)
            {
               
            }
            sRequestRootTagName = sNameSpacePrefix + ":" + sRequestRootTagName.Replace("/", string.Format("/{0}:", sNameSpacePrefix));
            sRequestRootTagName = sRequestRootTagName.Replace(string.Format("{0}:@", sNameSpacePrefix), "@");

            return bIsGeneric;
        }

        private void GetAppPrincipalIDForBRID(int ApplicationID, int CreateUserAsBRID, out int CreateUserAsAppPrincipalID)
        {
            //Database objDB = null;
            //Here we can get AppPrincipalIDfrom database
            CreateUserAsAppPrincipalID = 12;

        }
        private void ValidateGenericRequestCompletion(int ApplicationID, string templateXML, string strGenericRequestDetails, int iReqType)
        {
            
        }
    }
}
