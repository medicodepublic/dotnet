using System;
using Structurizr.Api;

namespace Structurizr.Examples
{
    /// <summary>
    ///     This is an example of how to use the HTTP-based health checks feature.
    ///     You can see the health checks running at https://structurizr.com/share/39441/health
    /// </summary>
    public class HttpHealthChecks
    {
        private const long WorkspaceId = 39441;
        private const string ApiKey = "key";
        private const string ApiSecret = "secret";

        private const string DatabaseTag = "Database";

        private static void Main()
        {
            var workspace = new Workspace("HTTP-based health checks example",
                "An example of how to use the HTTP-based health checks feature");
            var model = workspace.Model;
            var views = workspace.Views;

            var structurizr = model.AddSoftwareSystem("Structurizr",
                "A publishing platform for software architecture diagrams and documentation based upon the C4 model.");
            var webApplication = structurizr.AddContainer("structurizr.com",
                "Provides all of the server-side functionality of Structurizr, serving static and dynamic content to users.",
                "Java and Spring MVC");
            var database = structurizr.AddContainer("Database", "Stores information about users, workspaces, etc.",
                "Relational Database Schema");
            database.AddTags(DatabaseTag);
            webApplication.Uses(database, "Reads from and writes to", "JDBC");

            var amazonWebServices = model.AddDeploymentNode("Amazon Web Services", "", "us-east-1");
            var pivotalWebServices = amazonWebServices.AddDeploymentNode("Pivotal Web Services",
                "Platform as a Service provider.", "Cloud Foundry");
            var liveWebApplication = pivotalWebServices.AddDeploymentNode("www.structurizr.com",
                    "An open source Java EE web server.", "Apache Tomcat")
                .Add(webApplication);
            var liveDatabaseInstance = amazonWebServices
                .AddDeploymentNode("Amazon RDS", "Database as a Service provider.", "MySQL")
                .Add(database);

            // add health checks to the container instances, which return a simple HTTP 200 to say everything is okay
            liveWebApplication.AddHealthCheck("Web Application is running", "https://www.structurizr.com/health");
            liveDatabaseInstance.AddHealthCheck("Database is accessible from Web Application",
                "https://www.structurizr.com/health/database");

            // the pass/fail status from the health checks is used to supplement any deployment views that include the container instances that have health checks defined
            var deploymentView = views.CreateDeploymentView(structurizr, "Deployment",
                "A deployment diagram showing the live environment.");
            deploymentView.Environment = "Live";
            deploymentView.AddAllDeploymentNodes();

            views.Configuration.Styles.Add(new ElementStyle(Tags.Element) {Color = "#ffffff"});
            views.Configuration.Styles.Add(new ElementStyle(DatabaseTag) {Shape = Shape.Cylinder});

            var structurizrClient = new StructurizrClient(ApiKey, ApiSecret);
            WorkspaceUtils.PrintWorkspaceAsJson(workspace);
            Console.ReadKey();
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }
    }
}