using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleVideoPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var config = new Domain.Config();


            foreach (var arg in e.Args.Select(x=> x.ToLower()))
            {
                if (arg.Contains("-json"))
                {
                    config.Source = Domain.Config.Sources.JSON;
                    if (arg.Contains("-json:")) processPath(config, arg.Replace("-json:", ""));
                }
  
                if (arg.Contains("-ini"))
                {
                    config.Source = Domain.Config.Sources.INI;
                    if (arg.Contains("-ini:")) processPath(config, arg.Replace("-ini:", ""));
                }

                if (arg.Contains("-csv"))
                {
                    config.Source = Domain.Config.Sources.CSV;
                    if (arg.Contains("-csv:")) processPath(config, arg.Replace("-csv:", ""));
                }

                if (arg.Contains("-port:"))
                {

                    config.SerialPort = arg.Replace("-port:", "");
                    config.UseSerial = true;
                }

                if (arg.Contains("-baud:"))
                {
                    if (int.TryParse(arg.Replace("-baud:", ""), out int baudRate)) config.BaudRate = baudRate;
                }
            }

            MainWindow mainWindow = new MainWindow(config);
            mainWindow.Show();
        }


        private void processPath(Domain.Config config, string path)
        {
            if(System.IO.Path.IsPathRooted(path))
                config.BasePath = System.IO.Path.GetDirectoryName(path);

            config.FileName = System.IO.Path.GetFileNameWithoutExtension(path);
        }
    }
}
