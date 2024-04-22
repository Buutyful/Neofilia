using Neofilia.Server;
using Neofilia.Server.Components;
using Neofilia.Server.Data;
using Neofilia.Server.Services.Quiz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//Add ef identity
builder.Services.AddIdentityServices(builder.Configuration);

//Add context repos
builder.Services.AddPersistence();

builder.Services.AddSignalR();

//Add background services
builder.Services.AddHostedService<ChallengeManager>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

//Add hub endpoint
app.MapHub<QuizHub>(QuizHub.HubUrl);

//Set up methods
await QuizHub.CreateSignalRGroups(app.Services);

app.Run();

