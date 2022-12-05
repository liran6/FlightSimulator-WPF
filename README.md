# FlightSimulator-WPF
Microsoft flightGear simulator controller app.
An interperter for the microsoft flight simuletor written in C# with MVVM architecture.

NOTE: make make sure you have passed valid arguments to FlightGear.

enter these two arguments in the Settings->Additional Settings box:
--generic=socket,out,10,nnn.nnn.nnn.nnn,5400,tcp,generic_small -
with i.p. of computer running interpreter (and put the included generic small xml file in the FlightGear installation folder "data/Protocol").   
--telnet=socket,in,10,nnn.nnn.nnn.nnn,5402,tcp -
with the same i.p. as in the line above...

when you open our appication, click the settings button and insert your IP and two relevant ports (defaults are 5400 for info port
and 5402 for commands port...).

when done configuring your connection settings, press ok (which will close the settings window and save your settings for the next time you
run the application) and the press the connect button to initiate the connection to FlightGear.

once connected, you'll see the coordinates of the plane (longtitude and latitude) shown inside the grid-board on the left side of the
window. On the right side of the window, you can choose between manuel pilot mode, which includes sliders for the simulated plane's rudder and
throttle, and a joystick for the aileron and elevator values. The second option is the autopilot mode, which includes a textbox where you
can write set commands and assignments (e.g. "throttle = 1") for values (throttle, rudder, elevator, aileron and flaps), each in a
different line. Once done inputing commands, press the ok button and the textbox will switch from pink (switched to at the moment you type
something not yet sent to FlightGear) to its original color of white (indicating commands were sent). 2 seconds will seperate each command.
