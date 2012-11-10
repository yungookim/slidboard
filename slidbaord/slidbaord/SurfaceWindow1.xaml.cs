using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;

using System.Collections;
using Newtonsoft.Json;


namespace slidbaord
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {

        //private SocketClient sc;
        private ArrayList deviceIds = new ArrayList();

        private SocketClient sc;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            //Establish a TCP connection with the server
            //Async Stlye
            //AsynchronousClient.StartClient();
            //Blocking calls
            //sc = new SocketClient("69.164.219.86", 6060);
            //sc.connect();

            //Or doing it in HTTP

            HttpClient.GET("init", )
        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            //Close the socket
            //sc.close();

            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

        private void OnVisualizationAdded(object sender, TagVisualizerEventArgs e)
        {

            ObjectVisualization _obj = (ObjectVisualization)e.TagVisualization;
            switch (_obj.VisualizedTag.Value)
            {
                case 0xC1:
                    String deviceId = "87841656-3842-40cb-af59-389ee46b23cd";
                    this.deviceIds.Add(deviceId);

                    _obj.ObjectModel.Content = "KimY's Phone";
                    _obj.objectWrapper.Fill = SurfaceColors.Accent1Brush;

                    JSONRequestIndex reqMsg = new JSONRequestIndex(deviceId, "sdcard");
                    JSONMessageWrapper msgWrapper = new JSONMessageWrapper("getIndex", reqMsg.request());

                    String response = HttpClient.GET("getIndex", msgWrapper.getMessage());

                    break;
                case 0xC2:
                    _obj.ObjectModel.Content = "Nexus One";
                    _obj.objectWrapper.Fill = SurfaceColors.Accent2Brush;
                    break;
                default:
                    _obj.ObjectModel.Content = "UNKNOWN MODEL";
                    _obj.objectWrapper.Fill = SurfaceColors.ControlAccentBrush;
                    break;
            }
        }
    }
}
