using BlazorAPITest.Components;
using ClassFindrDataAccessLibrary;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBlazorBootstrap();  // Add bootstrap library to project

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();     // Add database connection to the program
builder.Services.AddTransient<IUserData, UserData>();

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
