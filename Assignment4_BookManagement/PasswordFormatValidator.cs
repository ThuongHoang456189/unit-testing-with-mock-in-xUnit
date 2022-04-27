using System;
using System.Collections.Generic;

namespace Assignment4_BookManagement
{
    public class PasswordFormatValidator
    {
        private const string PasswordField = "Password";
        private const string ConfirmPasswordField = "Confirm Password";

        public PasswordFormatValidator()
        {
        }
        public string PasswordStr
        {
            get { return PasswordStr; }
            set { PasswordStr = value; }
        }
        public string ConfirmPasswordStr
        {
            get { return ConfirmPasswordStr; }
            set { ConfirmPasswordStr = value; }
        }
        public Dictionary<string, string> FieldsMsg
        {
            get { return FieldsMsg; }
            set { FieldsMsg = value; }
        }
        public bool ValidatePassword(string PasswordStr, out string FieldMsg)
        {
            if (String.IsNullOrWhiteSpace(PasswordStr))
            {
                FieldMsg = "The Password is required not blank.";
                return false;
            }
            else
            {
                FieldMsg = String.Empty;
                return true;
            }
        }
        public bool ValidateNewPassword()
        {
            string PwdMsg;
            string ConfirmPwdMsg;
            bool IsValid = ValidatePassword(PasswordStr, out PwdMsg);
            if (IsValid)
            {
                if (!String.IsNullOrWhiteSpace(ConfirmPasswordStr) && String.Compare(PasswordStr, ConfirmPasswordStr, false) == 0)
                {
                    ConfirmPwdMsg = String.Empty;
                    IsValid = true;
                }
                else
                {
                    ConfirmPwdMsg = "Confirm Password must match the Password field.";
                    IsValid = false;
                }
            }
            else
            {
                ConfirmPwdMsg = "Please enter corresponding password after changing the blanked Password field.";
                IsValid = false;
            }

            FieldsMsg = new Dictionary<string, string>();

            if (!IsValid)
            {
                FieldsMsg.Add(PasswordField, PwdMsg);
                FieldsMsg.Add(ConfirmPasswordField, ConfirmPwdMsg);
            }

            return IsValid;
        }
    }
}
