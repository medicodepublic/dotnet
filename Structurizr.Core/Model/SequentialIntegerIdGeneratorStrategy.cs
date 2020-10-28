namespace Structurizr
{
    internal class SequentialIntegerIdGeneratorStrategy
    {
        private int Id;

        internal void Found(string id)
        {
            var idAsInt = int.Parse(id);
            if (idAsInt > Id) Id = idAsInt;
        }

        internal string GenerateId(Element element)
        {
            lock (this)
            {
                return "" + ++Id;
            }
        }


        internal string GenerateId(Relationship relationship)
        {
            lock (this)
            {
                return "" + ++Id;
            }
        }
    }
}