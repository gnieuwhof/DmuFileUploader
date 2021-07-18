namespace DmuFileUploader
{
    using System;

    public static class JWTService
    {
        public static DateTime GetUtcValidTo(string accessToken)
        {
            var jsonWebToken = JsonWebToken.Create(accessToken);

            long timestamp = jsonWebToken.GetValue<long>("exp");

            DateTime utcDateTime = UnixTimestampToUtcDateTime(timestamp);

            return utcDateTime;
        }

        public static DateTime UnixTimestampToUtcDateTime(long unixTimestamp)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            DateTime utcDateTime = epoch.AddSeconds(unixTimestamp);

            return utcDateTime;
        }
    }
}
