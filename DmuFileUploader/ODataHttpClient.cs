namespace DmuFileUploader
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;

    public class ODataHttpClient : HttpClient
    {
        public ODataHttpClient(Uri endpointUri,
            AuthenticationHeaderValue authenticationHeader = null)
        {
            _ = endpointUri ?? throw new ArgumentNullException(nameof(endpointUri));


            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            base.BaseAddress = endpointUri;

            base.DefaultRequestHeaders.Authorization = authenticationHeader;
        }
    }
}
