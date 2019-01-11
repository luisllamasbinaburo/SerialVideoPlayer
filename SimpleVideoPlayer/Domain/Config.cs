using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleVideoPlayer.Domain
{
    public class Config
    {

        public enum Sources
        {
            JSON,
            INI,
            CSV
        }

        public enum Modes
        {
            KEY,
            BUFFERED
        }

        public Modes Mode { get; set; } = Config.Modes.BUFFERED;
        public Sources Source { get; set; } = Config.Sources.JSON;

        public bool NoKeyboard { get; set; } = false;

        public bool UseSerial { get; set; }
        public string SerialPort { get; set; }
        public int BaudRate { get; set; } = 9600;

        public string BasePath { get; set; } = System.AppDomain.CurrentDomain.BaseDirectory;

        public string FileName { get; set; } = "playlist";

        public string FullPath
        {
            get {
                var path = System.IO.Path.Combine(BasePath, FileName);
                var extension = Source == Sources.CSV ? "csv" :
                                Source == Sources.INI ? "ini" :
                                "json";

                return System.IO.Path.ChangeExtension(path, extension);
                }
        }
    }
}
