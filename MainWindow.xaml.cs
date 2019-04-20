using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;

namespace Wpf_MediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;
        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer dt = new DispatcherTimer();
            dt.Tick += new EventHandler(timer_trick);
            dt.Interval = new TimeSpan(0, 0, 1);
            dt.Start();

        }

        private void btn_pause_Click(object sender, RoutedEventArgs e)
        {
            MediaElement1.Pause();
        }

        private void btn_play_Click(object sender, RoutedEventArgs e)
        {
            MediaElement1.Play();

        }

        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            MediaElement1.Stop();
        }

        private void btn_open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd= new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "*.*";
            ofd.Filter = "Media Files (*.mp3;*.mpg;*.mpeg;*.MKV;*.mp4;*.MP4)|*.mp3;*.mpg;*.mpeg*.MKV;*.mp4;*.MP4|All files (*.*)|*.*";
            ofd.ShowDialog();
            try
            {
                listplay.Items.Clear();
 
                MediaElement1.Source = new Uri(ofd.FileName);
                listplay.Items.Add(MediaElement1.Source);
                listplay.SelectedIndex = 0;
                btn_play_Click(null, null);
            }
            catch (Exception ex)
            {
                new NullReferenceException("Error"+ex);
            }
                  
           
        }
        void timer_trick(object sender, EventArgs e)
        {
            if ((MediaElement1.Source != null) && (MediaElement1.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                sli_movie.Minimum = 0;
                sli_movie.Maximum = MediaElement1.NaturalDuration.TimeSpan.TotalSeconds;
                sli_movie.Value = MediaElement1.Position.TotalSeconds;
            }
        }

        private void sli_movie_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TimeSpan ts = TimeSpan.FromSeconds(e.NewValue);
            MediaElement1.Position = ts;
            lblProgressStatus.Text = TimeSpan.FromSeconds(sli_movie.Value).ToString(@"hh\:mm\:ss");

        }

        private void sli_sound_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MediaElement1.Source != null)
            {
                MediaElement1.Volume = sli_sound.Value;
                lblVolume.Text = MediaElement1.Volume.ToString();
            }
        }

        private void MediaElement1_MediaOpened(object sender, RoutedEventArgs e)
        {
            if(MediaElement1.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = TimeSpan.FromMilliseconds(MediaElement1.NaturalDuration.TimeSpan.TotalMilliseconds);
                sli_movie.Maximum = ts.TotalSeconds;
                lblTotal.Text = ts.ToString(@"hh\:mm\:ss");
                media.Title = MediaElement1.Source.OriginalString;


               

            }
           
                sli_sound.Minimum = 0;
                sli_sound.Maximum = 100;
                sli_movie.Value = 50;
                MediaElement1.Volume = 50;
                lblVolume.Text = int.Parse(MediaElement1.Volume.ToString()).ToString();


        }

        private void sli_movie_DragStarted(object sender, DragStartedEventArgs e)
        {
            MediaElement1.Pause();
            userIsDraggingSlider = true;

        }

        private void sli_movie_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            MediaElement1.Position = TimeSpan.FromSeconds(sli_movie.Value);
            MediaElement1.Play();
        }


        private void btn_addtolist_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "*.*";
            ofd.Filter = "Media Files (*.mp3;*.mpg;*.mpeg;*.MKV;*.mp4;*.MP4)|*.mp3;*.mpg;*.mpeg*.MKV;*.mp4;*.MP4|All files (*.*)|*.*";
            ofd.Multiselect = true; 
            ofd.ShowDialog();
            try
            {
                foreach (var x in ofd.FileNames)
                {
                    if(!listplay.Items.Contains(x))
                    listplay.Items.Add(x);
                    
                }
                if (MediaElement1.Source == null)
                {
                    MediaElement1.Source = new Uri(ofd.FileName);
                    listplay.SelectedIndex = 0;
                    btn_play_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                new NullReferenceException("Error" + ex);
            }
        }

        private void listplay_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MediaElement1.Source = new Uri(listplay.SelectedItem.ToString());
            btn_play_Click(null, null);
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
                listplay.SelectedItem = MediaElement1.Source;
                int index= listplay.SelectedIndex;
                listplay.SelectedIndex = index+1;
            MediaElement1.Source =new Uri( listplay.SelectedItem.ToString());


        }

        private void btn_prev_Click(object sender, RoutedEventArgs e)
        {
            listplay.SelectedItem = MediaElement1.Source;
            int index = listplay.SelectedIndex;
            if(index !=0)
            listplay.SelectedIndex = index -1;
            MediaElement1.Source = new Uri(listplay.SelectedItem.ToString());
        }
    }
}
