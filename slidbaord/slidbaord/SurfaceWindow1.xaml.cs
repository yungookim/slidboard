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
        ObjectVisualization deviceObject;

        public static ScatterView GlobalDirList;

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

            //Make the DirList ScatterView accessable globally
            GlobalDirList = this.DirList;

            //Or doing it in HTTP
            JSONMessageWrapper _msg = new JSONMessageWrapper("init", "");
            String response = HttpClient.GET("init", _msg.getMessage());

            //TODO DELETE THIS. FOR TESTING ONLY
            //String deviceId = "87841656-3842-40cb-af59-389ee46b23cd";
            //this.getIndexObject(deviceId);
            
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

        private void OnStack(object sender, EventArgs e)
        {

            foreach (ScatterViewItem ic in this.DirList.Items)
            {
                if (!ic.Name.Equals("ControlBox"))
                {
                    Point point = deviceObject.Center;
                    point.X -= 300;
                    ic.Center = point;
                }
            }

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
            this.deviceObject = _obj;
            
            switch (_obj.VisualizedTag.Value)
            {
                case 0xC1:
                    String deviceId = "87841656-3842-40cb-af59-389ee46b23cd";
                    String deviceName = "Samsung Infuse";

                    _obj.ObjectModel.Content = deviceName;
                    _obj.objectWrapper.Fill = SurfaceColors.Accent1Brush;

                    ScatterViewItem[] ls = _obj.createFileList(
                                            HttpClient.getIndexObject(deviceId, "/mnt/sdcard"), 
                                            deviceName);

                    Console.WriteLine("Dir views added");
                    foreach (ScatterViewItem i in ls)
                    {
                        if (i != null)
                        {
                            this.DirList.Items.Add(i);
                        }
                    }

                    break;
                case 0xC2:
                    _obj.ObjectModel.Content = "Nexus One";
                    _obj.objectWrapper.Fill = SurfaceColors.Accent2Brush;
                    break;
                default:
                    _obj.ObjectModel.Content = "UNKNOWN MODEL";
                    this.DirList.Visibility = Visibility.Hidden;
                    //_obj.objectWrapper.Fill = SurfaceColors.ControlAccentBrush;
                    break;
            }
        }

        private void OnVisualizationRemoved(object sender, TagVisualizerEventArgs e) 
        {
            ObjectVisualization _obj = (ObjectVisualization)e.TagVisualization;
            this.DirList.Items.Clear();

            ScatterViewItem controlbox = new ScatterViewItem();
            controlbox.Name = "ControlBox";
            //controlbox.Background = Brush."#5D9D2020";
            controlbox.BorderThickness = new Thickness(0);
            controlbox.CanScale = false;
            controlbox.CanMove = false;
            controlbox.Center = new Point(100, 100);
            controlbox.Orientation = 0;

            SurfaceButton checkbox = new SurfaceButton();
            checkbox.Name = "CenterItems";
            checkbox.Background = Brushes.Aquamarine;
            checkbox.FontSize = 14;
            checkbox.Margin = new Thickness(10, 10, 10, 10);
            checkbox.Click += new RoutedEventHandler(OnStack);
            checkbox.Content = "Stack Items";
            controlbox.Content = checkbox;

            this.DirList.Items.Add(controlbox);
        }

    }
}
