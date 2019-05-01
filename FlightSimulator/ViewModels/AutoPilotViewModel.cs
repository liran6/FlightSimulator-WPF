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
        private bool writingStarted;
        public ConnectionCommand commandClient;    

        public AutoPilotViewModel()
        {
            commandClient = ConnectionCommand.getInstance;
            text = "";
            writingStarted = false;
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
                writingStarted = true;
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
                return writingStarted ? "Pink" : "White";
              
            }
        }

        // Property of SendCommand
        public ICommand SendCommand
        {
            get
            {
                // activate OKClick
                return sendCommand ?? (sendCommand = new CommandHandler(() => OKClick()));
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


        private void OKClick()
        {
            string[] delimeter = { "\r\n" };
            string[] commandLines = text.Split(delimeter, StringSplitOptions.None);
            writingStarted = false;
            commandClient.AutoSend(commandLines);     
            NotifyPropertyChanged("BackgroundColor");
        }

        private void ClearClick()
        {
            text = "";
            writingStarted = false;
            NotifyPropertyChanged("UserText");
            NotifyPropertyChanged("BackgroundColor");
        }

    }
}
