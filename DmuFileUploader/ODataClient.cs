namespace DmuFileUploader
{
    using DmuFileUploader.ODataFilter;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class ODataClient
    {
        private const int TOO_MANY_REQUESTS = 429;

        private readonly HttpClient httpClient;
        private readonly ConnectionInfo connectionInfo;
        private readonly Action<string> WriteLine;
        private readonly SemaphoreSlim semaphore =
            AsyncLock.CreateSemaphore();


        public ODataClient(ODataHttpClient httpClient,
            ConnectionInfo connectionInfo, Action<string> writeLine)
        {
            this.httpClient = httpClient ??
                throw new ArgumentNullException(nameof(httpClient));

            this.connectionInfo = connectionInfo ??
                throw new ArgumentNullException(nameof(connectionInfo));

            this.WriteLine = writeLine ??
                throw new ArgumentNullException(nameof(writeLine));
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

        private Task<HttpResponseMessage> CallAsync(
            HttpMethod method, string requestUri, string jsonContent = null)
        {
            return InnerCallAsync(method, requestUri, jsonContent);
        }

        private async Task<HttpResponseMessage> InnerCallAsync(
            HttpMethod method, string requestUri, string jsonContent, int retryCount = 0)
        {
            await this.CheckToken();

            var request = new HttpRequestMessage(method, requestUri);

            if (jsonContent != null)
            {
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            }

            var response = await this.httpClient.SendAsync(request);

            if ((int)response.StatusCode == TOO_MANY_REQUESTS)
            {
                ++retryCount;
                int delaySeconds = GetDelaySeconds(response, retryCount);

                this.WriteLine($"Too many requests, waiting {delaySeconds} seconds (retry count: {retryCount}).");

                await Task.Delay(TimeSpan.FromSeconds(delaySeconds));

                response = await InnerCallAsync(method, requestUri, jsonContent, retryCount);
            }

            return response;
        }

        private async Task CheckToken()
        {
            using (await AsyncLock.Enter(semaphore))
            {
                if (!this.connectionInfo.IsValid(TimeSpan.FromMinutes(1)))
                {
                    var header = await Authentication.GetAuthenticationHeader(
                        this.httpClient,
                        this.connectionInfo.AuthorizeUrl,
                        this.connectionInfo.Resource,
                        LoginFrm.CLIENT_ID,
                        this.connectionInfo.Username,
                        this.connectionInfo.Password
                        );

                    this.connectionInfo.SetHeader(header);

                    this.WriteLine("New OAuth token retrieved.");
                }
            }
        }

        private static int GetDelaySeconds(HttpResponseMessage response, int retryCount)
        {
            int delaySeconds;

            response.Headers.TryGetValues(
                "Retry-After", out IEnumerable<string> retryAfterHeaders);

            string retryAfter = retryAfterHeaders?.FirstOrDefault();

            if (int.TryParse(retryAfter, out int retryAfterSeconds))
            {
                delaySeconds = retryAfterSeconds;
            }
            else
            {
                // Exponentially increase the delay.
                delaySeconds = (int)Math.Pow(2, retryCount);
            }

            return delaySeconds;
        }
    }
}
