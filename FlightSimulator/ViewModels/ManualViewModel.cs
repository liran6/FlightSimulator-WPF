using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Model;

namespace FlightSimulator.ViewModels
{
    class ManualViewModel : BaseNotify
    {
        private double changeThrottle;
        private double changeRudder;
        private double aileron;
        private double elevator;


        public double ChangeThrottle
        {
            get
            {
                return this.changeThrottle;
            }
            set
            {
                this.changeThrottle = value;
                NotifyPropertyChanged("ChangeThrottle");
                ConnectionCommand.getInstance.JoystickSend("controls/engines/current-engine/throttle", changeThrottle);
            }
        }


        public double ChangeRudder
        {
            get
            {
                return this.changeRudder;
            }
            set
            {
                this.changeRudder = value;
                NotifyPropertyChanged("ChangeRudder");
                ConnectionCommand.getInstance.JoystickSend("controls/flight/rudder", changeThrottle);
            }
        }


        public double Aileron
        {
            get
            {
                return this.aileron;
            }
            set
            {
                this.aileron = value;
                NotifyPropertyChanged("ChangeAileron");
                ConnectionCommand.getInstance.JoystickSend("controls/flight/aileron", aileron);
            }
        }

        public double Elevator
        {
            get
            {
                return elevator;
            }
            set
            {
                this.elevator = value;
                NotifyPropertyChanged("ChangeElevator");
                ConnectionCommand.getInstance.JoystickSend("controls/flight/elevator", elevator);
            }
        }
    }
}
