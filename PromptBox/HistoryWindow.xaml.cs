using System.Windows;

namespace PromptBox
{
    /// <summary>
    /// Interaction logic for HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {
        public HistoryWindow()
        {
            InitializeComponent();
            HistoryText.Text = HistoryManager.GetHistory().ToMarkdown();
        }
    }
}
