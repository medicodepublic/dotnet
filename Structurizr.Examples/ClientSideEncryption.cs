using Structurizr.Api;
using Structurizr.Encryption;

namespace Structurizr.Examples
{
    /// <summary>
    ///     This is an example of how to use client-side encryption.
    ///     You can see the workspace online at https://structurizr.com/share/41
    ///     (the passphrase is "password")
    /// </summary>
    public class ClientSideEncryption
    {
        private const long WorkspaceId = 41;
        private const string ApiKey = "key";
        private const string ApiSecret = "secret";

        private static void Main()
        {
            var workspace = new Workspace("Client-side encrypted workspace",
                "This is a client-side encrypted workspace. The passphrase is 'password'.");
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
            styles.Add(new ElementStyle(Tags.SoftwareSystem) {Background = "#d34407", Color = "#ffffff"});
            styles.Add(new ElementStyle(Tags.Person) {Background = "#f86628", Color = "#ffffff", Shape = Shape.Person});

            var structurizrClient = new StructurizrClient(ApiKey, ApiSecret);
            structurizrClient.EncryptionStrategy = new AesEncryptionStrategy("password");
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }
    }
}