namespace DmuFileUploader.ODataFilter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ODataCombineFilterBase : IODataFilter
    {
        private readonly BooleanOperator op;
        private readonly IODataFilter[] filters;


        public bool IsValid { get; }


        public enum BooleanOperator
        {
            And,
            Or
        }


        public ODataCombineFilterBase(BooleanOperator op, params IODataFilter[] filters)
        {
            this.op = op;

            this.filters = (op == BooleanOperator.And)
                ? filters
                : filters?.Where(f => f.IsValid).ToArray();

            this.IsValid = (this.filters?.Any() == true);

            if (op == BooleanOperator.And)
            {
                this.IsValid &= (filters.All(f => f.IsValid) == true);
            }
        }


        public string Expression()
        {
            if (this.filters?.Any() != true)
            {
                throw new InvalidOperationException("There are no filters to combine.");
            }

            if (this.filters.Length == 1)
            {
                IODataFilter filter = this.filters[0];
                return filter.Expression();
            }

            IEnumerable<string> inParentheses =
                this.filters.Select(f => $"({f.Expression()})");

            string separator = (this.op == BooleanOperator.And) ? "and" : "or";

            string expression = string.Join(separator, inParentheses);

            return expression;
        }
    }
}
