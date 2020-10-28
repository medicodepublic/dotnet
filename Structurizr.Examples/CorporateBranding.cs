using System.IO;
using Structurizr.Api;
using Structurizr.Documentation;
using Structurizr.Util;

namespace Structurizr.Examples
{
    /// <summary>
    ///     This is a simple example that illustrates the corporate branding features of Structurizr, including:
    ///     - A logo, which is included in diagrams and documentation.
    ///     You can see the live workspace at https://structurizr.com/share/35031
    /// </summary>
    public class CorporateBranding
    {
        private const long WorkspaceId = 35031;
        private const string ApiKey = "key";
        private const string ApiSecret = "secret";

        private static void Main()
        {
            var workspace = new Workspace("Corporate Branding", "This is a model of my software system.");
            var model = workspace.Model;

            var user = model.AddPerson("User", "A user of my software system.");
            var softwareSystem = model.AddSoftwareSystem("Software System", "My software system.");
            user.Uses(softwareSystem, "Uses");

            var views = workspace.Views;
            var contextView = views.CreateSystemContextView(softwareSystem, "SystemContext",
                "An example of a System Context diagram.");
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            var styles = views.Configuration.Styles;
            styles.Add(new ElementStyle(Tags.Person) {Shape = Shape.Person});

            var template = new StructurizrDocumentationTemplate(workspace);
            template.AddContextSection(softwareSystem, Format.Markdown,
                "Here is some context about the software system...\n\n![](embed:SystemContext)");

            var branding = views.Configuration.Branding;
            branding.Logo = ImageUtils.GetImageAsDataUri(new FileInfo("structurizr-logo.png"));

            var structurizrClient = new StructurizrClient(ApiKey, ApiSecret);
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }
    }
}