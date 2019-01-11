using RJCP.IO.Ports;
using SimpleVideoPlayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleVideoPlayer.Domain
{
    internal class ArduinoSerialPort
    {
        private const char NEW_LINE_CHAR = '\r';

        private SerialPortStream _arduinoPort = new SerialPortStream();

        public string LastRecieved { get; private set; } = "";

        public bool IsReady { get; private set; }

        public bool IsOpen;

        public event EventHandler DataArrived;

        public void Open(string port, int baud)
        {
            IsOpen = true;
            _arduinoPort.PortName = port;
            _arduinoPort.BaudRate = baud;
            _arduinoPort.DtrEnable = true;
            _arduinoPort.ReadTimeout = 1;
            _arduinoPort.WriteTimeout = 1;
            _arduinoPort.Open();
            _arduinoPort.DiscardInBuffer();
            _arduinoPort.DiscardOutBuffer();
            _arduinoPort.DataReceived += DataRecieved;
        }


        public void Close()
        {
            if (!IsOpen) return;
            try
            {
                _arduinoPort.Flush();
                _arduinoPort.DataReceived -= DataRecieved;
                _arduinoPort.Close();
                IsOpen = false;
            }
            catch (Exception)
            {
                //do nothing
            }

        }

        private void DataRecieved(object sender, RJCP.IO.Ports.SerialDataReceivedEventArgs e)
        {
            LastRecieved = _arduinoPort.ReadExisting().Replace('\n'.ToString(), ""); ;
            DataArrived?.Invoke(this, new EventArgs());
        }
    }
}
