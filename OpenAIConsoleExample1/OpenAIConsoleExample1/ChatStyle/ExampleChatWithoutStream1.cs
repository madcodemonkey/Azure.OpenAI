using Azure.AI.OpenAI;
using Azure;

namespace OpenAIConsoleExample1;

public class ExampleChatWithoutStream1 : IConsoleAppWorker
{
    public async Task DoWorkAsync(OpenAIClient client, string deploymentOrModelName)
    {
        Response<ChatCompletions> responseWithoutStream = await client.GetChatCompletionsAsync(
            deploymentOrModelName,
            new ChatCompletionsOptions()
            {
                Messages =
                {
                    new ChatMessage(ChatRole.System, @"You are an AI assistant that helps people find information."),
                    new ChatMessage(ChatRole.User, @"How many moons does earth have?"),
                    new ChatMessage(ChatRole.Assistant, @"Earth has one moon.  Have you seen it at night up in the sky?"),
                    new ChatMessage(ChatRole.User, @"How many moons does Mars have?"),
                    new ChatMessage(ChatRole.Assistant, @"Mars has two moons, Phobos and Deimos.  They are hard to see except with special tools."),
                    new ChatMessage(ChatRole.User, @"What's the distance between earth and mars?"),
                    new ChatMessage(ChatRole.Assistant, @"It's a long way. The distance between Earth and Mars varies depending on their positions in their respective orbits around the sun. At their closest approach, which occurs about every 26 months, the distance between Earth and Mars is about 33.9 million miles (54.6 million kilometers). At their farthest distance, the distance between the two planets can be as much as 249 million miles (401 million kilometers)."),
                    new ChatMessage(ChatRole.User, @"How old were the two men who first stepped foot on the moon?"),
                },
                Temperature = (float)0.7,
                MaxTokens = 800,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            });

        ChatCompletions completions = responseWithoutStream.Value;

        string completion = completions.Choices[0].Message.Content;
        Console.WriteLine($"Chatbot: {completion}");

    }
}