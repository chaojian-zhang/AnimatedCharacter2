using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using System;
using System.Linq;
using System.Windows;

namespace PromptBox
{
    public partial class MainWindow : Window
    {
        public OpenAIService OpenAiService { get; }
        public MainWindow()
        {
            OpenAiService = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = OpenAIKey.Key
            });

            InitializeComponent();
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string input = Input.Text;
            Result.Text = "Loading...";

            var completionResult = await OpenAiService.Completions.CreateCompletion(new CompletionCreateRequest()
            {
                Prompt = input,
                Model = Models.TextDavinciV3,
                MaxTokens = 2049, /*MAX*/
                Temperature = 1
            });

            if (completionResult.Successful)
            {
                string reply = completionResult.Choices.FirstOrDefault()?.Text;
                if (!string.IsNullOrEmpty(reply))
                    Result.Text = reply.Trim();
                HistoryManager.AddHistory(new History(input, reply, DateTime.Now));
            }
            else
            {
                if (completionResult.Error == null)
                    Result.Text = "Unknown Error";
                Result.Text = $"{completionResult.Error.Code}: {completionResult.Error.Message}";
            }
        }

        private void ViewHistoryFileMenu_Click(object sender, RoutedEventArgs e)
        {
            new HistoryWindow().Show();
        }
    }
}
