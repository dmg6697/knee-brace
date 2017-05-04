## Synopsis

This project is the result of a senior design project at RIT.  This repository captures the electronic components of the project, while the rest of the project is available [here](http://edge.rit.edu/edge/P17012/public/Home).

The electronic project includes:
* PC data collection application
* NetDuino Plus 2 firmware & supporting application

## Code Explanation

Code was written with an MVC design in mind, but pivots during programming caused some issues with a totally clean implementation.  There are three layers:
1.  Microcontroller layer - Handles firmware and all code on the microcontroller including Bluetooth communication and ADC logic.
2.  Interface layer - Controller layer between the display and the microcontroller.  Ideally a pass-through.
3.  Display layer - View type layer that displays on the Windows/Android/Apple end device.  Only implemented in Windows as of now using Chromium in a Windows form (CefSharp).

## Installation

Clone this Git repository.  Ensure that you have Visual Studio 2013 Express installed if you wish to edit the NetDuino application.  Newer versions of VS are **NOT** supported because of plugin requirements.  The PC data application was created in VS 2015 Community but should not have any strict requirements.

## Contributions

* Microcontrollers:
** The project specified a TI CC2650/40 for Bluetooth operation.  Due to time constrains and several issues with programming the chip the NetDuino Plus 2 was used in the final delivery instead.  Future work on this project should include a PCB design for the TI chipset as well as GATT or another Bluetooth stack architecture with greater robustness and reliability.
** Further diagnostics and data logging should be incorporated in order to increase robustness.  Data logging should include step detection.

* PC Data Application
** The project will require alterations if the Bluetooth framework is overhauled.
** The project should show greater separation into an MVC style framework.  Business logic and display logic should be isolated into one area instead of split between javascript in the Chromium implementation and C# in the windows forms application.

* Mobile Application(s)
** iPhone/Apple implementation is needed
** Android implementation is needed