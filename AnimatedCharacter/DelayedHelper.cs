using System;
using System.Windows.Threading;

namespace AnimatedCharacter
{
    internal static class DelayedHelper
    {
        public static void DoDelay(double seconds, Action action)
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(seconds) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {
                timer.Stop();
                action?.Invoke();
            };
        }
    }
}
