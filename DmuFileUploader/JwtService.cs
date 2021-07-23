namespace DmuFileUploader
{
    using System;

    public static class JWTService
    {
        public static DateTime GetUtcIssuedAt(string accessToken)
        {
            var jsonWebToken = JsonWebToken.Create(accessToken);

            long timestamp = jsonWebToken.GetValue<long>("iat");

            DateTime utcDateTime = UnixTimestampToUtcDateTime(timestamp);

            return utcDateTime;
        }

        public static DateTime GetUtcValidTo(string accessToken)
        {
            DateTime utcValidTo = GetUtcDateTime(accessToken, "exp");

            return utcValidTo;
        }

        private static DateTime GetUtcDateTime(string accessToken, string key)
        {
            var jsonWebToken = JsonWebToken.Create(accessToken);

            long timestamp = jsonWebToken.GetValue<long>(key);

            DateTime utcDateTime = UnixTimestampToUtcDateTime(timestamp);

            return utcDateTime;
        }

        private static DateTime UnixTimestampToUtcDateTime(long unixTimestamp)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            DateTime utcDateTime = epoch.AddSeconds(unixTimestamp);

            return utcDateTime;
        }
    }
}
