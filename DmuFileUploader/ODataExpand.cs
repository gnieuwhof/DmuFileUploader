namespace DmuFileUploader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ODataExpand
    {
        private readonly Dictionary<string, IEnumerable<string>> expands;


        public ODataExpand(string field, params string[] columns)
            : this(new Dictionary<string, IEnumerable<string>> { { field, columns } })
        {
        }

        public ODataExpand(Dictionary<string, IEnumerable<string>> expands)
        {
            this.expands = expands ??
                throw new ArgumentNullException(nameof(expands));
        }


        public string Expression()
        {
            var expandList = new List<string>();
            foreach (var expand in expands)
            {
                string entity = expand.Key;
                IEnumerable<string> fields = expand.Value;

                string query = entity;

                if (fields?.Any() == true)
                {
                    string fieldsQuery = string.Join(",", fields);

                    query += $"($select={fieldsQuery})";
                }

                expandList.Add(query);
            }

            string expression = string.Join(",", expandList);

            return expression;
        }
    }
}
