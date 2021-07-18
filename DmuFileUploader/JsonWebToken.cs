namespace DmuFileUploader
{
    using JWT;
    using JWT.Algorithms;
    using JWT.Serializers;
    using Newtonsoft.Json.Linq;
    using System;

    public class JsonWebToken
    {
        private readonly JToken jToken;


        private JsonWebToken(JToken jToken)
        {
            this.jToken = jToken;
        }


        public static JsonWebToken Create(string accessToken)
        {
            _ = accessToken ??
                throw new ArgumentNullException(nameof(accessToken));

            var serializer = new JsonNetSerializer();
            var provider = new UtcDateTimeProvider();
            var validator = new JwtValidator(serializer, provider);

            var urlEncoder = new JwtBase64UrlEncoder();
            var algorithm = new HMACSHA256Algorithm();
            var decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

            JToken token = decoder.DecodeToObject<JToken>(accessToken);

            var result = new JsonWebToken(token);

            return result;
        }

        public T GetValue<T>(string key)
        {
            T result = this.jToken.Value<T>(key);

            return result;
        }
    }
}
