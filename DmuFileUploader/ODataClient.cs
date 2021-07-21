﻿namespace DmuFileUploader
{
    using DmuFileUploader.ODataFilter;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public class ODataClient
    {
        private readonly HttpClient httpClient;


        public ODataClient(ODataHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }


        public async Task<T> FindEntryAsync<T>(
            string resourcePath,
            IODataFilter oDataFilter,
            ODataExpand oDataExpand,
            params string[] columns
            )
        {
            HttpResponseMessage result = await GetAsync(
                resourcePath, oDataFilter, oDataExpand, columns);

            string content = await result.Content.ReadAsStringAsync();

            ODataResponse<T> response =
                JsonConvert.DeserializeObject<ODataResponse<T>>(content);

            return response.Value.FirstOrDefault();
        }

        public Task<HttpResponseMessage> PostAsync(string entitySetName, string json)
        {
            return CallAsync(HttpMethod.Post, entitySetName, json);
        }

        public Task<HttpResponseMessage> PatchAsync(
            string entitySetName, string json, Guid id)
        {
            string resourcePath = $"{entitySetName}({id})";

            return CallAsync(new HttpMethod("PATCH"), resourcePath, json);
        }

        public Task<HttpResponseMessage> GetAsync(
            string resourcePath, IODataFilter oDataFilter, params string[] columns)
        {
            string filter = oDataFilter.Expression();

            string query = filter.Length > 0 ? filter.Insert(0, "?$filter=") : string.Empty;

            if (columns?.Length > 0)
            {
                query = $"{query}&$select={string.Join(",", columns)}";
            }

            return CallAsync(HttpMethod.Get, $"{resourcePath}{query}");
        }

        public Task<HttpResponseMessage> GetAsync(string resourcePath,
            IODataFilter oDataFilter, ODataExpand oDataExpand, params string[] columns)
        {
            var queryDict = new Dictionary<string, string>();

            if (oDataFilter != null)
            {
                queryDict.Add("$filter", oDataFilter.Expression());
            }

            if (columns?.Length > 0)
            {
                queryDict.Add("$select", string.Join(",", columns));
            }

            if (oDataExpand != null)
            {
                queryDict.Add("$expand", oDataExpand.Expression());
            }

            return GetAsync(resourcePath, queryDict);
        }

        public Task<HttpResponseMessage> GetAsync(
            string relativePath, IDictionary<string, string> queryParams = null)
        {
            string requestUri = relativePath;

            if (queryParams?.Any() == true)
            {
                var parts = new List<string>();

                foreach (KeyValuePair<string, string> kvp in queryParams)
                {
                    string queryParam = $"{kvp.Key}={kvp.Value}";

                    parts.Add(queryParam);
                }

                string query = string.Join("&", parts);

                requestUri = $"{requestUri}?{query}";
            }

            return CallAsync(HttpMethod.Get, requestUri);
        }

        private async Task<HttpResponseMessage> CallAsync(
            HttpMethod method, string requestUri, string jsonContent = null)
        {
            var request = new HttpRequestMessage(method, requestUri);

            if (jsonContent != null)
            {
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            }

            var response = await this.httpClient.SendAsync(request);

            int tooManyRequests = 429;
            if ((int)response.StatusCode == tooManyRequests)
            {
                int delaySeconds = 10;

                response.Headers.TryGetValues(
                    "Retry-After", out IEnumerable<string> retryAfterHeaders);

                string retryAfter = retryAfterHeaders?.FirstOrDefault();

                if (int.TryParse(retryAfter, out int retryAfterSeconds))
                {
                    delaySeconds = retryAfterSeconds;
                }

                await Task.Delay(delaySeconds * 1000);

                response = await CallAsync(method, requestUri, jsonContent);
            }

            return response;
        }
    }
}