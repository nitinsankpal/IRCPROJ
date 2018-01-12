using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace IRCWhatIFRequestAPI.DBOprations
{
    public class TemplateDBOperations
    {
        private bool callFromRequest = false;

        public string approvalTemplateNS = "http://www.approva.net/ApprovalTemplate.xsd";
        public void SelectTemplate(string scopeXML, string requestXml, string OperationType, bool persistObjectOwner, bool bResolveFAVariables, out string templateXML)
        {
            templateXML = "";


        }
        public void SelectTemplateForRequest(string scopeXML, int requesterID, string requestPayload, string OperationType, int templateID, bool persistObjectOwner, bool bResolveFAVariables, out string templateXML)
        {
            templateXML = "";
        }
        private void GetObject(int templateID, out string templateXML, string ParticipatingObjectsXml, string SPForExpandVariable, bool persistObjectOwner, bool bResolveFAVariables, string sAdditionalUserAttributeForFA)
        {
            templateXML = "";
        }
        private void GetInvalidApprovers(ref XmlNode rootNode, ref XmlDocument doc,
            int templateID, XmlNamespaceManager nsmgr)
        {
            bool bFirstNameEmpty = false;
            bool bLastNameEmpty = false;
            bool bListOfApprovers = false;
            const int ADMINISTRATOR = 1;
            const int MANAGER_OF_USER = -1;
            const int TEMPLATE_XML = 0;
            const int FIRST_NAME = 0;
            const int LAST_NAME = 1;
            const int PROFILE_NAME = 2;
            ArrayList alProfiles = new ArrayList();
            try
            {
                string sSQL = string.Empty;

                string sSQLFromAPTables = " SELECT c.FirstName, c.LastName, c.ProfileName " +
                    " FROM ApprovalStageApprovers a " +
                    " INNER JOIN ApprovalTemplateStageMapping b ON a.StageID = b.StageID " +
                    " INNER JOIN Profiles c ON a.ProfileID = c.[ID] " +
                    " WHERE b.TemplateID = " + templateID +
                    " AND a.ProfileID NOT IN (1, -1)  " +
                    " AND c.Availability = 0 " +
                    " AND ProfileName NOT IN (SELECT ProfileName FROM Profiles WHERE Availability = 1) ";

                string profileName = string.Empty;
                string approversFromTemplate = string.Empty;

                string sSQLToSelectAPTCmdName = "Approva.BizRights.Core.TemplateOperations.SQLToSelectAPT";
                object[] selectAPTparams = new object[] { templateID.ToString() };

                string templateToBeVerified = string.Empty;

                //select TemplateXML from ApprovalTemplateXML where id = '{0}'

                string sSQLToSelectAPT = "select TemplateXML from ApprovalTemplateXML where id = '" + templateID + "'";



                //Added the  CommandBuilder class reference which will send data encrypted format : CWE ID- 89 (SQL Injection)
                // CommandBuilder.BuildSqlCommand(sqlcmd, "", sSQLToSelectAPT);

                DataSet objDS = new DataSet();
                //newObjDB.Execute(sqlcmd, out objDS);

                //while (dataReaderForAPT.Read())
                if ((objDS != null) && (objDS.Tables.Count > 0))
                {
                    if (objDS.Tables[0].Rows.Count > 0)
                    {
                        templateToBeVerified = objDS.Tables[0].Rows[0]["TemplateXML"].ToString();
                    }
                }

                if (templateToBeVerified != string.Empty)
                {
                    XmlDocument xDoc = new XmlDocument();

                    xDoc.LoadXml(templateToBeVerified);
                    //Value concatenated in Xpath is hard coded in the code. Hence no need to change the code : CWE ID- 643 (Injection.Xpath)
                    XmlNodeList approverList = xDoc.SelectNodes("descendant::apns:approver[apns:id != '" + ADMINISTRATOR + "' and apns:id != '" + MANAGER_OF_USER + "']", nsmgr);

                    for (int i = 0; i < approverList.Count; i++)
                    {
                        if (approverList.Item(i).SelectSingleNode("apns:userid", nsmgr) != null)
                        {
                            approversFromTemplate += "N'" + approverList.Item(i).SelectSingleNode("apns:userid", nsmgr).InnerText.Replace("'", "''") + "',";
                            bListOfApprovers = true;
                        }
                    }
                }

                string listOfApproversCmdName = string.Empty;
                object[] paramsForListOfApprovers;
                if (bListOfApprovers)
                {
                    approversFromTemplate = approversFromTemplate.Remove(approversFromTemplate.Length - 1, 1);

                    sSQL = " SELECT ISNULL(A.FirstName,''), ISNULL(A.LastName,''), A.DefApprover ";
                    sSQL += " FROM ( 	Select FirstName, LastName, ProfileName as DefApprover FROM Profiles ";
                    sSQL += " WHERE ProfileName in (" + approversFromTemplate + ") ";
                    sSQL += ") A    ";
                    sSQL += " LEFT OUTER JOIN Profiles P ON A.DefApprover = P.ProfileName AND P.Availability = 1 ";
                    sSQL += " WHERE	IsNull(P.Id, 0) = 0 ";
                    sSQL += " UNION ";
                    sSQL += " SELECT c.FirstName, c.LastName, c.ProfileName ";
                    sSQL += " FROM ApprovalStageApprovers a ";
                    sSQL += " INNER JOIN ApprovalTemplateStageMapping b ON a.StageID = b.StageID ";
                    sSQL += " INNER JOIN Profiles c ON a.ProfileID = c.[ID] ";
                    sSQL += " WHERE b.TemplateID = " + templateID;
                    sSQL += " AND a.ProfileID NOT IN (1, -1)  ";
                    sSQL += " AND c.Availability = 0 ";
                    sSQL += " AND ProfileName NOT IN (SELECT ProfileName FROM Profiles WHERE Availability = 1) ";

                    listOfApproversCmdName = "Approva.BizRights.Core.TemplateOperations.ListOfApproversANDSQLFromAPTables";
                    paramsForListOfApprovers = new object[] { approversFromTemplate, templateID.ToString() };
                }

                else
                {
                    sSQL = sSQLFromAPTables;
                    listOfApproversCmdName = "Approva.BizRights.Core.TemplateOperations.SQLFromAPTables";
                    paramsForListOfApprovers = new object[] { templateID.ToString() };
                }

                DataSet objnewDS = new DataSet();
                XmlElement element = doc.CreateElement("InvalidApprovers", approvalTemplateNS);
                rootNode.AppendChild(element);
                try
                {
                    //  newObjDB.Execute(sqlcmd, out objnewDS);

                    if ((objnewDS != null) && (objnewDS.Tables.Count > 0))
                    {
                        //while (dataReader.Read())
                        foreach (DataRow dr in objnewDS.Tables[0].Rows)
                        {
                            profileName = string.Empty;

                            if (!string.IsNullOrEmpty(dr[FIRST_NAME].ToString()))
                            {
                                if (dr[FIRST_NAME].ToString().Trim().Length > 0)
                                {
                                    profileName = dr[FIRST_NAME].ToString();
                                }
                                else if (dr[FIRST_NAME].ToString().Trim().Length == 0)
                                {
                                    bFirstNameEmpty = true;
                                }
                            }
                            if (!string.IsNullOrEmpty(dr[LAST_NAME].ToString()))
                            {
                                if (dr[LAST_NAME].ToString().Trim().Length > 0)
                                {
                                    profileName += " " + dr[LAST_NAME].ToString();
                                }
                                else if (dr[LAST_NAME].ToString().Trim().Length == 0)
                                {
                                    bLastNameEmpty = true;
                                }
                            }

                            if (bFirstNameEmpty && bLastNameEmpty)
                            {
                                if (!!string.IsNullOrEmpty(dr[PROFILE_NAME].ToString()))
                                {
                                    profileName = dr[PROFILE_NAME].ToString();
                                }
                            }

                            if (!alProfiles.Contains(profileName))
                            {
                                XmlElement elementApprover = doc.CreateElement("approver", approvalTemplateNS);
                                elementApprover.InnerText = profileName;
                                XmlNode refNode = rootNode.SelectSingleNode("apns:InvalidApprovers", nsmgr);
                                refNode.AppendChild(elementApprover);
                                alProfiles.Add(profileName);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }

            }

            catch (Exception exp)
            {
            }
        }
    }
}
