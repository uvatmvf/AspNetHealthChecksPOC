using AspNetHealthChecksPOC;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHealthChecks()
    //.AddTypeActivatedCheck<DemoHealthCheck>("demo_health_check",
    //    failureStatus: HealthStatus.Degraded,
    //    tags: new[] { "Check1" },
    //    args: new object[] { "Foo", 5 })
    .AddCheck<DemoHealthCheck>("Bar",
        failureStatus: HealthStatus.Degraded,
        tags: new[] { "Check2" });
builder.Services.Configure<HealthCheckPublisherOptions>(options =>
{
    options.Delay = TimeSpan.FromSeconds(2);
});
builder.Services.AddSingleton<IHealthCheckPublisher, ReadinessPublisher>();

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

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health");
});

app.MapRazorPages();

app.Run();
