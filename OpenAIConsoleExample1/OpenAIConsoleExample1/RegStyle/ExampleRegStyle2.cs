using Azure;
using Azure.AI.OpenAI;

namespace OpenAIConsoleExample1;


// https://learn.microsoft.com/en-us/azure/cognitive-services/openai/how-to/completions
// If you're having trouble getting the API to perform as expected, follow this checklist:
// 1. Is it clear what the intended generation should be?
// 2. Are there enough examples?
// 3. Did you check your examples for mistakes? (The API won't tell you directly)
// 4. Are you using temp and top_p correctly?
public class ExampleRegStyle2 : IConsoleAppWorker
{
    public async Task DoWorkAsync(OpenAIClient client, string deploymentOrModelName)
    {

        var completionsOptions = new CompletionsOptions()
        {
            Temperature = (float)0.7,
            MaxTokens = 800,
            Prompts = { "This is a tweet sentiment classifier\n\nTweet: \"I loved the new Batman movie!\"\nSentiment: Positive\n\nTweet: \"I hate it when my phone battery dies.\" \nSentiment: Negative\n\nTweet: \"My day has been 👍\"\nSentiment: Positive\n\nTweet: \"This is the link to the article\"\nSentiment: Neutral\n\nTweet: \"This new music video blew my mind\"\nSentiment:" },
            NucleusSamplingFactor = (float)0.95,
        };

        // TODO: This does not seem to work the same way...so each prompt must be it's own question with it's own expected response??
        //var completionsOptions = new CompletionsOptions()
        //{
        //    Temperature = (float)0.7,
        //    MaxTokens = 800,
        //    NucleusSamplingFactor = (float)0.95,
        //};
        //completionsOptions.Prompts.Add("This is a tweet sentiment classifier");
        //completionsOptions.Prompts.Add("Tweet: \"I loved the new Batman movie!\" \nSentiment: Positive");
        //completionsOptions.Prompts.Add("Tweet: \"I hate it when my phone battery dies.\" \nSentiment: Negative");
        //completionsOptions.Prompts.Add("Tweet: \"My day has been 👍\" \nSentiment: Positive");
        //completionsOptions.Prompts.Add("Tweet: \"This is the link to the article\" \nSentiment: Neutral");
        //completionsOptions.Prompts.Add("Tweet: \"This new music video blew my mind\" \nSentiment");

        Console.Write($"Inputs:\n");
        foreach (var item in completionsOptions.Prompts)
        {
            Console.WriteLine(item);
        }
        
        
        Response<Completions> completionsResponse =
            await client.GetCompletionsAsync(deploymentOrModelName,
                completionsOptions);



        //Response<Completions> completionsResponse =
        //    await client.GetCompletionsAsync(deploymentOrModelName, prompt);

        Console.WriteLine($"Open API responded with {completionsResponse.Value.Choices.Count} choices");

        int choiceCounter = 1;
        foreach (Choice choice in completionsResponse.Value.Choices)
        {
            Console.WriteLine($"Choice: {choiceCounter++}");
            Console.WriteLine($"Response: {choice.Text}");
        }
        
    }
}