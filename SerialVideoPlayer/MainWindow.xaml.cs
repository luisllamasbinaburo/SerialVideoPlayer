using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Collections.ObjectModel;
using Microsoft.Expression.Encoder.Devices;
using System.Runtime.CompilerServices;
using System.IO;
using Newtonsoft.Json;
using SimpleVideoPlayer.Domain;

namespace SimpleVideoPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        #region Fields
        private string _command = "";
        private string _buffer = "";
        private ArduinoSerialPort _arduinoSerialPort;
        #endregion  

        #region Properties
        public Config Config { get; set; } = new Config();

        Playlist Playlist { get; set; }

        private string _videoPath;
        public string VideoPath
        {
            get { return _videoPath; }
            set
            {
                if (value == _videoPath) return;
                _videoPath = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public MainWindow(Config config)
        {
            Config = config;
            this.DataContext = this;

            InitializeComponent();
            Init();
        }


        #region Events
        private void Window_TextInput(object sender, TextCompositionEventArgs e)
        {
            ProcessText(e.Text);
        }

        private void ArduinoSerialPort_DataArrived(object sender, EventArgs e)
        {
            foreach (var key in _arduinoSerialPort.LastRecieved)
            {
                ProcessText(key.ToString());
            }
        }

        private void VideoControl_ResetPosition(object sender, RoutedEventArgs e)
        {
            VideoControl.Position = TimeSpan.FromSeconds(0);
        }


        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (Config.UseSerial) _arduinoSerialPort.DataArrived -= ArduinoSerialPort_DataArrived;
        }
        #endregion

        #region Methods
        private void Init()
        {
            if (!Config.NoKeyboard) this.TextInput += Window_TextInput;
            if (Config.UseSerial) InitSerialPort(Config);

            Playlist = GetPlaylistFactory()?.CreatePlaylist();
            LoadVideo(Playlist?.Items.FirstOrDefault());
        }

        private void InitSerialPort(Config config)
        {
             _arduinoSerialPort = new ArduinoSerialPort();
             _arduinoSerialPort.DataArrived += ArduinoSerialPort_DataArrived;

            try
            {
                _arduinoSerialPort.Open(config.SerialPort, config.BaudRate);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private IPlaylistFactory GetPlaylistFactory()
        {
            if (Config.Source == Config.Sources.JSON) return new PlaylistFactoryJSON() { JsonPath = Config.FullPath };
            if (Config.Source == Config.Sources.INI) return new PlaylistFactoryINI() { IniPath = Config.FullPath };
            if (Config.Source == Config.Sources.CSV) return new PlaylistFactoryCSV() { CsvPath = Config.FullPath };
            return null;
        }

        private void ProcessText(string text)
        {
            if (Config.Mode == Config.Modes.KEY) ProcessKey(text);
            if (Config.Mode == Config.Modes.BUFFERED) ProcessKeyBuffered(text);
        }

        private void ProcessKey(string text)
        {
            _command = text;
            RunCommands();
        }

        private void ProcessKeyBuffered(string text)
        {
            if (text == '\r'.ToString())
            {
                _command = _buffer;
                _buffer = "";
                RunCommands();
            }
            else
            {
                _buffer += text;
            }
        }

        public void RunCommands()
        {
            if(_command.Any())
            {
                var video = Playlist.GetByCommand(_command);
                LoadVideo(video);
            }
        }

        private void LoadVideo(PlaylistItem playlistItem)
        {
            if (playlistItem == null || string.IsNullOrWhiteSpace(playlistItem.URL)) return;
            VideoPath = playlistItem.URL;
        }
        #endregion


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [Annotations.NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion  
    }
}
