using Azure.AI.OpenAI;

namespace OpenAIConsoleExample1;

public interface IConsoleAppWorker
{
    Task DoWorkAsync(OpenAIClient client, string deploymentOrModelName);
}