namespace DmuFileUploader
{
    using System.Linq;

    public class ODataExpand
    {
        public ODataExpand(string field)
        {
            this.Field = field;
        }

        public ODataExpand(string field, params string[] columns)
        {
            this.Field = field;

            this.Select = columns;
        }


        public string Field { get; }

        public string[] Select { get; }


        public string Expression()
        {
            string expression = this.Field;

            if (this.Select?.Any() == true)
            {
                expression = $"{expression}($select={string.Join(",", this.Select)})";
            }

            return expression;
        }
    }
}
