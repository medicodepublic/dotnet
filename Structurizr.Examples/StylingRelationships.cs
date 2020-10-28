using Structurizr.Api;

namespace Structurizr.Examples
{
    /// <summary>
    ///     An example of how to style relationships on diagrams.
    ///     The live workspace is available to view at https://structurizr.com/share/36131
    /// </summary>
    internal class StylingRelationships
    {
        private const long WorkspaceId = 36131;
        private const string ApiKey = "key";
        private const string ApiSecret = "secret";

        private static void Main()
        {
            var workspace = new Workspace("Styling Relationships", "This is a model of my software system.");
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
//            styles.Add(new RelationshipStyle(Tags.Relationship) { Color = "#ff0000" });

            // example 2
//            model.Relationships.Where(r => "HTTPS".Equals(r.Technology)).ToList().ForEach(r => r.AddTags("HTTPS"));
//            model.Relationships.Where(r => "JDBC".Equals(r.Technology)).ToList().ForEach(r => r.AddTags("JDBC"));
//            styles.Add(new RelationshipStyle("HTTPS") { Color = "#ff0000" });
//            styles.Add(new RelationshipStyle("JDBC") { Color = "#0000ff" });

            var structurizrClient = new StructurizrClient(ApiKey, ApiSecret);
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }
    }
}