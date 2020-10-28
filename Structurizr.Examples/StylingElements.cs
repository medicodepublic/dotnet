using Structurizr.Api;

namespace Structurizr.Examples
{
    /// <summary>
    ///     An example of how to style elements on diagrams.
    ///     The live workspace is available to view at https://structurizr.com/share/36111
    /// </summary>
    internal class StylingElements
    {
        private const long WorkspaceId = 36111;
        private const string ApiKey = "key";
        private const string ApiSecret = "secret";

        private static void Main()
        {
            var workspace = new Workspace("Styling Elements", "This is a model of my software system.");
            var model = workspace.Model;

            var user = model.AddPerson("User", "A user of my software system.");
            var softwareSystem = model.AddSoftwareSystem("Software System", "My software system.");
            var webApplication =
                softwareSystem.AddContainer("Web Application", "My web application.", "Java and Spring MVC");
            var database = softwareSystem.AddContainer("Database", "My database.", "Relational database schema");
            user.Uses(webApplication, "Uses", "HTTPS");
            webApplication.Uses(database, "Reads from and writes to", "JDBC");

            var views = workspace.Views;
            var containerView =
                views.CreateContainerView(softwareSystem, "containers", "An example of a container diagram.");
            containerView.AddAllElements();

            var styles = workspace.Views.Configuration.Styles;

            // example 1
//            styles.Add(new ElementStyle(Tags.Element) { Background = "#438dd5", Color = "#ffffff" });

            // example 2
//            styles.Add(new ElementStyle(Tags.Element) { Color = "#ffffff" });
//            styles.Add(new ElementStyle(Tags.Person) { Background = "#08427b" });
//            styles.Add(new ElementStyle(Tags.Container) { Background = "#438dd5" });

            // example 3
//            styles.Add(new ElementStyle(Tags.Element) { Color = "#ffffff" });
//            styles.Add(new ElementStyle(Tags.Person) { Background = "#08427b" , Shape = Shape.Person });
//            styles.Add(new ElementStyle(Tags.Container) { Background = "#438dd5" });
//            database.AddTags("Database");
//            styles.Add(new ElementStyle("Database") { Shape = Shape.Cylinder });

            var structurizrClient = new StructurizrClient(ApiKey, ApiSecret);
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }
    }
}