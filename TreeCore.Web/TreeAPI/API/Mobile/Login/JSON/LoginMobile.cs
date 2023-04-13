using System.Collections.Generic;

namespace TreeAPI.API.Mobile.Login.JSON
{
    // THIS MUST MATCH THE CLIENT

    public class MobileModuleAccess
    {
        public long ModuleID { get; set; }
    }

    public class UserMobileSession
    {
        public string UserName { get; set; }
        public long UserID { get; set; }
        public string Locale { get; set; }
        public string LoginTime { get; set; }
        public string Email { get; set; }

        public List<MobileModuleAccess> UserMobileAccess { get; set; }
    }

    public class LoginMobileSessionPackage
    {
        public bool RequestResult { get; set; }
        public string RequestResultString { get; set; }
        public long BlockSize { get; set; }
        public string BlockData { get; set; }

    }

    public class LoginMobileSessionPackageV3
    {
        public bool RequestResult { get; set; }
        public string RequestResultString { get; set; }
        public long BlockSize { get; set; }
        public string BlockData { get; set; }
        public string GeneratedCodeDevice { get; set; }

    }
}
