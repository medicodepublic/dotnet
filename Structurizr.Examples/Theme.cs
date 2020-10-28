using Structurizr.Api;

namespace Structurizr.Examples
{
    /// <summary>
    ///     This is an example of how to use an external theme.
    ///     The live workspace is available to view at https://structurizr.com/share/38898
    /// </summary>
    internal class Theme
    {
        private const long WorkspaceId = 38898;
        private const string ApiKey = "";
        private const string ApiSecret = "";

        private static void Main()
        {
            var workspace = new Workspace("Theme", "This is a model of my software system.");
            var model = workspace.Model;

            var user = model.AddPerson("User", "A user of my software system.");
            var softwareSystem = model.AddSoftwareSystem("Software System", "My software system.");
            user.Uses(softwareSystem, "Uses");

            var viewSet = workspace.Views;
            var contextView = viewSet.CreateSystemContextView(softwareSystem, "SystemContext",
                "An example of a System Context diagram.");
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            // add a theme
            viewSet.Configuration.Theme =
                "https://raw.githubusercontent.com/structurizr/dotnet/master/Structurizr.Examples/Theme/theme.json";

            var structurizrClient = new StructurizrClient(ApiKey, ApiSecret);
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }
    }
}