# Configuration Placeholders
Placeholder support for the ASP.NET Core Configuration framework.

## Installation
Add a reference to the `Microsoft.Extensions.Configuration.Placeholders` NuGet package.

## Usage
Define placeholdes in your settings file. You can use multiple placeholders inside static strings.

```json
{
  "FooService": {
    "Endpoint": "http://example.com/api/",
    "Resource": "resources/v2/"
  },

  "BarService": {
    "Endpoint": "[FooService:Endpoint]"
  },

  "BazService": {
    "Endpoint": "http://example2.com/api/[ASPNETCORE_ENVIRONMENT]/[FooService:Resource]"
  }
}
```

Build your configuration object and call `ReplacePlaceholders()`:

```cs
public Startup(IHostingEnvironment env)
{
    Configuration = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build()
        .ReplacePlaceholders();
}

private IConfiguration Configuration { get; }
```

Use the configuration values:
```cs
var endpoint = Configuration["BazService:Endpoint"];
// http://example2.com/api/Development/resources/v2/
```

## Samples
Check out the [sample web app](https://github.com/henkmollema/ConfigurationPlaceholders/tree/master/samples/SampleWebApp) for more details.