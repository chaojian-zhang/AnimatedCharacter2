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
            (30, 3),
            (60, 5),
            (90, 6.5),
            (120, 7.5),
            (150, 8.5),
            (190, 9),
            (220, 10),
            (250, 12),
            (300, 15),
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
