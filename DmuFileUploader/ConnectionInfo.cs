namespace DmuFileUploader
{
    using System;
    using System.Net.Http.Headers;

    public class ConnectionInfo
    {
        public ConnectionInfo(
            Uri resource,
            Uri authorizeUrl,
            string username,
            string password,
            AuthenticationHeaderValue authHeader
            )
        {
            this.Resource = resource ??
                throw new ArgumentNullException(nameof(resource));

            this.AuthorizeUrl = authorizeUrl ??
                throw new ArgumentNullException(nameof(authorizeUrl));

            this.Username = username ??
                throw new ArgumentNullException(nameof(username));

            this.Password = password ??
                throw new ArgumentNullException(nameof(password));

            this.SetHeader(authHeader);
        }


        public Uri Resource { get; }

        public Uri AuthorizeUrl { get; }

        public string Username { get; }

        public string Password { get; }

        public AuthenticationHeaderValue AuthHeader { get; private set; }

        public DateTime UtcValidTo { get; private set; }


        public void SetHeader(AuthenticationHeaderValue authHeader)
        {
            this.AuthHeader = authHeader ??
                throw new ArgumentNullException(nameof(authHeader));

            string token = $"{authHeader}".Substring("Bearer ".Length);

            this.UtcValidTo = JWTService.GetUtcValidTo(token);
        }

        public bool IsValid(TimeSpan margin)
        {
            var utcNow = DateTime.UtcNow;

            var validRemaining = this.UtcValidTo.Subtract(utcNow);

            return (validRemaining > margin);
        }
    }
}
