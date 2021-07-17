namespace DmuFileUploader
{
    using JWT;
    using JWT.Algorithms;
    using JWT.Serializers;
    using Newtonsoft.Json.Linq;
    using System;

    public static class JWTService
    {
        public static DateTime GetValidTo(string accessToken)
        {
            var serializer = new JsonNetSerializer();
            var provider = new UtcDateTimeProvider();
            var validator = new JwtValidator(serializer, provider);

            var urlEncoder = new JwtBase64UrlEncoder();
            var algorithm = new HMACSHA256Algorithm();
            var decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

            JToken token = decoder.DecodeToObject<JToken>(accessToken);

            long timestamp = token.Value<long>("exp");

            DateTime utcDateTime = UnixTimestampToUtcDateTime(timestamp);

            return utcDateTime.ToLocalTime();
        }

        public static DateTime UnixTimestampToUtcDateTime(long unixTimestamp)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            DateTime utcDateTime = epoch.AddSeconds(unixTimestamp);

            return utcDateTime;
        }
    }
}
