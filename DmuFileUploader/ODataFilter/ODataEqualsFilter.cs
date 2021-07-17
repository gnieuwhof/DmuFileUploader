namespace DmuFileUploader.ODataFilter
{
    using System;

    public class ODataEqualsFilter<T> : ODataValueFilterBase<T> where T : IComparable
    {
        public ODataEqualsFilter(string field, T value)
            : base(field, CompareOperator.Equals, value)
        {
        }
    }
}
