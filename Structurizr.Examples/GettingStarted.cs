using Structurizr.Api;

namespace Structurizr.Examples
{
    /// <summary>
    ///     A "getting started" example that illustrates how to
    ///     create a software architecture diagram using code.
    ///     The live workspace is available to view at https://structurizr.com/share/25441
    /// </summary>
    internal class GettingStarted
    {
        private const long WorkspaceId = 25441;
        private const string ApiKey = "key";
        private const string ApiSecret = "secret";

        private static void Main()
        {
            var workspace = new Workspace("Getting Started", "This is a model of my software system.");
            var model = workspace.Model;

            var user = model.AddPerson("User", "A user of my software system.");
            var softwareSystem = model.AddSoftwareSystem("Software System", "My software system.");
            user.Uses(softwareSystem, "Uses");

            var viewSet = workspace.Views;
            var contextView = viewSet.CreateSystemContextView(softwareSystem, "SystemContext",
                "An example of a System Context diagram.");
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            var styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle(Tags.SoftwareSystem) {Background = "#1168bd", Color = "#ffffff"});
            styles.Add(new ElementStyle(Tags.Person) {Background = "#08427b", Color = "#ffffff", Shape = Shape.Person});

            var structurizrClient = new StructurizrClient(ApiKey, ApiSecret);
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }
    }
}