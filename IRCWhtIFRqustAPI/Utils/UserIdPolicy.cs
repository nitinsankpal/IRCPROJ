using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace IRCWhatIFRequestAPI.Utils
{
    public class UserIdPolicy
    {
        public UserIdPolicy()
        {
        }
        private string sLogFileName = "BizRightsCoreDebug.log";
        private string sCallingModule = "Platform Request";
        
        public RETVAL ValidateUserID(string strUserId, string UserIDPolicyXML)
        {
            RETVAL objRetrunValue = RETVAL.Nothing;
            try
            {
                bool IsRepeat;
                bool IsFirstCharAlphabet = false;
                bool IsFirstCharNum = false;
                bool IsFirstCharSplChar = false;
                bool IsStartWith = false;
                int nAlphaCount, nNumCount, nSplCharCount, nCapitalCharacterCount, nLowerCharacterCount;
                int AlphaCount, NumCount, SplCharCount, CapitalCharacterCount, LowerCharacterCount, actualFirstCharCount;
                int nUserIdMinCount, nUserIdMaxCount, nFirstCharacterCount;
                string strXmlOutput;
                string strInputUserID, nUserIdType;
                strInputUserID = strUserId;

                #region Parse XML

                // if you already have an XmlDocument then use that, otherwise
                // create one
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(UserIDPolicyXML);
                XmlNode root = xmlDocument.SelectSingleNode("useridpolicy");
                /*
                  <useridpolicy IsApplicable="true" >
                    <RepeatCharactersAllowed>false</RepeatCharactersAllowed>D
                    <UserIdLength min="4" max="15"></UserIdLength>D
                    <AlphabetCharacterLength min="0" />D
                    <NumericCharacterLength min="0" />D
                    <SpecialCharacterLength min="0" />D
                    <Start>
                        <type minLength="0" operator="with">AlphabetCharacter</type>D
                    </Start>
                    <UserIdType>uppercase</UserIdType>
                  </useridpolicy>
                */

                bool IsApplicable = Convert.ToBoolean(root.Attributes["IsApplicable"].InnerText);
                if (IsApplicable)
                {

                    strXmlOutput = root.SelectSingleNode(@"RepeatCharactersAllowed").InnerText;
                    IsRepeat = (strXmlOutput == "true") ? true : false;

                    strXmlOutput = root.SelectSingleNode(@"AlphabetCharacterLength").Attributes["min"].InnerText;
                    nAlphaCount = Convert.ToInt32(strXmlOutput);

                    strXmlOutput = root.SelectSingleNode(@"NumericCharacterLength").Attributes["min"].InnerText;
                    nNumCount = Convert.ToInt32(strXmlOutput);

                    strXmlOutput = root.SelectSingleNode(@"SpecialCharacterLength").Attributes["min"].InnerText;
                    nSplCharCount = Convert.ToInt32(strXmlOutput);

                    strXmlOutput = root.SelectSingleNode(@"UserIdLength").Attributes["min"].InnerText;
                    nUserIdMinCount = Convert.ToInt32(strXmlOutput);

                    strXmlOutput = root.SelectSingleNode(@"UserIdLength").Attributes["max"].InnerText;
                    nUserIdMaxCount = Convert.ToInt32(strXmlOutput);

                    strXmlOutput = root.SelectSingleNode(@"UserIdType").InnerText;
                    nUserIdType = Convert.ToString(strXmlOutput);

                    //strXmlOutput = root.SelectSingleNode(@"AlphabetLowerCharacterLength").Attributes["min"].InnerText;
                    //nLowerCharacterCount = Convert.ToInt32(strXmlOutput);


                    actualFirstCharCount = 0;
                    foreach (XmlElement ele in root.SelectNodes(@"Start/type"))
                    {
                        if (ele.Attributes["operator"].Value.ToLower() == "without" || ele.Attributes["operator"].Value.ToLower() == "with")
                        {
                            if (ele.Attributes["operator"].Value.ToLower() == "with")
                            {
                                IsStartWith = true;
                                try
                                {
                                    actualFirstCharCount = Convert.ToInt32(ele.Attributes["minLength"].Value);
                                }
                                catch
                                {
                                    actualFirstCharCount = 0;
                                }
                            }
                            switch (ele.InnerText)
                            {
                                case "SpecialCharacter": IsFirstCharSplChar = true; break;
                                case "NumericCharacter": IsFirstCharNum = true; break;
                                case "AlphabetCharacter": IsFirstCharAlphabet = true; break;
                                default: break;
                            }
                        }
                    }
                    #endregion

                    #region Check for UserId Length Min and Max
                    #endregion
                    if (strInputUserID.Length < nUserIdMinCount || strInputUserID.Length > nUserIdMaxCount)
                    {
                        objRetrunValue = RETVAL.InvalidUserIdLength;
                    }
                    else
                    {
                        #region First Character Check
                        if (IsStartWith && IsFirstCharAlphabet)
                        {
                            if (!(IsFirstCharAlphabet && Char.IsLetter(strInputUserID[0])) && actualFirstCharCount > 0)
                                objRetrunValue = RETVAL.InvalidFirstCharacter;
                        }
                        else
                        {
                            if (IsFirstCharAlphabet && Char.IsLetter(strInputUserID[0]))
                                objRetrunValue = RETVAL.InvalidFirstCharacter;
                        }

                        if (IsStartWith && IsFirstCharNum)
                        {
                            if (!(IsFirstCharNum && Char.IsDigit(strInputUserID[0])) && actualFirstCharCount > 0)
                                objRetrunValue = RETVAL.InvalidFirstCharacter;
                        }
                        else
                        {
                            if ((IsFirstCharNum && Char.IsDigit(strInputUserID[0])))
                                objRetrunValue = RETVAL.InvalidFirstCharacter;
                        }

                        if (IsFirstCharSplChar)
                        {
                            if (IsStartWith)
                            {
                                if (!(!Char.IsLetter(strInputUserID[0]) && !Char.IsDigit(strInputUserID[0])) && actualFirstCharCount > 0)
                                    objRetrunValue = RETVAL.InvalidFirstCharacter;
                            }
                            else
                            {
                                if ((!Char.IsLetter(strInputUserID[0]) && !Char.IsDigit(strInputUserID[0])))
                                    objRetrunValue = RETVAL.InvalidFirstCharacter;
                            }
                        }
                        #endregion

                        #region Repeat Character Allowed Check
                        if (IsRepeat == false)
                        {
                            if (CheckRepeatations(strInputUserID))
                                objRetrunValue = RETVAL.RepeatNotAllowed;
                        }
                        #endregion

                        #region Charcter(Typewise) Cout Checking

                        CountCharacters(strInputUserID, actualFirstCharCount, IsFirstCharSplChar, IsFirstCharNum, IsFirstCharAlphabet, out AlphaCount, out NumCount, out SplCharCount, out CapitalCharacterCount, out LowerCharacterCount, out nFirstCharacterCount);
                        if (actualFirstCharCount != 0)
                        {
                            if (actualFirstCharCount > nFirstCharacterCount)
                            {
                                if (IsFirstCharSplChar)
                                {
                                    objRetrunValue = RETVAL.InvalidStartSplCharCharacterLength;
                                }
                                else if (IsFirstCharNum)
                                {
                                    objRetrunValue = RETVAL.InvalidStartNumCharacterLength;
                                }
                                else if (IsFirstCharAlphabet)
                                {
                                    objRetrunValue = RETVAL.InvalidStartAlphaCharacterLength;
                                }
                            }
                        }
                        if (nAlphaCount != 0)
                        {
                            if (AlphaCount < nAlphaCount)
                                objRetrunValue = RETVAL.InvalidAlphabetCharacterLength;
                        }
                        if (nNumCount != 0)
                        {
                            if (NumCount < nNumCount)
                                objRetrunValue = RETVAL.InvalidNumericCharacterLength;
                        }
                        if (nSplCharCount != 0)
                        {
                            if (SplCharCount < nSplCharCount)
                                objRetrunValue = RETVAL.InvalidSpecialCharacterLength;
                        }
                        if (nUserIdType == "uppercase" && LowerCharacterCount > 0)
                        {
                            objRetrunValue = RETVAL.InvalidAlphabetLowerCharacter;
                        }
                        if (nUserIdType == "lowercase" && CapitalCharacterCount > 0)
                        {
                            objRetrunValue = RETVAL.InvalidAlphabetCapitalCharacter;
                        }
                        #endregion
                    }
                }
                else
                {
                    objRetrunValue = RETVAL.Successful;
                }
            }
            catch (Exception e)
            {
               
                throw e;
            }
            if (objRetrunValue == RETVAL.Nothing)
            {
                objRetrunValue = RETVAL.Successful;
            }
            return objRetrunValue;
        }

        private bool CheckRepeatations(string userID)
        {
            bool IsRepeat = false;
            int index;
            for (int i = 0; i < userID.Length; i++)
            {
                index = userID.IndexOf(userID[i], i + 1);
                if (index != -1)
                {
                    IsRepeat = true;
                    break;
                }
            }
            return IsRepeat;
        }

        private void CountCharacters(string UserID, int actualFirstCharCount, bool IsFirstCharSplChar, bool IsFirstCharNum, bool IsFirstCharAlphabet, out int AlphaCount, out int NumCount, out int SplCharCount, out int CapitalCharacterCount, out int LowerCharacterCount, out int FirstCharacterCount)
        {
            AlphaCount = NumCount = SplCharCount = CapitalCharacterCount = LowerCharacterCount = FirstCharacterCount = 0;

            for (int i = 0; i < UserID.Length; i++)
            {
                if (Char.IsDigit(UserID[i]))
                {
                    NumCount++;
                    if (IsFirstCharNum)
                    {
                        if (actualFirstCharCount > i)
                        {
                            FirstCharacterCount++;
                        }
                    }
                }
                else if (Char.IsLetter(UserID[i]))
                {
                    AlphaCount++;
                    if (IsFirstCharAlphabet)
                    {
                        if (actualFirstCharCount > i)
                        {
                            FirstCharacterCount++;
                        }
                    }
                    if (Char.IsLower(UserID[i]))
                    {
                        LowerCharacterCount++;
                    }
                    else
                    {
                        CapitalCharacterCount++;
                    }
                }
                else
                {
                    if (IsFirstCharSplChar)
                    {
                        if (actualFirstCharCount > i)
                        {
                            FirstCharacterCount++;
                        }
                    }
                    SplCharCount++;
                }
            }
        }
    }
    public enum RETVAL
    {
        Nothing = -2,
        InternalError = -1,
        Successful = 0,
        InvalidUserIdLength = 1,
        InvalidFirstCharacter = 2,
        RepeatNotAllowed = 3,
        ContaininigInvalidSpecialCharacters = 4,
        InvalidAlphabetCharacterLength = 5,
        InvalidNumericCharacterLength = 6,
        InvalidSpecialCharacterLength = 7,
        InvalidAlphaNumericCharacterLength = 8,
        NotContainingOnlyAlphabateCharacter = 9,
        NotContainingOnlyNumericCharacter = 10,
        InvalidAlphaNumericSplCharacterLength = 11,
        InvalidAlphabetCapitalCharacter = 12,
        InvalidAlphabetLowerCharacter = 13,
        InvalidStartSplCharCharacterLength = 14,
        InvalidStartNumCharacterLength = 15,
        InvalidStartAlphaCharacterLength = 16
    }
}
