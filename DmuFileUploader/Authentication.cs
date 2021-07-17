namespace DmuFileUploader
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public static class Authentication
    {
        public static async Task<AuthenticationHeaderValue> GetAuthenticationHeader(
            HttpClient httpClient,
            Uri authorizationUrl,
            Uri resource,
            string clientId,
            string username,
            string password
            )
        {
            HttpResponseMessage responseMessage = await GetAuthResponse(
                httpClient, authorizationUrl, resource, clientId, username, password);

            string jsonResponse = await responseMessage.Content.ReadAsStringAsync();

            var jsonContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);

            string token = jsonContent["access_token"];

            var header = new AuthenticationHeaderValue("Bearer", token);

            return header;
        }


        public static async Task<HttpResponseMessage> GetAuthResponse(
            HttpClient httpClient,
            Uri authorizationUrl,
            Uri resource,
            string clientId,
            string username,
            string password
            )
        {
            string postData =
                $"resource={resource}" +
                $"&client_id={clientId}" +
                $"&grant_type=password" +
                $"&username={username}" +
                $"&password={password}" +
                $"&scope=openid";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, authorizationUrl);
            request.Content = new StringContent(postData, Encoding.UTF8);
            request.Content.Headers.Remove("Content-Type");
            request.Content.Headers.TryAddWithoutValidation("Content-Type", $"application/x-www-form-urlencoded");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpResponseMessage response = await httpClient.SendAsync(request);

            return response;
        }

        public static async Task<AuthenticationHeaderValue> GetAuthenticationHeader(
            HttpResponseMessage authResponse)
        {
            string jsonResponse = await authResponse.Content.ReadAsStringAsync();

            var jsonContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);

            string token = jsonContent["access_token"];

            var header = new AuthenticationHeaderValue("Bearer", token);

            return header;
        }

        public static async Task<OAuthError> GetError(HttpResponseMessage authResponse)
        {
            string content = await authResponse.Content.ReadAsStringAsync();

            OAuthError oAuthError = JsonConvert.DeserializeObject<OAuthError>(content);

            return oAuthError;
        }
    }
}
