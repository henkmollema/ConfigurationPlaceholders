# Configuration Placeholders
Placeholder support for the ASP.NET Core Configuration framework.

## Installation
Add a reference to the `Microsoft.Extensions.Configuration.Placeholders` NuGet package.

## Usage
Define placeholdes in your settings file:

```json
{
  "FooService": {
    "Endpoint": "http://example.com/api/"
  },

  "BarService": {
    "Endpoint": "[FooService:Endpoint]"
  },

  "BazService": {
    "Endpoint": "[FooService:Endpoint]"
  }
}
```

Build your configuration object and call `ReplacePlaceholders`:

```cs
public Startup(IHostingEnvironment env)
{
    Configuration = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json")
        .Build()
        .ReplacePlaceholders();
}

private IConfiguration Configuration { get; }
```

Use the configuration values:
```cs
var endpoint = Configuration["BarService:Endpoint"];
// "http://example.com/api/"
```