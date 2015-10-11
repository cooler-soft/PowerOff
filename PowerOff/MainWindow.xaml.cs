using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace PowerOff
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer timer;
        private bool Start = true;
        private DispatcherTimer dispatcherTimer;
        private Reboot reboot;
        public MainWindow()
        {
            InitializeComponent();
            reboot = new Reboot();
            OpenFile();
        }
    
        private void hoursDown_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var image = (Image)sender;
            if (image == minuteUp)
                timer.SetTimes(timer.GetTimes().Add(new TimeSpan(0, 1, 0)));
            if (image == minuteDown)
                timer.SetTimes(timer.GetTimes().Subtract(new TimeSpan(0, 1, 0)));
            if (image == hoursUp)
                timer.SetTimes(timer.GetTimes().Add(new TimeSpan(1, 0, 0)));
            if (image == hoursDown)
                timer.SetTimes(timer.GetTimes().Subtract(new TimeSpan(1, 0, 0)));
            time.Content = String.Format("{0:hh}:{0:mm}", timer.GetTimes());
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Start)
            {
                hoursDown.IsEnabled = false;
                hoursUp.IsEnabled = false;
                minuteDown.IsEnabled = false;
                minuteUp.IsEnabled = false;
                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();
                start_stop.Content = "Stop";
                second.Visibility = Visibility.Visible;
                PowerOff_pc.Visibility = Visibility.Visible;
                Start = false;
                SaveFile();
            }
            else
            {
                hoursDown.IsEnabled = true;
                hoursUp.IsEnabled = true;
                minuteDown.IsEnabled = true;
                minuteUp.IsEnabled = true;

                dispatcherTimer.Stop();
                start_stop.Content = "Start";
                second.Visibility = Visibility.Hidden;
                PowerOff_pc.Visibility = Visibility.Hidden;
                Start = true;
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            timer.SetTimes(timer.GetTimes().Subtract(new TimeSpan(0, 0, 1)));
            time.Content = String.Format("{0:hh}:{0:mm}", timer.GetTimes());
            second.Content = String.Format("{0:ss}", timer.GetTimes());
            if (timer.GetTimes().TotalSeconds == 0)
            {
                dispatcherTimer.Stop();
                second.Content = String.Format("{0:ss}", timer.GetTimes());
                reboot.halt(false, false);
                start_stop.Content = "Start";
                Start = true;

            }
        }

        private void start_stop_MouseEnter(object sender, MouseEventArgs e)
        {
            start_stop.Foreground = SystemColors.HighlightBrush;
        }

        private void start_stop_MouseLeave(object sender, MouseEventArgs e)
        {
            start_stop.Foreground = Brushes.White;
        }

        private void OpenFile()
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            if (File.Exists("Setting.dat"))
            {
                using (Stream fStream = File.OpenRead("Setting.dat"))
                {
                    timer = (Timer)binFormat.Deserialize(fStream);
                }
            }
            else
                timer = new Timer();
            time.Content = String.Format("{0:hh}:{0:mm}", timer.GetTimes());
        }

        private void SaveFile()
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            using (Stream fStream = new FileStream("Setting.dat", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                binFormat.Serialize(fStream, timer);
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            exit.Source = new BitmapImage(new Uri("pack://application:,,,/Image/0.png", UriKind.Absolute));
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            exit.Source = new BitmapImage(new Uri("pack://application:,,,/Image/01.png", UriKind.Absolute));
        }
    }
}
