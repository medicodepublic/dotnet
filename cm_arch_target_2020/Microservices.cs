using Structurizr.Api;

namespace Structurizr.Examples
{
    public class MicroservicesExample
    {
        private const long WorkspaceId = 60093;
        private const string ApiKey = "206d6560-3dd2-4467-bde3-38894cb1a17e";
        private const string ApiSecret = "933965ca-92e8-4c0b-ae03-7df3aebb399d ";

        private const string MicroserviceTag = "Microservice";
        private const string MessageBusTag = "RabbitMQ";
        private const string DataStoreTag = "PostgreSQL";

        private static void Main()
        {
            var workspace = new Workspace("Microservices example",
                "An example of a microservices architecture, which includes asynchronous and parallel behaviour.");
            var model = workspace.Model;

            var mySoftwareSystem = model.AddSoftwareSystem("Customer Information System", "Stores information ");
            var customer = model.AddPerson("Customer", "A customer");
            var customerApplication = mySoftwareSystem.AddContainer("Customer Application",
                "Allows customers to manage their profile.", "Angular");

            var customerService = mySoftwareSystem.AddContainer("Customer Service",
                "The point of access for customer information.", "Java and Spring Boot");
            customerService.AddTags(MicroserviceTag);
            var customerDatabase =
                mySoftwareSystem.AddContainer("Customer Database", "Stores customer information.", "Oracle 12c");
            customerDatabase.AddTags(DataStoreTag);

            var reportingService = mySoftwareSystem.AddContainer("Reporting Service",
                "Creates normalised data for reporting purposes.", "Ruby");
            reportingService.AddTags(MicroserviceTag);
            var reportingDatabase = mySoftwareSystem.AddContainer("Reporting Database",
                "Stores a normalised version of all business data for ad hoc reporting purposes.", "MySQL");
            reportingDatabase.AddTags(DataStoreTag);

            var auditService = mySoftwareSystem.AddContainer("Audit Service",
                "Provides organisation-wide auditing facilities.", "C# .NET");
            auditService.AddTags(MicroserviceTag);
            var auditStore = mySoftwareSystem.AddContainer("Audit Store",
                "Stores information about events that have happened.", "Event Store");
            auditStore.AddTags(DataStoreTag);

            var messageBus = mySoftwareSystem.AddContainer("Message Bus", "Transport for business events.", "RabbitMQ");
            messageBus.AddTags(MessageBusTag);

            customer.Uses(customerApplication, "Uses");
            customerApplication.Uses(customerService, "Updates customer information using", "JSON/HTTPS",
                InteractionStyle.Synchronous);
            customerService.Uses(messageBus, "Sends customer update events to", "", InteractionStyle.Asynchronous);
            customerService.Uses(customerDatabase, "Stores data in", "JDBC", InteractionStyle.Synchronous);
            customerService.Uses(customerApplication, "Sends events to", "WebSocket", InteractionStyle.Asynchronous);
            messageBus.Uses(reportingService, "Sends customer update events to", "", InteractionStyle.Asynchronous);
            messageBus.Uses(auditService, "Sends customer update events to", "", InteractionStyle.Asynchronous);
            reportingService.Uses(reportingDatabase, "Stores data in", "", InteractionStyle.Synchronous);
            auditService.Uses(auditStore, "Stores events in", "", InteractionStyle.Synchronous);

            var views = workspace.Views;

            var containerView = views.CreateContainerView(mySoftwareSystem, "Containers", null);
            containerView.AddAllElements();

            var dynamicView = views.CreateDynamicView(mySoftwareSystem, "CustomerUpdateEvent",
                "This diagram shows what happens when a customer updates their details.");
            dynamicView.Add(customer, customerApplication);
            dynamicView.Add(customerApplication, customerService);

            dynamicView.Add(customerService, customerDatabase);
            dynamicView.Add(customerService, messageBus);

            dynamicView.StartParallelSequence();
            dynamicView.Add(messageBus, reportingService);
            dynamicView.Add(reportingService, reportingDatabase);
            dynamicView.EndParallelSequence();

            dynamicView.StartParallelSequence();
            dynamicView.Add(messageBus, auditService);
            dynamicView.Add(auditService, auditStore);
            dynamicView.EndParallelSequence();

            dynamicView.StartParallelSequence();
            dynamicView.Add(customerService, "Confirms update to", customerApplication);
            dynamicView.EndParallelSequence();

            var styles = views.Configuration.Styles;
            styles.Add(new ElementStyle(Tags.Element) {Color = "#000000"});
            styles.Add(new ElementStyle(Tags.Person) {Background = "#ffbf00", Shape = Shape.Person});
            styles.Add(new ElementStyle(Tags.Container) {Background = "#facc2E"});
            styles.Add(new ElementStyle(MessageBusTag) {Width = 1600, Shape = Shape.Pipe});
            styles.Add(new ElementStyle(MicroserviceTag) {Shape = Shape.Hexagon});
            styles.Add(new ElementStyle(DataStoreTag) {Background = "#f5da81", Shape = Shape.Cylinder});
            styles.Add(new RelationshipStyle(Tags.Relationship) {Routing = Routing.Orthogonal});

            styles.Add(new RelationshipStyle(Tags.Asynchronous) {Dashed = true});
            styles.Add(new RelationshipStyle(Tags.Synchronous) {Dashed = false});

            var structurizrClient = new StructurizrClient(ApiKey, ApiSecret);
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }
    }
}