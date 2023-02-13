using System.Linq;
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

            int textLength = content.Length;
            double duration = TextShowUpTimes.FirstOrDefault(t => t.TextLength >= textLength).Duration;
            DelayedHelper.DoDelay(duration, Close);
        }
        #endregion

        #region Delay Counters
        private (int TextLength, double Duration)[] TextShowUpTimes = new (int TextLength, double Duration)[]
        {
            (30, 3.5),
            (60, 5),
            (90, 6.5),
            (120, 8),
            (150, 9),
            (190, 11),
            (220, 12.5),
            (250, 15),
            (300, 60),
            (500, 120), // Roughly 100 words in English
            (2000, 450), // Roughly 400 words in English
            (int.MaxValue, 15)
        };
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
