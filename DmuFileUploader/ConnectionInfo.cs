namespace DmuFileUploader
{
    using System;
    using System.Net.Http.Headers;

    public class ConnectionInfo
    {
        public ConnectionInfo(
            Uri resource,
            string username,
            string password,
            AuthenticationHeaderValue authHeader,
            DateTime validTo
            )
        {
            this.Resource = resource ??
                throw new ArgumentNullException(nameof(resource));

            this.Username = username ??
                throw new ArgumentNullException(nameof(username));

            this.Password = password ??
                throw new ArgumentNullException(nameof(password));

            this.AuthHeader = authHeader ??
                throw new ArgumentNullException(nameof(authHeader));

            this.UtcValidTo = validTo;
        }


        public Uri Resource { get; }

        public string Username { get; }

        public string Password { get; }

        public AuthenticationHeaderValue AuthHeader { get; }

        public DateTime UtcValidTo { get; set; }
    }
}
