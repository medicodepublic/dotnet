using System.IO;
using Structurizr.Api;
using Structurizr.Documentation;

namespace Structurizr.Examples
{
    /// <summary>
    ///     This is a simple (incomplete) example C4 model based upon the financial risk system
    ///     architecture kata, which can be found at http://bit.ly/sa4d-risksystem
    ///     You can see the workspace online at https://structurizr.com/public/31
    /// </summary>
    public class FinancialRiskSystem
    {
        private const long WorkspaceId = 31;
        private const string ApiKey = "key";
        private const string ApiSecret = "secret";

        private const string AlertTag = "Alert";

        private static void Main()
        {
            var workspace = new Workspace("Financial Risk System",
                "This is a simple (incomplete) example C4 model based upon the financial risk system architecture kata, which can be found at http://bit.ly/sa4d-risksystem");
            var model = workspace.Model;

            var financialRiskSystem = model.AddSoftwareSystem("Financial Risk System",
                "Calculates the bank's exposure to risk for product X.");

            var businessUser = model.AddPerson("Business User", "A regular business user.");
            businessUser.Uses(financialRiskSystem, "Views reports using");

            var configurationUser = model.AddPerson("Configuration User",
                "A regular business user who can also configure the parameters used in the risk calculations.");
            configurationUser.Uses(financialRiskSystem, "Configures parameters using");

            var tradeDataSystem =
                model.AddSoftwareSystem("Trade Data System", "The system of record for trades of type X.");
            financialRiskSystem.Uses(tradeDataSystem, "Gets trade data from");

            var referenceDataSystem = model.AddSoftwareSystem("Reference Data System",
                "Manages reference data for all counterparties the bank interacts with.");
            financialRiskSystem.Uses(referenceDataSystem, "Gets counterparty data from");

            var referenceDataSystemV2 = model.AddSoftwareSystem("Reference Data System v2.0",
                "Manages reference data for all counterparties the bank interacts with.");
            referenceDataSystemV2.AddTags("Future State");
            financialRiskSystem.Uses(referenceDataSystemV2, "Gets counterparty data from").AddTags("Future State");

            var emailSystem = model.AddSoftwareSystem("E-mail system", "The bank's Microsoft Exchange system.");
            financialRiskSystem.Uses(emailSystem, "Sends a notification that a report is ready to");
            emailSystem.Delivers(businessUser, "Sends a notification that a report is ready to", "E-mail message",
                InteractionStyle.Asynchronous);

            var centralMonitoringService = model.AddSoftwareSystem("Central Monitoring Service",
                "The bank's central monitoring and alerting dashboard.");
            financialRiskSystem.Uses(centralMonitoringService, "Sends critical failure alerts to", "SNMP",
                InteractionStyle.Asynchronous).AddTags(AlertTag);

            var activeDirectory = model.AddSoftwareSystem("Active Directory",
                "The bank's authentication and authorisation system.");
            financialRiskSystem.Uses(activeDirectory, "Uses for user authentication and authorisation");

            var views = workspace.Views;
            var contextView = views.CreateSystemContextView(financialRiskSystem, "Context",
                "An example System Context diagram for the Financial Risk System architecture kata.");
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            var styles = views.Configuration.Styles;
            financialRiskSystem.AddTags("Risk System");

            styles.Add(new ElementStyle(Tags.Element) {Color = "#ffffff", FontSize = 34});
            styles.Add(new ElementStyle("Risk System") {Background = "#550000", Color = "#ffffff"});
            styles.Add(new ElementStyle(Tags.SoftwareSystem)
                {Width = 650, Height = 400, Background = "#801515", Shape = Shape.RoundedBox});
            styles.Add(new ElementStyle(Tags.Person) {Width = 550, Background = "#d46a6a", Shape = Shape.Person});

            styles.Add(new RelationshipStyle(Tags.Relationship)
                {Thickness = 4, Dashed = false, FontSize = 32, Width = 400});
            styles.Add(new RelationshipStyle(Tags.Synchronous) {Dashed = false});
            styles.Add(new RelationshipStyle(Tags.Asynchronous) {Dashed = true});
            styles.Add(new RelationshipStyle(AlertTag) {Color = "#ff0000"});

            styles.Add(new ElementStyle("Future State") {Opacity = 30, Border = Border.Dashed});
            styles.Add(new RelationshipStyle("Future State") {Opacity = 30, Dashed = true});

            var template = new StructurizrDocumentationTemplate(workspace);
            var documentationRoot = new DirectoryInfo("FinancialRiskSystem");
            template.AddContextSection(financialRiskSystem,
                new FileInfo(Path.Combine(documentationRoot.FullName, "context.adoc")));
            template.AddFunctionalOverviewSection(financialRiskSystem,
                new FileInfo(Path.Combine(documentationRoot.FullName, "functional-overview.md")));
            template.AddQualityAttributesSection(financialRiskSystem,
                new FileInfo(Path.Combine(documentationRoot.FullName, "quality-attributes.md")));
            template.AddImages(documentationRoot);

            var structurizrClient = new StructurizrClient(ApiKey, ApiSecret);
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }
    }
}