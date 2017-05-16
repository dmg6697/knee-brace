using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace P17012CEF
{
    public partial class P17012Form : Form
    {
        private ChromiumWebBrowser _browser;
        private const string _startPage = @"web\knee.html";
        private bool _debugMode = false;

        /// <summary>
        /// Form constructor
        /// </summary>
        public P17012Form()
        {
            InitializeComponent();

            // initialize CefSharp library for chromium browser instance
            CefSettings settings = new CefSettings();
            CefSharp.Cef.Initialize(settings);
        }

        /// <summary>
        /// Event raised in the case where we've received a BTMessage instance from the BT communicator.
        /// </summary>
        /// <param name="msg"></param>
        private void Btc_MessageReceived(BTMessage msg)
        {
            // message type is locked here as ReadADC.
            // if (msg.GetCmdType() == BTMessage.CmdType.ReadADC) other event types handled elsewhere (internally in bluetoothcommunicator)
            string adc_str = System.Text.Encoding.UTF8.GetString(msg.DataPayload); // this could be contained in 2 bytes, but utf-8 uses 4 bytes in this case so we need to make sure the encoding stays consistent
            int adc_value;
            bool parsed = int.TryParse(adc_str, out adc_value);
            if (!parsed)
            {
                adc_value = 0;
            }

            // get browser update data
            // TODO: Consider including all these properties in an object/JSON or dictionary type structure
            double pforce = ADCInterpreter.GetForcePercentage(adc_value);
            double flexion = ADCInterpreter.GetFlexion(adc_value, false);
            double orientation = DegreesToRadians(180); // TODO: implement.  Issue: Do not have a secondary sensor for this yet.
            double force = ADCInterpreter.GetForce(adc_value);

            // send the update to the browser instance
            _browser.ExecuteScriptAsync("Update", force, flexion, orientation, pforce);
            
            Logger.Log(adc_str + "\r\n"); // log adc values
        }

        /// <summary>
        /// Resize the form's contents to appropriately fit the new resolution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P17012Form_Resize(object sender, EventArgs e)
        {
            Size panelSize = this.ClientSize; // use client size for better total accuracy of draw area, .Size() reports with the window styling, etc.
            /// browser section
            ResizeBrowserContainer(panelSize);
            /// end browser section

            /// controls section
            ResizeControlsSection(panelSize);
            /// end controls section
        }

        /// <summary>
        /// Assists in scaling by resizing a Size by a percentage
        /// </summary>
        /// <param name="original">Window or Client Size</param>
        /// <param name="wPercent">Double as percentage.  i.e. 33 = 33%, 0.33 = 0.33%</param>
        /// <param name="hPercent">Double as percentage.  i.e. 33 = 33%, 0.33 = 0.33%</param>
        /// <returns></returns>
        private Size ResizePercent(Size original, double wPercent, double hPercent)
        {
            Size newSize = new Size();
            newSize.Height = (int)Math.Floor(original.Height * hPercent / 100);
            newSize.Width = (int)Math.Floor(original.Width * wPercent / 100);

            return newSize;
        }

        /// <summary>
        /// Main form load.  Handles further configuration and initialization of the browser instance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P17012Form_Load(object sender, EventArgs e)
        {
            /// Browser section
            _browser = new ChromiumWebBrowser(String.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory, _startPage));

            Size panelSize = this.ClientSize;
            ResizeBrowserContainer(panelSize);
            /// end browser section

            /// controls section
            ResizeControlsSection(panelSize);
            /// end controls section

            _browser.RegisterAsyncJsObject("csharp", Program.jsc); // register the javascript -> csharp communications object

            _browser.IsBrowserInitializedChanged += _browser_IsBrowserInitializedChanged; // register the initialization event, because we can't *just* check the IsBrowserInitialized variable without the event or the variable may not be correct
        }

        /// <summary>
        /// Event issued when the bluetooth connection attempt has completed, regardless of success.
        /// </summary>
        /// <param name="IsConnected">Bool:  True if the connection attempt was succesful, else false</param>
        /// <param name="portNames">Available serial port names to make available to the user for selection.  Empty string if the connection attempt was successful.</param>
        private void Btc_Connected(bool IsConnected, string[] portNames)
        {
            // json serialization and related passage of variables doesn't seem to work as expected so we have to split out the string on the js side
            _browser.ExecuteScriptAsync("connectionChanged", IsConnected, String.Join(",", portNames));

            // if using the winforms method uncomment this and comment the browser line
            //Invoke(new SetupFormDelegate(SetupFormLaunch));
        }

        /// <summary>
        /// Alternate set up form for configuring serial port names, etc. if the chromium implementation is not desired.
        /// </summary>
        private delegate void SetupFormDelegate();
        private void SetupFormLaunch()
        {
            Setup setupForm = new Setup();

            setupForm.ShowDialog(this);
        }

        /// <summary>
        /// Chromium event for when CefSharp browser instance readiness is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _browser_IsBrowserInitializedChanged(object sender, IsBrowserInitializedChangedEventArgs e)
        {
            if (_browser.IsBrowserInitialized)
            {
                /// subscribe to bluetooth messages
                Program.btc.MessageReceived += Btc_MessageReceived; // bluetooth message received event
                Program.jsc.MessageReceived += Jsc_MessageReceived; // javascript listener events

                // serial port connection
                Program.btc.Connected += Btc_Connected; // bluetooth connection event
                Program.btc.ConnectAsync(); // attempt to connect asynchronously so that we don't hold up the form loading procedures
            }
        }

        /// <summary>
        /// Invocation handler from JavaScript side in Chromium.
        /// </summary>
        /// <param name="obj"></param>
        private void Jsc_MessageReceived(object obj)
        {
            // ignore null message calls
            if (obj == null) return;

            // TODO: Greater robustness for message types and better definition of message expectations.  Alternative method for error/log messages split from the expected commands so that logging has a separation.

            if (typeof(string) == obj.GetType())
            {
                Console.WriteLine(obj);
            }
            else // should be a dictionary / js object
            {
                // json parser requires a type, but if you treat it as a dictionary you can parse out a "generic" object
                try
                {
                    Dictionary<string, object> js = (Dictionary<string, object>)obj;

                    // "object" type
                    string type = js["type"].ToString();

                    switch (type)
                    {
                        case "connection": // handle user selection of a different serial port
                            Program.btc.ConnectAsync(js["port"].ToString());
                            break;
                        case "settings": // handle message sending to the microcontroller from javascript
                            // bt message to the controller
                            int interval = 0;
                            int.TryParse(js["f"].ToString(), out interval); // frequency of transmission parameter
                            byte[] payload = Encoding.UTF8.GetBytes(interval.ToString());
                            BTMessage msg = new BTMessage(BTMessage.CmdType.WriteSetting, payload, BTMessage.NextPacketID(), false);
                            Program.btc.EnqueueMsg(msg); // TODO: Verify operation of this

                            // local program variables
                            int k = 0;
                            int.TryParse(js["k"].ToString(), out k); // spring constant parameter from javascript for the adcinterpretor
                            ADCInterpreter.LbsPerInch = k;
                            break;
                        default: // otherwise make sure we capture unexpected commands
                            Console.WriteLine("Unrecognized command: {0}", obj);
                            break;
                    }
                } catch (Exception e)
                {
                    Console.WriteLine(e.GetType() + ":" + e.Message);
                }
            }
        }

        /// <summary>
        /// Handles resizing and repositioning of browser elements
        /// </summary>
        /// <param name="panelSize"></param>
        private void ResizeBrowserContainer(Size panelSize)
        {
            BrowserContainer.AutoScroll = false;
            int height = 100;
            if (_debugMode)
            {
                height = 70;
            }
            BrowserContainer.Size = ResizePercent(panelSize, 100, height);
            BrowserContainer.Controls.Add(_browser);
            BrowserContainer.Margin = new Padding(0, 0, 0, 0);
            BrowserContainer.Padding = new Padding(0, 0, 0, 0);
            BrowserContainer.Location = new Point(0, 0);
            BrowserContainer.BackColor = Color.Transparent;
            _browser.Dock = DockStyle.None;
            _browser.Size = BrowserContainer.Size;
            _browser.Margin = new Padding(0, 0, 0, 0);
            _browser.Padding = new Padding(0, 0, 0, 0);
        }

        /// <summary>
        /// WinForms controls resizing, primarily for debug purposes.
        /// </summary>
        /// <param name="panelSize"></param>
        private void ResizeControlsSection(Size panelSize)
        {
            int height = 100;
            if (_debugMode)
            {
                height = 30;
            }
            ControlsContainer.Size = ResizePercent(panelSize, 100, height);
            ControlsContainer.Location = new Point(0, BrowserContainer.Height);
            ControlsContainer.BackColor = Color.Transparent;
            ControlsContainer.Margin = new Padding(0, 0, 0, 0);
            ControlsContainer.Padding = new Padding(0, 0, 0, 0);

            RepositionControls();
        }

        /// <summary>
        /// Evenly distributes winforms controls for better generic appearance
        /// </summary>
        private void RepositionControls()
        {
            // inner control positioning
            List<Control> ctrls = new List<Control>();

            for (int i = 0; i < ControlsContainer.Controls.Count; i++)
            {
                ctrls.Add(ControlsContainer.Controls[i]);
            }

            ctrls.Sort((m1, m2) => CompareTag(m1, m2)); // reorder the panels/buttons to keep consistency

            Control[] controls = ctrls.ToArray();

            int pad = 0; // warning: pad variable is not applied quite correctly.  Non-zero values will result in unintended behavior.
            Point p = new Point(pad, 0);
            Size stdMinSize = new Size(200, (new TextBox()).Height + (new Label()).Height);
            

            List<List<Control>> controlMatrix = new List<List<Control>>();
            controlMatrix.Add(new List<Control>());

            // sort our controls into a row/column matrix so that we can place them without as much guessing
            foreach (Control c in controls) // first pass determine row
            {
                c.Width = stdMinSize.Width;
                c.Height = stdMinSize.Height;

                if (p.X == pad) // just started a new row
                {
                    c.Location = new Point(pad, p.Y + pad);
                    p.X += (c.Size.Width + pad);
                }
                else // continuation of an existing row
                {
                    if (p.X + c.Size.Width + pad > ControlsContainer.Width) // if it spills over
                    {
                        p.Y += (c.Size.Height + pad);
                        p.X = pad;
                        c.Location = p;
                        p.X += (c.Size.Width + pad);

                        controlMatrix.Add(new List<Control>());
                    }
                    else // it doesn't spill over
                    {
                        c.Location = p;
                        p.X += (c.Size.Width + pad);
                    }
                }

                controlMatrix[controlMatrix.Count - 1].Add(c);
            }

            // place the controls according to their row and how much space we can allocate
            int maxCols = controlMatrix.Max(x => x.Count);
            int unitWidth = ControlsContainer.Width / maxCols;
            for (int i = 0; i < controlMatrix.Count; i++)
            {
                List<Control> row = controlMatrix[i];
                for (int k = 0; k < row.Count; k++)
                {
                    row[k].Width = unitWidth;
                    row[k].Location = new Point(k * unitWidth, row[k].Location.Y);
                    

                    if (row[k].GetType().Equals(typeof(Panel)))
                    {
                        for (int t = 0; t < row[k].Controls.Count; t++)
                        {
                            if (row[k].Controls[t].GetType().Equals(typeof(Label))) // label
                            {
                                row[k].Controls[t].Location = new Point(0, 0);
                            }
                            else
                            {
                                row[k].Controls[t].Location = new Point(0, row[k].Controls[t].Height);
                                row[k].Controls[t].Width = row[k].Width;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Manual override for drawing on the browser when BT is down.  TODO: Update this to match the actual update function as opposed to the old one.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateBrowser_Click(object sender, EventArgs e)
        {
            double force = (double)ForceNumeric.Value;
            double flexion = DegreesToRadians((double)FlexNumeric.Value);
            double orientation = DegreesToRadians((double)OrieNumeric.Value);
            int fmax = (int)MaxNumeric.Value;
            int fmin = (int)MinNumeric.Value;
            _browser.ExecuteScriptAsync("Update", force, flexion, orientation, fmax, fmin);
        }

        /// <summary>
        /// Adjusts degrees to radians for use in the chromium instance
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        private double DegreesToRadians(double degrees)
        {
            return (degrees / 180 * Math.PI);
        }

        /// <summary>
        /// Sort prioritization of pre-defined winforms are stored in the Tag property as integers.  Most are not set, they are not required to be set.  Primarily for debug purposes.
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        private int CompareTag(object m1, object m2)
        {
            try
            {
                Control c1 = (Control)m1;
                Control c2 = (Control)m2;

                if ((int)c1.Tag > (int)c2.Tag)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception e) { }
            return 0;
        }


        /// <summary>
        /// Disposal requirements.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P17012Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            // TODO: Ensure these things are properly disposed in formclosing.  May require class destructor (~P17012Form()) or in individual destructors.
            Program.btc.MessageReceived -= Btc_MessageReceived; // unsub from events
            Program.btc.Disconnect();
            _browser.Dispose(); // kill browser
            Logger.Deactivate();
        }
    }
}
