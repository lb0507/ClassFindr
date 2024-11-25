using BlazorAPITest.Components;
using ClassFindrDataAccessLibrary;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBlazorBootstrap();  // Add bootstrap library to project

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddTransient<IMapBoxConfig, MapBoxConfig>();   // Add the mapbox configuration data to the project
builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();     // Add database connection to the program
builder.Services.AddSingleton<IBuildingData, BuildingData>();   // Add the building API services
builder.Services.AddTransient<IClassData, ClassData>();   // Add class API services
builder.Services.AddSingleton<IUserData, UserData>();   // Add the singleton for the user login info
builder.Services.AddSingleton<IScheduleData, ScheduleData>();   // Add the user's schedule data


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.Run();
