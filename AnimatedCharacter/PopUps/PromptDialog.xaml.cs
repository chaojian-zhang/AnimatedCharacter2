using AnimatedCharacter.Windows;
using System.Windows;

namespace AnimatedCharacter.PopUps
{
    public partial class PromptDialog : Window
    {
        public enum InputType
        {
            Text,
            Password
        }

        #region Construction
        public PromptDialog(string question, string title, string defaultValue = "", InputType inputType = InputType.Text, string furtherExplanation = null, string alternativeOkButtonLabel = null, string alternativeCancelButtonLabel = null)
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(PromptDialogLoaded);
            QuestionText.Text = question;
            Title = title;
            GeneralTextResponse.Text = defaultValue;
            HistoryButton.Visibility = (App.Current.MainWindow as MainWindow).ConversationMessages.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

            _InputType = inputType;
            if (_InputType == InputType.Password)
                GeneralTextResponse.Visibility = Visibility.Collapsed;
            else
                PasswordTextResponse.Visibility = Visibility.Collapsed;

            if (!string.IsNullOrEmpty(furtherExplanation))
            {
                ExplanationText.Text = furtherExplanation;
                ExplanationText.Visibility = Visibility.Visible;
            }

            if (!string.IsNullOrEmpty(alternativeOkButtonLabel))
                OkButton.Content = "_" + alternativeOkButtonLabel;
            if (!string.IsNullOrEmpty(alternativeCancelButtonLabel))
                CancelButton.Content = "_" + alternativeCancelButtonLabel;
        }
        #endregion

        #region Properties
        private readonly InputType _InputType = InputType.Text;
        public string ResponseText
        {
            get
            {
                if (_InputType == InputType.Password)
                    return PasswordTextResponse.Password;
                else
                    return GeneralTextResponse.Text;
            }
        }
        #endregion

        #region Events
        void PromptDialogLoaded(object sender, RoutedEventArgs e)
        {
            if (_InputType == InputType.Password)
                PasswordTextResponse.Focus();
            else
                GeneralTextResponse.Focus();
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelBUtton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void PreviewTextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new HistoryWindow().Show();
            Close();
        }
        #endregion

        #region Interface Method
        public static string Prompt(string question, string title, string defaultValue = "", InputType inputType = InputType.Text, string furtherExplanation = null, string alternativeOkButtonLabel = null, string alternativeCancelButtonLabel = null)
        {
            PromptDialog inst = new(question, title, defaultValue, inputType, furtherExplanation, alternativeOkButtonLabel, alternativeCancelButtonLabel)
            {
                Owner = App.Current.MainWindow
            };
            inst.ShowDialog();
            if (inst.DialogResult == true)
                return inst.ResponseText;
            return null;
        }
        #endregion
    }
}
