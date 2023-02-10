using System.Numerics;
using System.Windows;

namespace AnimatedCharacter.PopUps
{
    public partial class DialoguePopUp : Window
    {
        #region Construction
        public DialoguePopUp(string content)
        {
            InitializeComponent();
            Text.Text = content;

            DelayedHelper.DoDelay(2, Close);
        }
        #endregion

        #region Interface Method
        public static void Show(string content, Vector2 startUpLocation)
        {
            DialoguePopUp inst = new(content)
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = startUpLocation.X,
                Top = startUpLocation.Y,
                Owner = App.Current.MainWindow
            };
            inst.Show();
        }
        #endregion
    }
}
