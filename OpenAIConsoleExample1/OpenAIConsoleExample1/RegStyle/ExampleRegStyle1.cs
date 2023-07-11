using Azure;
using Azure.AI.OpenAI;

namespace OpenAIConsoleExample1;

// https://learn.microsoft.com/en-us/azure/cognitive-services/openai/how-to/completions
public class ExampleRegStyle1 : IConsoleAppWorker
{
    public async Task DoWorkAsync(OpenAIClient client, string deploymentOrModelName)
    {
        string prompt = @$"
    You are an AI assistant that helps people find information.
     How many moons does Mars have?
";

        Console.Write($"{prompt}\n");


        Response<Completions> completionsResponse =
            await client.GetCompletionsAsync(deploymentOrModelName,
                new CompletionsOptions()
                {
                    Temperature = (float)0.7,
                    MaxTokens = 800,
                    Prompts = { prompt },
                    NucleusSamplingFactor = (float)0.95,
                });


        //Response<Completions> completionsResponse =
        //    await client.GetCompletionsAsync(deploymentOrModelName, prompt);
        
        Console.WriteLine($"Open API responded with {completionsResponse.Value.Choices.Count} choices");
        
        string completion = completionsResponse.Value.Choices[0].Text;
        Console.WriteLine($"Response: {completion}");


    }
}