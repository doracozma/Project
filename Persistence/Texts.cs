namespace Persistence
{
    public class Texts
    {
        public static readonly TextKey emailActivateAccount = new TextKey("emailActivateAccount");
        public static readonly TextKey emailSubjectActivateAccount = new TextKey("emailSubjectActivateAccount");
        public static readonly TextKey emailLostPassword = new TextKey("emailLostPassword");
        public static readonly TextKey emailAddCoupleLink = new TextKey("emailAddCoupleLink");
        public static readonly TextKey emailAddCoupleLinkSubject = new TextKey("emailAddCoupleLinkSubject");
        public static readonly TextKey emailLostPasswordChange = new TextKey("emailLostPasswordChange");
        public static readonly TextKey emailSubjectLostPasswordChange = new TextKey("emailSubjectLostPasswordChange");
        public static readonly TextKey emailSubjectLostPassword = new TextKey("emailSubjectLostPasswordChange");
        public static readonly TextKey smsActivateAccount = new TextKey("smsActivateAccount");
        public static readonly TextKey smsLostPasswordChange = new TextKey("smsLostPasswordChange");
        public static readonly TextKey smsAddCoupleLink = new TextKey("smsAddCoupleLink");
        public static readonly TextKey smsLostPassword = new TextKey("smsLostPassword");


        public static readonly string macroCode = "code";
        public static readonly string macroUrl = "url";
        public static readonly string macroInviterName = "inviterName";
        public static readonly string macroPassword = "password";
    }

    public class TextKey
    {
        private string key { get; set; }

        public TextKey(string key)
        {
            this.key = key;
        }

        public string getKey()
        {
            return this.key;
        }
     
    }
}