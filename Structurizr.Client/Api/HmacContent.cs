using System.Text;

namespace Structurizr.Api
{
    internal class HmacContent
    {
        private readonly string[] strings;

        internal HmacContent(params string[] strings)
        {
            this.strings = strings;
        }

        public override string ToString()
        {
            var buf = new StringBuilder();
            foreach (var s in strings)
            {
                buf.Append(s);
                buf.Append("\n");
            }

            return buf.ToString();
        }
    }
}