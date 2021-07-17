namespace DmuFileUploader
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class ODataResponse<T>
    {
        [DataMember(Name = "@odata.context")]
        public string ODataContext
        {
            get; set;
        }

        [DataMember(Name = "value")]
        public List<T> Value
        {
            get; set;
        }
    }
}
