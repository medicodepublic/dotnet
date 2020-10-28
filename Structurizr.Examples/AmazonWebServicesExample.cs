using Structurizr.Api;

namespace Structurizr.Examples
{
    /// <summary>
    ///     An example of how to document an AWS deployment.
    ///     The live workspace is available to view at https://structurizr.com/share/54915
    /// </summary>
    internal class AmazonWebServicesExample
    {
        private const long WorkspaceId = 54915;
        private const string ApiKey = "key";
        private const string ApiSecret = "secret";

        private const string SpringBootTag = "Spring Boot Application";
        private const string DatabaseTag = "Database";

        private static void Main()
        {
            var workspace = new Workspace("Amazon Web Services Example", "An example AWS deployment architecture.");
            var model = workspace.Model;

            var softwareSystem = model.AddSoftwareSystem("Spring PetClinic",
                "Allows employees to view and manage information regarding the veterinarians, the clients, and their pets.");
            var webApplication = softwareSystem.AddContainer("Web Application",
                "Allows employees to view and manage information regarding the veterinarians, the clients, and their pets.",
                "Java and Spring Boot");
            webApplication.AddTags(SpringBootTag);
            var database = softwareSystem.AddContainer("Database",
                "Stores information regarding the veterinarians, the clients, and their pets.",
                "Relational database schema");
            database.AddTags(DatabaseTag);

            webApplication.Uses(database, "Reads from and writes to", "JDBC/SSL");

            var amazonWebServices = model.AddDeploymentNode("Amazon Web Services");
            amazonWebServices.AddTags("Amazon Web Services - Cloud");
            var amazonRegion = amazonWebServices.AddDeploymentNode("US-East-1");
            amazonRegion.AddTags("Amazon Web Services - Region");
            var autoscalingGroup = amazonRegion.AddDeploymentNode("Autoscaling group");
            autoscalingGroup.AddTags("Amazon Web Services - Auto Scaling");
            var ec2 = autoscalingGroup.AddDeploymentNode("Amazon EC2");
            ec2.AddTags("Amazon Web Services - EC2");
            var webApplicationInstance = ec2.Add(webApplication);

            var route53 = amazonRegion.AddInfrastructureNode("Route 53");
            route53.AddTags("Amazon Web Services - Route 53");

            var elb = amazonRegion.AddInfrastructureNode("Elastic Load Balancer");
            elb.AddTags("Amazon Web Services - Elastic Load Balancing");

            route53.Uses(elb, "Forwards requests to", "HTTPS");
            elb.Uses(webApplicationInstance, "Forwards requests to", "HTTPS");

            var rds = amazonRegion.AddDeploymentNode("Amazon RDS");
            rds.AddTags("Amazon Web Services - RDS");
            var mySql = rds.AddDeploymentNode("MySQL");
            mySql.AddTags("Amazon Web Services - RDS_MySQL_instance");
            var databaseInstance = mySql.Add(database);

            var views = workspace.Views;
            var deploymentView = views.CreateDeploymentView(softwareSystem, "AmazonWebServicesDeployment",
                "An example deployment diagram.");
            deploymentView.AddAllDeploymentNodes();

            deploymentView.AddAnimation(route53);
            deploymentView.AddAnimation(elb);
            deploymentView.AddAnimation(webApplicationInstance);
            deploymentView.AddAnimation(databaseInstance);

            var styles = views.Configuration.Styles;
            styles.Add(new ElementStyle(SpringBootTag) {Shape = Shape.RoundedBox, Background = "#ffffff"});
            styles.Add(new ElementStyle(DatabaseTag) {Shape = Shape.Cylinder, Background = "#ffffff"});
            styles.Add(new ElementStyle(Tags.InfrastructureNode) {Shape = Shape.RoundedBox, Background = "#ffffff"});

            views.Configuration.Theme =
                "https://raw.githubusercontent.com/structurizr/themes/master/amazon-web-services/theme.json";

            var structurizrClient = new StructurizrClient(ApiKey, ApiSecret);
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }
    }
}