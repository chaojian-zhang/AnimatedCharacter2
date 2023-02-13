using AnimatedCharacter.PopUps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AnimatedCharacter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Default Context
        /// <summary>
        /// Provides basic self-identify and current world/time relevancy
        /// </summary>
        public static string DefaultContent = $"""
            Speaker: What's your name?
            Reply: My name is Miku.
            Speaker: What do you like?
            Reply: I like hiking.
            Speaker: What's current date?
            Reply: {DateTime.Now.ToLongDateString()}
            Speaker: What day is it today?
            Reply: {DateTime.Now.DayOfWeek}
            """;
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            InitializeAnimations();

            DelayedHelper.DoDelay(2, () => DialoguePopUp.Show("Hi, how are you?\n(Reight-click on me to make a reply.)", new System.Numerics.Vector2((float)Left, (float)Top)));
        }
        public void InitializeAnimations()
        {
            // Create animation states
            AnimationStates = new Dictionary<string, BitmapSource[]>();
            StateTransitions = new Dictionary<string, List<string>>();
            Rnd = new Random(Guid.NewGuid().GetHashCode());

            // Load animation assets
            var binaryImage = EmbeddedResourceHelper.ReadBinaryResource(@"AnimatedCharacter.AnimationAssets.MikuPopSpriteTrans.png");
            var bitmap = new System.Drawing.Bitmap(binaryImage);
            AnimationStates["Default"] = LoadCompositeAnimation(bitmap, 59, 64,
                new List<Point>()
                {
                    new Point(0,0),
                    new Point(59,0),
                    new Point(118,0),
                    new Point(177,0),
                    new Point(236,0),
                    new Point(295,0),
                    new Point(354,0),
                    new Point(413,0),
                    new Point(472,0),
                    new Point(531,0),
                    new Point(590,0),
                    new Point(649,0),
                    new Point(708,0),
                    new Point(767,0),
                    new Point(826,0),
                    new Point(885,0),
                    new Point(944,0),
                    new Point(1003,0),
                    new Point(1062,0),
                    new Point(1121,0)
                });

            // Define state transitions - In the future those can be loaded as a seperate step e.g. from a file
            StateTransitions["Default"] = new List<string>() { "Default" };

            // Initialize animation state
            CurrentState = "Default";

            // Setup Playback
            FrameTimer.Tick += OnFrame;
            FrameTimer.Interval = TimeSpan.FromSeconds(1.0 / 24.0); // Fixed framerate for now
            FrameTimer.Start();
        }
        #endregion

        #region Properties
        public List<string> ConversationMessages = new();
        #endregion

        #region Helpers
        private BitmapSource[] LoadCompositeAnimation(System.Drawing.Bitmap imageFile, int spriteWidth, int spriteHeight, List<Point> locations)
        {
            return locations.Select(l => CropImage(imageFile, (int)l.X, (int)l.Y, spriteWidth, spriteHeight)).ToArray();
        }
        private BitmapSource[] LoadSequenceAnimation(System.Drawing.Bitmap imageFile, int numRows, int numCols)
        {
            int frames = (imageFile.Width / numCols) * (imageFile.Height / numRows);  // Assume sqaure usage
            BitmapSource[] sequence = new BitmapSource[frames];

            int spriteWidth = imageFile.Width / numCols;
            int spriteHeight = imageFile.Height / numRows;

            for (int col = 0; col < numCols; col++)
            {
                for (int row = 0; row < numRows; row++)
                {
                    sequence[row * numCols + col] = CropImage(imageFile, col * spriteWidth, row * spriteHeight, spriteWidth, spriteHeight);
                }
            }

            return sequence;
        }
        private BitmapSource[] LoadSequenceAnimation(string[] fileNames)
        {
            return fileNames.Select(f => new BitmapImage(new Uri(f))).ToArray();
        }

        private BitmapSource CropImage(System.Drawing.Bitmap source, int x, int y, int cropWidth, int cropHeight)
        {
            System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(x, y, cropWidth, cropHeight);

            System.Drawing.Bitmap target = new System.Drawing.Bitmap(cropRect.Width, cropRect.Height);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(target))
            {
                g.DrawImage(source, new System.Drawing.Rectangle(0, 0, target.Width, target.Height), cropRect, System.Drawing.GraphicsUnit.Pixel);
            }
            BitmapSource cropped = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(target.GetHbitmap(), IntPtr.Zero,
                Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(target.Width, target.Height));

            return cropped;
        }
        private string GetNextState()
        {
            // Randomly choose next state
            var nextStates = StateTransitions[CurrentState];
            return nextStates[Rnd.Next(nextStates.Count)];
        }
        #endregion

        #region Event Management
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private async void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                string message = PromptDialog.Prompt("Enter what you want to say: ", "Say Something😆");
                await HandleInputMessage(message);
            }
        }
        private void OnFrame(object sender, EventArgs e)
        {
            SpriteImage.Source = CurrentAnimation[CurrentFrameCount];
            CurrentFrameCount++;

            // State transition
            if (CurrentFrameCount >= CurrentAnimation.Length)
            {
                CurrentState = GetNextState();
                CurrentFrameCount = 0;
            }
        }
        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            CloseButton.Visibility = Visibility.Visible;
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            CloseButton.Visibility = Visibility.Hidden;
        }
        #endregion

        #region Animation State Management
        Dictionary<string, BitmapSource[]> AnimationStates { get; set; }
        System.Windows.Threading.DispatcherTimer FrameTimer = new System.Windows.Threading.DispatcherTimer();
        int CurrentFrameCount = 0;
        string CurrentState = null;
        BitmapSource[] CurrentAnimation { get { return AnimationStates[CurrentState]; } }
        Dictionary<string, List<string>> StateTransitions { get; set; }
        Random Rnd;
        #endregion

        #region Routines
        private async Task HandleInputMessage(string message)
        {
            if (message != null)
            {
                string[] contexts = new string[] { OpenAIKey.Prelude, DefaultContent.Trim(), OpenAIKey.AdditionalContext.Trim() }
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Concat(ConversationMessages.TakeLast(System.Math.Min(ConversationMessages.Count, 10))) // Remark-cz: Keep input tokens amount reasonable by only keep last 10 conversations in memory
                    .ToArray();
                string newMessage = $"Speaker: {message}";
                string contextualInput = string.Join(Environment.NewLine, contexts) + Environment.NewLine
                    + newMessage;
                try
                {
                    string reply = await CompletionHelpers.Complete(contextualInput.Trim());
                    string displayReply = Regex.Replace(reply, @"^[Rr]eply: ", string.Empty);
                    DialoguePopUp.Show(displayReply, new System.Numerics.Vector2((float)Left, (float)Top));

                    ConversationMessages.Add(newMessage);
                    ConversationMessages.Add($"Reply: {displayReply}");
                }
                catch (Exception e)
                {
                    DialoguePopUp.Show($"An error occured when trying to make a reply: {e.Message}", new System.Numerics.Vector2((float)Left, (float)Top));
                }
            }
        }
        #endregion
    }
}
