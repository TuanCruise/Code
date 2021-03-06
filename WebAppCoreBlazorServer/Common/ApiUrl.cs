﻿namespace WebAppCoreBlazorServer.Common
{
    public static class ApiUrl
    {
        public const string AuthorizeEndpoint = "/connect/authorize";
        public const string LogoutEndpoint = "/connect/endsession";
        public const string TokenEndpoint = "/connect/token";
        public const string UserInfoEndpoint = "/connect/userinfo";
        public const string IdentityTokenValidationEndpoint = "/connect/identitytokenvalidation";
        public const string TokenRevocationEndpoint = "/connect/revocation";
    }
}