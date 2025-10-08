using Bh.AppWeb.Components;
using Bh.AppWeb.Configuracion;
using Bh.AppWeb.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddRazorComponents();

var awsConfig = new AwsConfig();
builder.Configuration.GetSection("AWS").Bind(awsConfig);

builder.Services.AddTransient(s => new AmazonSesEmailSender(awsConfig.GetAwsCredentials()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

//app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapRazorComponents<App>();

app.Run();
