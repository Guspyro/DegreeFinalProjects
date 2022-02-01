public struct ConstantsBackend
{
    public const string GameName = "game-pcg";

    public const string BaseUrlAuth = "https://4njsldxxh0.execute-api.eu-west-3.amazonaws.com/prod/";
    public const string UrlSignin = "signin";
    public const string UrlSignup = "signup";
    public const string UrlChangePassword = "edituser/password";
    public const string UrlForgotPassword = "forgotpassword";
    public const string UrlRefreshAccessToken = "refreshaccesstoken";

    public const string BaseUrlCloudStorage = "https://ggx1do2fu3.execute-api.eu-west-3.amazonaws.com/prod/";
    public const string UrlWorlds = "worlds";
    public const string UrlWorld = "world";
    
    public const int MaxCloudWorlds = 5;

    public struct ErrorMessages
    {
        public const string MandatoryFields = "All fields are mandatory.";
        public const string EmailFormat = "Enter a valid email.";
        public const string PasswordLength = "Passwords must have between 6 and 255 characters.";
        public const string PasswordMatch = "Passwords don't match.";
        public const string TermsNotAccepted = "You must be 18 years or older to create an account.";
        public const string InvalidAccessToken = "An error ocurred. Try to sign in again.";
        public const string NotSignedin = "Not signed in.";
        public const string MaxCloudWorldsReached = "Maximum amount of worlds in cloud reached.";
        public const string DeleteWorld = "Error trying to delete world.";
    }

    public struct SuccessMessages
    {
        public const string Signup = "Account created successfully. Check your email for verification link.";
        public const string Signin = "Sign in succesful.";
        public const string ChangePassword = "Password changed successfully.";
        public const string ForgotPassword = "Check your email to get the recovery code.";
        public const string ConfirmForgotPassword = "Password changed successfully.";
        public const string DownloadWorld = "World moved to your device.";
        public const string UploadWorld = "World moved to cloud.";
        public const string DeleteWorld = "World deleted.";
    }

    public const string EmailPattern =
    @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
    + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
    + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
    + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

}
