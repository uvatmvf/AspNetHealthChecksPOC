using AspNetHealthChecksPOC;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHealthChecks()
    .AddTypeActivatedCheck<HealthCheckWithArgs>("demo_health_check",
        failureStatus: HealthStatus.Degraded,
        tags: new[] { "Check1" },
        args: new object[] { "Foo", 5 })
    .AddCheck<DemoHealthCheck>("Bar",
        failureStatus: HealthStatus.Degraded,
        tags: new[] { "Check2" })
    .AddCheck("Example", () => HealthCheckResult.Healthy("Prove it out!"));

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
    endpoints.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
    {
        ResponseWriter = WriteResponse
    });
});

app.MapRazorPages();

app.Run();


static Task WriteResponse(HttpContext context, HealthReport result)
{
    // adapted from https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-6.0
    context.Response.ContentType = "application/json; charset=utf-8";

    var options = new JsonWriterOptions
    {
        Indented = true
    };

    using (var stream = new MemoryStream())
    {
        using (var writer = new Utf8JsonWriter(stream, options))
        {
            writer.WriteStartObject();
            writer.WriteString("status", result.Status.ToString());
            writer.WriteStartObject("results");
            foreach (var entry in result.Entries)
            {
                writer.WriteStartObject(entry.Key);
                writer.WriteString("status", entry.Value.Status.ToString());
                writer.WriteString("description", entry.Value.Description);
                writer.WriteStartObject("data");
                foreach (var item in entry.Value.Data)
                {
                    writer.WritePropertyName(item.Key);
                    JsonSerializer.Serialize(
                        writer, item.Value, item.Value?.GetType() ??
                        typeof(object));
                }
                writer.WriteEndObject();
                writer.WriteEndObject();
            }
            writer.WriteEndObject();
            writer.WriteEndObject();
        }

        var json = Encoding.UTF8.GetString(stream.ToArray());

        return context.Response.WriteAsync(json);
    }
}