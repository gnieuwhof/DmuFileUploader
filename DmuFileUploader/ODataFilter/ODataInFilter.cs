namespace DmuFileUploader.ODataFilter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ODataInFilter<T> : IODataFilter where T : IComparable
    {
        private readonly IEnumerable<ODataEqualsFilter<T>> filters;


        public bool IsValid { get; }


        public ODataInFilter(string field, params T[] values)
        {
            this.filters = values?.Select(v => new ODataEqualsFilter<T>(field, v));

            this.IsValid = (this.filters?.Any() == true);
        }

        public ODataInFilter(string field, IEnumerable<T> values)
            : this(field, values?.ToArray())
        {
        }


        public string Expression()
        {
            var anyFilter = new ODataAnyFilter(this.filters.ToArray());

            string expression = anyFilter.Expression();

            return expression;
        }
    }
}
