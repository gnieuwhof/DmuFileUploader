namespace DmuFileUploader.ODataFilter
{
    using Newtonsoft.Json;
    using System;

    public class ODataValueFilterBase<T> : IODataFilter where T : IComparable
    {
        private readonly string field;
        private readonly CompareOperator op;
        private readonly T value;


        public bool IsValid { get; } = true;


        public enum CompareOperator
        {
            Equals,
            NotEquals,
            LessThan,
            GreaterThan,
            LessThanOrEqualTo,
            GreaterThanOrEqualTo
        }


        public ODataValueFilterBase(string field, CompareOperator op, T value)
        {
            this.field = field;
            this.op = op;
            this.value = value;
        }


        public string Expression()
        {
            string compareOperator = GetCompareOperator();

            string formattedValue;
            switch (this.value)
            {
                case int _:
                case double _:
                case Guid _:
                    formattedValue = $"{this.value}";
                    break;
                case string _:
                    formattedValue = $"'{this.value}'";
                    break;
                case DateTime _:
                    string iso8601 = JsonConvert.SerializeObject(this.value);
                    formattedValue = iso8601.Trim('"');
                    break;
                default:
                    throw new NotImplementedException();
            }

            string expression = $"{this.field} {compareOperator} {formattedValue}";

            return expression;
        }

        private string GetCompareOperator()
        {
            switch (this.op)
            {
                case CompareOperator.Equals:
                    return "eq";
                case CompareOperator.NotEquals:
                    return "ne";
                case CompareOperator.LessThan:
                    return "lt";
                case CompareOperator.GreaterThan:
                    return "gt";
                case CompareOperator.LessThanOrEqualTo:
                    return "le";
                case CompareOperator.GreaterThanOrEqualTo:
                    return "ge";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
