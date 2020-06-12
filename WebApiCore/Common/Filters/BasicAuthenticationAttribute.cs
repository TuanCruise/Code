using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using WebApiCore.BasicAuthenFilter.Results;

namespace WebApiCore.BasicAuthenFilter.Filters {
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class BasicAuthenticationAttribute : Attribute, IAuthenticationFilter {
        public string Realm { get; set; }
        public async Task OnAuthorization(AuthorizationFilterContext filterContext)
        {
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null) {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }

            if (authorization.Scheme != "Basic") {
                context.ErrorResult = new AuthenticationFailureResult("Authorization Scheme is invalid, required Basic!", request);
                return;
            }

            if (string.IsNullOrEmpty(authorization.Parameter)) {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }

            var userNameAndPasword = ExtractUserNameAndPassword(authorization.Parameter);

            if (userNameAndPasword == null) {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", request);
                return;
            }

            var userName = userNameAndPasword.Item1;
            var password = userNameAndPasword.Item2;

            var principal = await AuthenticateAsync(userName, password, cancellationToken);

            if (principal == null) {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult("Invalid username or password", request);
            }
            else {
                // Authentication was attempted and succeeded. Set Principal to the authenticated user.
                context.Principal = principal;
            }
        }

        protected abstract Task<IPrincipal> AuthenticateAsync(string userName, string password,
            CancellationToken cancellationToken);

        public static Tuple<string, string> ExtractUserNameAndPassword(string authorizationParameter)
        {
            byte[] credentialBytes;

            try {
                credentialBytes = Convert.FromBase64String(authorizationParameter);
            }
            catch (FormatException) {
                //return null khi không phase được base64 => return null để raise 401: Unauthorized
                return null;
            }

            // The currently approved HTTP 1.1 specification says characters here are ISO-8859-1.
            // However, the current draft updated specification for HTTP 1.1 indicates this encoding is infrequently
            // used in practice and defines behavior only for ASCII.
            var encoding = Encoding.ASCII;
            // Make a writable copy of the encoding to enable setting a decoder fallback.
            encoding = (Encoding)encoding.Clone();
            // Fail on invalid bytes rather than silently replacing and continuing.
            encoding.DecoderFallback = DecoderFallback.ExceptionFallback;
            string decodedCredentials;

            try {
                decodedCredentials = encoding.GetString(credentialBytes);
            }
            catch (DecoderFallbackException) {
                //return null để raise 401: Unauthorized
                return null;
            }

            if (string.IsNullOrEmpty(decodedCredentials)) {
                return null;
            }

            var colonIndex = decodedCredentials.IndexOf(':');

            if (colonIndex == -1) {
                return null;
            }

            var userName = decodedCredentials.Substring(0, colonIndex);
            var password = decodedCredentials.Substring(colonIndex + 1);
            return new Tuple<string, string>(userName.ToLower(), password);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter;

            if (String.IsNullOrEmpty(Realm)) {
                parameter = null;
            }
            else {
                // A correct implementation should verify that Realm does not contain a quote character unless properly
                // escaped (precededed by a backslash that is not itself escaped).
                parameter = "realm=\"" + Realm + "\"";
            }

            context.ChallengeWith("Basic", parameter);
        }

        public virtual bool AllowMultiple {
            get { return false; }
        }
    }
}