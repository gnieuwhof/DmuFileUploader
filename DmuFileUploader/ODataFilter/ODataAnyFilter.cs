namespace DmuFileUploader.ODataFilter
{
    public class ODataAnyFilter : ODataCombineFilterBase
    {
        public ODataAnyFilter(params IODataFilter[] filters)
            : base(BooleanOperator.Or, filters)
        {
        }
    }
}
