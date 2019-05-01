using FlightSimulator.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Views;
using System.Windows.Input;
using FlightSimulator.Model;

namespace FlightSimulator.ViewModels
{
    public class FlightBoardViewModel : BaseNotify
    {
        private ICommand setCommand;
        private double lat;
        private double lon;
        private static FlightBoardViewModel instance = null;

        /*
         * singleton to create only one instance of the FlightBoardViewModel
        */
        public static FlightBoardViewModel getInstance
        {
            get
            {
                return null == instance ? instance = new FlightBoardViewModel() : instance;
            }
        }
        // Property of Lat
        public double Lat
        {
            get
            {
                return this.lat;
            }
            set
            {
                this.lat = value;
                NotifyPropertyChanged("Lat");
            }
        }

        // Property of Lon
        public double Lon
        {
            get
            {
                return this.lon;
            }
            set
            {
                this.lon = value;
                NotifyPropertyChanged("Lon");
            }
        }


        // Property of OpenSettingsWindow
        public ICommand OpenSettingsWindow
        {
            get
            {
          
                return setCommand == null ? (setCommand = new CommandHandler(() => OnClick())) : setCommand;
            }
        }

        /*
         * open the settings window
         */
        private void OnClick()
        {
            Settings settings = new Settings();
            // shows the window
            settings.ShowDialog();
        }


        bool isConnect = false;
        private ICommand connectCommand;

        public ICommand ConnectCommand
        {
            get
            {
                // activate ConnectClick
                return connectCommand  == null ? (connectCommand = new CommandHandler(() => ConnectClick())) : connectCommand;
            }
        }

        /*
          * if not connected- connect the client and server
        */
        private void ConnectClick()
        {
            // only if not connected- connect.
            if (!this.isConnect)
            {
                this.open();
                this.isConnect = true;
            }
            else
            {
                return;
            }
        }

        private void open()
        {
            new Task(() =>
            {
                ConnectionInfo.getInstance.openInfoServer();
                ConnectionCommand.getInstance.ConnectToServer();
            }).Start();
        }


    }
}
