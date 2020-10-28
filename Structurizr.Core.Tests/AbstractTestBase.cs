namespace Structurizr.Core.Tests
{
    public abstract class AbstractTestBase
    {
        protected Model Model;
        protected ViewSet Views;

        protected Workspace Workspace;

        public AbstractTestBase()
        {
            Workspace = new Workspace("Name", "Description");
            Model = Workspace.Model;
            Views = Workspace.Views;
        }
    }
}