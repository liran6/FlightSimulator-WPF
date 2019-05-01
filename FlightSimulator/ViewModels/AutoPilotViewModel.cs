using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FlightSimulator.Model;

namespace FlightSimulator.ViewModels
{
    class AutoPilotViewModel : BaseNotify
    {
        private ICommand sendCommand = null;
        private ICommand clearCommand = null;
        private string text;
        //private string color;
        private bool startedWrite;
        public ConnectionCommand commandClient;
        public bool isOkPressed = false;
        public string oldTxt;

        public AutoPilotViewModel()
        {
            commandClient = ConnectionCommand.getInstance;
            //color = "White";
            text = "";
            oldTxt = "";
            startedWrite = false;
        }

        // Property of UserText
        public string UserText
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                startedWrite = true;
                // notify that the text and background has change
                NotifyPropertyChanged("UserText");
                NotifyPropertyChanged("BackgroundColor");
            }
        }

        // Property of BackgroundColor
        public string BackgroundColor
        {
            get
            {
                return startedWrite ? "Pink" : "White";
              
            }
        }

        // Property of SendCommand
        public ICommand SendCommand
        {
            get
            {
                // activate SendClick
                return sendCommand ?? (sendCommand = new CommandHandler(() => SendClick()));
            }
        }

        // Property of ClearCommand
        public ICommand ClearCommand
        {
            get
            {
                // activate ClearClick
                return clearCommand ?? (clearCommand = new CommandHandler(() => ClearClick()));
            }
        }

        /*
         * sent the command to the simulator, and change the back ground to white
         */
        private void SendClick()
        {
            string[] delimeter = { "\r\n" };
            string[] commandLines = text.Split(delimeter, StringSplitOptions.None);
            startedWrite = false;
            commandClient.AutoSend(commandLines);
            oldTxt = UserText;
            isOkPressed = true;
            NotifyPropertyChanged("BackgroundColor");
        }

        /*
         * delete the text in the auto pilot
         */
        private void ClearClick()
        {
            text = "";
            startedWrite = false;
            NotifyPropertyChanged("UserText");
            NotifyPropertyChanged("BackgroundColor");
        }

    }
}
