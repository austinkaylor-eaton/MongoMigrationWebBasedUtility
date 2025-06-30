using Microsoft.AspNetCore.Components;

namespace OnlineMongoMigration.BlazorApp.Components.Pages;

public partial class GetEnvironmentComponent : ComponentBase
{
    [Inject]
    private IWebHostEnvironment WebHostEnvironment { get; set; }

    private string EnvironmentName => WebHostEnvironment.EnvironmentName;
}