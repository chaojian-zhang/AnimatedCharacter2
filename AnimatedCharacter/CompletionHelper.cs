using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace AnimatedCharacter
{
    public static class CompletionHelpers
    {
        private static OpenAIService OpenAiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = OpenAIKey.Key
        });
        public static async Task<string> Complete(string input, string model = null, int maxTokens = 400, float temperature = 0.5f)
        {            
            var completionResult = await OpenAiService.Completions.CreateCompletion(new CompletionCreateRequest()
            {
                Prompt = input,
                Model = model ?? Models.TextDavinciV3,
                MaxTokens = maxTokens,
                Temperature = temperature
            });

            if (completionResult.Successful)
            {
                string reply = completionResult.Choices.FirstOrDefault()?.Text;
                if (!string.IsNullOrEmpty(reply))
                    return reply.Trim();
                return null;
            }
            else
            {
                if (completionResult.Error == null)
                    throw new Exception("Unknown Error");
                throw new Exception($"{completionResult.Error.Code}: {completionResult.Error.Message}");
            }
        }
    }
}
