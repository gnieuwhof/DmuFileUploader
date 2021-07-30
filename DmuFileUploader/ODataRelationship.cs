namespace DmuFileUploader
{
    using Newtonsoft.Json;
    using System;

    public class ODataRelationship
    {
        private ODataRelationship(string requestUri,
            string content, string sourceId, string targetId)
        {
            this.RequestUri = requestUri;

            this.Content = content;

            this.SourceId = sourceId;

            this.TargetId = targetId;
        }


        public string RequestUri { get; private set; }

        public string Content { get; private set; }

        public string SourceId { get; private set; }

        public string TargetId { get; private set; }


        public static ODataRelationship Create(Uri baseAddress, string resourcePath,
            string relationshipName, string sourceId, string targetId)
        {
            string requestUri = $"{resourcePath}({sourceId})/{relationshipName}/$ref";

            var id = new Guid(targetId);
            var assoc = new ODataAssociate(baseAddress, resourcePath, id);

            string json = JsonConvert.SerializeObject(assoc);

            var result = new ODataRelationship(requestUri, json, sourceId, targetId);

            return result;
        }
    }
}
