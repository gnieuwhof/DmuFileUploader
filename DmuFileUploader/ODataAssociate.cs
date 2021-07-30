namespace DmuFileUploader
{
    using Newtonsoft.Json;
    using System;

    // https://docs.microsoft.com/en-us/powerapps/developer/common-data-service/webapi/associate-disassociate-entities-using-web-api#add-a-reference-to-a-collection-valued-navigation-property
    public class ODataAssociate
    {
        public ODataAssociate(Uri baseAddress, string entityPluralName, Guid id)
        {
            this.ODataId = $"{baseAddress}{Program.ApiPath}{entityPluralName}({id})";
        }


        [JsonProperty(PropertyName = "@odata.id")]
        public string ODataId { get; set; }
    }
}
