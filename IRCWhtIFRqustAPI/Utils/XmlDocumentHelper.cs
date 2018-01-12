using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace IRCWhatIFRequestAPI.Utils
{
    public class XmlDocumentHelper
    {
        private XmlDocumentHelper()
        {
            //Creation of its object is not allowed directly
        }


        public static XmlDocumentHelper GetObject()
        {
            XmlDocumentHelper sDocHelper = new XmlDocumentHelper();
            return sDocHelper;
        }
        public static XmlElement CreateAndAttachElement(ref XmlDocument doc, ref XmlElement parent, string localName, string nsUri)
        {
            return CreateAndAttachElement(ref doc, ref parent, localName, nsUri, String.Empty);
        }
        public static XmlElement CreateAndAttachElement(ref XmlDocument doc, ref XmlElement parent, string localName, string nsUri, string elementValue)
        {
            XmlElement tempElement = doc.CreateElement(localName, nsUri);
            tempElement.InnerText = elementValue;
            if (parent == null)
            {
                doc.AppendChild(tempElement);
            }
            else
            {
                parent.AppendChild(tempElement);
            }
            return tempElement;
        }
        public static XmlElement CreateAndAttachElement(ref XmlDocument doc, ref XmlNode parent, string localName, string nsUri)
        {
            return CreateAndAttachElement(ref doc, ref parent, localName, nsUri, String.Empty);
        }
        public static XmlElement CreateAndAttachElement(ref XmlDocument doc, ref XmlNode parent, string localName, string nsUri, string elementValue)
        {
            XmlElement tempElement = doc.CreateElement(localName, nsUri);
            tempElement.InnerText = elementValue;
            if (parent == null)
            {
                doc.AppendChild(tempElement);
            }
            else
            {
                parent.AppendChild(tempElement);
            }
            return tempElement;
        }

        public static string GetNodeValue(XmlDocument doc, XmlNamespaceManager nsMgr, string xPath)
        {
            XmlNode node = doc.SelectSingleNode(xPath, nsMgr);
            if (node != null)
                return node.InnerText;
            else
                return String.Empty;
        }


        public static string GetNodeAttributeValue(XmlDocument doc, XmlNamespaceManager nsMgr, string xPath, string sAttribute)
        {
            XmlNode node = doc.SelectSingleNode(xPath, nsMgr);
            if (node != null)
            {
                XmlAttribute xmlAttrib = node.Attributes[sAttribute];
                if (xmlAttrib != null)
                {
                    return xmlAttrib.InnerText;
                }
                else
                {
                    return String.Empty;
                }
            }
            else
            {
                return String.Empty;
            }
        }


        //public static XmlDocument LoadConfig(string sFileName)
        //{
        //    XmlDocument xDoc = new XmlDocument();
        //    xDoc.Load(GetFullPath(@"\Settings\" + sFileName));
        //    return xDoc;
        //}


        ///// <summary>
        ///// Reads registry to prepare fully qualified path for the file.
        ///// </summary>
        ///// <param name="strFile">The partial path for which full path needs to be retrieved.</param>
        ///// <returns>Fully qualified path for the file</returns>
        //public static string GetFullPath(string strFile)
        //{
        //    RegistryKey registryKey = null;
        //    string returnData = strFile;
        //    try
        //    {
        //        registryKey = Registry.LocalMachine.OpenSubKey("SoftWare\\Approva\\BizRights");
        //        object RegValue = registryKey.GetValue("InstallPath");

        //        returnData = RegValue.ToString() + strFile;
        //    }
        //    catch (Exception)
        //    {
        //        //TODO: Log the Exception
        //    }
        //    finally
        //    {
        //        try
        //        {
        //            if (registryKey != null)
        //            {
        //                registryKey.Close();
        //            }
        //        }
        //        catch { }
        //    }
        //    return returnData;
        //}

        public static XmlNodeList GetNodes(XmlDocument doc, XmlNamespaceManager nsMgr, string xPath)
        {
            XmlNodeList nodelist = doc.SelectNodes(xPath, nsMgr);
            return nodelist;
        }

    }
}
