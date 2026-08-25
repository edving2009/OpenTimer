using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace OpenTimer
{
    public partial class MainWindow : Window
    {
        private int remainingSeconds = 90 * 60;
        private int sessionDurationSeconds = 90 * 60;
        private readonly DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize the timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            SetReadyState();
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TimerInputBox.Text, out int minutes) || minutes <= 0)
            {
                // TODO: Add some way of saying what the error is
                // If a number is not entered, start button will not work
                return;
            }

            sessionDurationSeconds = minutes * 60;
            remainingSeconds = sessionDurationSeconds;

            UpdateTimeDisplay();

            TaskName.IsEnabled = false;
            TimerInputBox.IsEnabled = false;

            timer.Start();
            SetRunningState();
        }

        private void Pause_Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            SetPausedState();
        }

        private void Resume_Button_Click(object sender, RoutedEventArgs e)
        {
            if (remainingSeconds <= 0)
                return;

            timer.Start();
            SetRunningState();
        }

        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();

            remainingSeconds = sessionDurationSeconds;
            UpdateTimeDisplay();

            TaskName.IsEnabled = true;
            TimerInputBox.IsEnabled = true;

            SetReadyState();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            remainingSeconds--;

            if (remainingSeconds <= 0)
            {
                remainingSeconds = 0;
                UpdateTimeDisplay();

                timer.Stop();
                SetCompletedState();

                return;
            }

            UpdateTimeDisplay();
        }

        private void UpdateTimeDisplay()
        {
            TimeSpan time = TimeSpan.FromSeconds(remainingSeconds);
            int totalMinutes = (int)time.TotalMinutes;
            TimeText.Text = $"{totalMinutes:D2}:{time.Seconds:D2}";
        }

        // ---------------------------------------
        // STATES FOR ENABLING AND DISABLING ITEMS
        // ---------------------------------------

        private void SetReadyState()
        {
            StartButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
            ResumeButton.IsEnabled = false;
            StopButton.IsEnabled = false;
        }

        private void SetRunningState()
        {
            StartButton.IsEnabled = false;
            PauseButton.IsEnabled = true;
            ResumeButton.IsEnabled = false;
            StopButton.IsEnabled = true;
        }

        private void SetPausedState()
        {
            StartButton.IsEnabled = false;
            PauseButton.IsEnabled = false;
            ResumeButton.IsEnabled = true;
            StopButton.IsEnabled = true;
        }

        private void SetCompletedState()
        {
            StartButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
            ResumeButton.IsEnabled = false;
            StopButton.IsEnabled = false;

            TaskName.IsEnabled = true;
            TimerInputBox.IsEnabled = true;
        }
    }
}