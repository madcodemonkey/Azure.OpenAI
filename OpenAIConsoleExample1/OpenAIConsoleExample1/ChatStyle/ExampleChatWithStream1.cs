using System.Text;
using Azure;
using Azure.AI.OpenAI;

namespace OpenAIConsoleExample1;

public class ExampleChatWithStream1 : IConsoleAppWorker
{
    public async Task DoWorkAsync(OpenAIClient client, string deploymentOrModelName)
    {
        Response<StreamingChatCompletions> response = await client.GetChatCompletionsStreamingAsync(
            deploymentOrModelName: deploymentOrModelName,
            new ChatCompletionsOptions()
            {
                Messages =
                {
                    new ChatMessage(ChatRole.System, @"You are an AI assistant that helps people find information."),
                    new ChatMessage(ChatRole.User, @"How many moons does earth have?"),
                    new ChatMessage(ChatRole.Assistant, @"Earth has one moon."),
                    new ChatMessage(ChatRole.User, @"How many moons does Mars have?"),
                    new ChatMessage(ChatRole.Assistant, @"Mars has two moons, Phobos and Deimos."),
                    new ChatMessage(ChatRole.User, @"What's the distance between earth and mars?"),
                    new ChatMessage(ChatRole.Assistant, @"The distance between Earth and Mars varies depending on their positions in their respective orbits around the sun. At their closest approach, which occurs about every 26 months, the distance between Earth and Mars is about 33.9 million miles (54.6 million kilometers). At their farthest distance, the distance between the two planets can be as much as 249 million miles (401 million kilometers)."),
                    new ChatMessage(ChatRole.User, @"Is Pluto a planet?"),
                    // new ChatMessage(ChatRole.User, @"When did the last apollo mission launch?"),
                    // new ChatMessage(ChatRole.Assistant, @"The last Apollo mission to launch was Apollo 17, which launched on December 7, 1972."),
                },
                Temperature = (float)0.7,
                MaxTokens = 800,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            });

        using StreamingChatCompletions completions = response.Value;

        IAsyncEnumerable<StreamingChatChoice>? choices = completions.GetChoicesStreaming();
        
        await foreach (StreamingChatChoice oneChoice in choices)
        {
            StringBuilder sb = new StringBuilder();
            await foreach (ChatMessage? chatMessage in oneChoice.GetMessageStreaming())
            {
                sb.Append(chatMessage.Content);
            }

            Console.WriteLine($"Response: {sb}");
        }
    }
}