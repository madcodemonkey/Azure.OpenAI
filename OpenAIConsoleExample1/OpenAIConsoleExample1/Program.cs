using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenAIConsoleExample1;

// Update appsettings.json file with your settings!!!!
// Update appsettings.json file with your settings!!!!
// Update appsettings.json file with your settings!!!!
// the...........
// Try out different examples, by commenting and uncommenting workers in the RegisterDependencies method!!!!!!
// Try out different examples, by commenting and uncommenting workers in the RegisterDependencies method!!!!!!
// Try out different examples, by commenting and uncommenting workers in the RegisterDependencies method!!!!!!

// Based on code from here and the Azure AI Studio
// https://learn.microsoft.com/en-us/dotnet/api/overview/azure/ai.openai-readme?view=azure-dotnet-preview

IConfiguration config = SetupConfiguration();
IServiceProvider serviceProvider = RegisterDependencies(config);
ILogger<Program> logger = serviceProvider.GetRequiredService<ILogger<Program>>();

try
{
    var client = CreateOpenAiClient(config);

    string deploymentOrModelName = config["DeploymentOrModelName"];
    if (string.IsNullOrWhiteSpace(deploymentOrModelName) || deploymentOrModelName.StartsWith("---"))
        throw new ArgumentException("You must specify the name of the deployment.  You can get this from the Azure AI Studio!");
   

    var worker = serviceProvider.GetRequiredService<IConsoleAppWorker>();
    await worker.DoWorkAsync(client, deploymentOrModelName);

}
catch (Exception ex)
{
    logger.LogError(ex, "Something went wrong!");
}




static IServiceProvider RegisterDependencies(IConfiguration configuration)
{
    // ServiceCollection:  Requires Microsoft.Extensions.DependencyInjection
    var collection = new ServiceCollection();
    collection.AddSingleton(configuration);


    collection.AddLogging(configure => configure.AddConsole());

    // Comment out one and uncomment another to try different examples!!!!!!!!!!!!!!!!
    // Comment out one and uncomment another to try different examples
    // Comment out one and uncomment another to try different examples!!!!!!!!!!!!!!!!
    collection.AddTransient<IConsoleAppWorker, ExampleChatWithoutStream1>();
    //collection.AddTransient<IConsoleAppWorker, ExampleChatWithStream1>();
    //collection.AddTransient<IConsoleAppWorker, ExampleRegStyle1>();
    //collection.AddTransient<IConsoleAppWorker, ExampleRegStyle2>();

    var serviceProvider = collection.BuildServiceProvider();

    return serviceProvider;
}


static IConfiguration SetupConfiguration()
{
    // AddJsonFile requires:    Microsoft.Extensions.Configuration.Json NuGet package
    // AddUserSecrets requires: Microsoft.Extensions.Configuration.UserSecrets NuGet package
    // IConfiguration requires: Microsoft.Extensions.Configuration NuGet package (pulled in by previous NuGet)
    // https://stackoverflow.com/a/46437144/97803
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddUserSecrets<Program>();  // this is optional if you don't plan on using secrets

    IConfiguration config = builder.Build();

    return config;
}

OpenAIClient CreateOpenAiClient(IConfiguration configuration)
{
    // Note: The Azure OpenAI client library for .NET is in preview.
    // Install the .NET library via NuGet: dotnet add package Azure.AI.OpenAI --version 1.0.0-beta.5 
    var endPoint = configuration["OpenApiEndpoint"];
    if (string.IsNullOrWhiteSpace(endPoint) || endPoint.StartsWith("---"))
        throw new ArgumentException("You must specify a URL for the open ai client!  Please obtain it from the Azure Portal!");
    var theKey = configuration["OpenApiKey"];
    if (string.IsNullOrWhiteSpace(theKey) || theKey.StartsWith("---"))
        throw new ArgumentException("You must specify a key for the open ai client!  Please obtain it from the Azure Portal!");

    OpenAIClient openAiClient = new OpenAIClient(new Uri(endPoint), new AzureKeyCredential(theKey));
    return openAiClient;
}