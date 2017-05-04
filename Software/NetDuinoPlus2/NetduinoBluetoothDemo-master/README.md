# NetduinoBluetoothDemo
a netduino plus 2 with a RovingNetworks RN42 bluetooth thing (hanging off a sensor shield)

setup:
// http://www.netduino.com/downloads/  
// follow instructions to load dev env. under netduino 3, netduino 2, and go  
// (this is a netduino plus 2).  
// i updated the firmware to 4.3.2.1  
// https://learn.sparkfun.com/tutorials/using-the-bluesmirf - tells how to set up com terminal  
// http://www.netduino.com/downloads/gettingstarted.pdf  
// https://msdn.microsoft.com/en-us/subscriptions/downloads/  

// Test with Hyperterminal  
http://www.hilgraeve.com/hyperterminal-trial/  

1. connect the netdiuno to the PC with the USB cable.  
2. Open visual studio project - run it.  
3. Pair the PC bluetooth with the rn42 - open Control Panel > Devices and Printers and 'Add Device'  your device is named RN42-Roboto001-762E  
4.  Once it's paired, open the device and go to Hardware tab - it will tell you which com port the bluetooth link is on  
5. Open hyperterminal and create a new connection to the com port that you found. Be sure to enable line feeds under File>Properties>Setup>Ascii Setup - the lf is what the code looks for to do the echo.   
5a. Successfully openning a connection is shown on the bluetooth card by steady green LED.  

Type stuff, then hit enter.  if all works, the text will be echoed back to your terminal.  Text is also written to the console in VS.
