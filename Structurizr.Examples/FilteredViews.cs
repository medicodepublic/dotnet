using Structurizr.Api;

namespace Structurizr.Examples
{
    /// <summary>
    ///     An example of how to use filtered views to show "before" and "after" views of a software system.
    ///     You can see the live diagrams at https://structurizr.com/public/19911
    /// </summary>
    public class FilteredViews
    {
        private const long WorkspaceId = 19911;
        private const string ApiKey = "";
        private const string ApiSecret = "";

        private const string CurrentState = "Current State";
        private const string FutureState = "Future State";

        private static void Main()
        {
            var workspace = new Workspace("Filtered Views", "An example of using filtered views.");
            var model = workspace.Model;

            var user = model.AddPerson("User", "A description of the user.");
            var softwareSystemA = model.AddSoftwareSystem("Software System A", "A description of software system A.");
            var softwareSystemB = model.AddSoftwareSystem("Software System B", "A description of software system B.");
            softwareSystemB.AddTags(FutureState);

            user.Uses(softwareSystemA, "Uses for tasks 1 and 2").AddTags(CurrentState);
            user.Uses(softwareSystemA, "Uses for task 1").AddTags(FutureState);
            user.Uses(softwareSystemB, "Uses for task 2").AddTags(FutureState);

            var views = workspace.Views;
            var systemLandscapeView =
                views.CreateSystemLandscapeView("EnterpriseContext", "An example Enterprise Context diagram.");
            systemLandscapeView.AddAllElements();

            views.CreateFilteredView(systemLandscapeView, "CurrentState", "The current context.", FilterMode.Exclude,
                FutureState);
            views.CreateFilteredView(systemLandscapeView, "FutureState",
                "The future state context after Software System B is live.", FilterMode.Exclude, CurrentState);

            var styles = views.Configuration.Styles;
            styles.Add(new ElementStyle(Tags.Element) {Color = "#ffffff"});
            styles.Add(new ElementStyle(Tags.SoftwareSystem) {Background = "#91a437", Shape = Shape.RoundedBox});
            styles.Add(new ElementStyle(Tags.Person) {Background = "#6a7b15", Shape = Shape.Person});

            var structurizrClient = new StructurizrClient(ApiKey, ApiSecret);
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }
    }
}