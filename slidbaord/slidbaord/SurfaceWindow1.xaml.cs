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


namespace slidboard
{
    /// <summary>
    /// Controller for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        SlidboardView deviceObject;

        public static ScatterView GlobalDirList;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            //Make the DirList ScatterView accessable globally
            GlobalDirList = this.DirList;

            //Or doing it in HTTP
            JSONMessageWrapper _msg = new JSONMessageWrapper("init", "");
            //Test the connection to the server
            try
            {
                String response = HttpClient.GET("init", _msg.getMessage());
            }
            catch (Exception e) 
            {
                this.serverNotResondingDialog.Visibility = Visibility.Visible;
            }

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
            int y = 75;
            int zindex = 0;
            int x_subtract = 300;

            foreach (ScatterViewItem ic in this.DirList.Items)
            {
                if (!ic.Name.Equals("ControlBox"))
                {
                    Point point = deviceObject.Center;
                    point.X -= x_subtract;
                    point.Y = y;
                    y += 25;
                    if (y == 1000)
                    {
                        y = 75;
                        x_subtract = 550;
                    }
                    ic.Center = point;
                    ic.Orientation = 0;
                    ic.Width = 200;
                    ic.ZIndex = zindex++;
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

            SlidboardView _obj = (SlidboardView)e.TagVisualization;
            this.deviceObject = _obj;
            
            switch (_obj.VisualizedTag.Value)
            {
                case 0xC1:
                    String deviceId = "00000000-2b17-f0eb-0000-00001ef377b9";
                    String deviceName = "Samsung Infuse";

                    _obj.ObjectModel.Content = deviceName;

                    ScatterViewItem[] ls = _obj.createFileList(
                                            HttpClient.getIndexObject(deviceId, "/mnt/sdcard"), 
                                            deviceName);
                    foreach (ScatterViewItem i in ls)
                    {
                        if (i != null)
                        {
                            this.DirList.Items.Add(i);
                        }
                    }

                    break;
                default:
                    _obj.ObjectModel.Content = "UNKNOWN MODEL";
                    //_obj.objectWrapper.Fill = SurfaceColors.ControlAccentBrush;
                    break;
            }
        }

        private void OnVisualizationRemoved(object sender, TagVisualizerEventArgs e) 
        {
            SlidboardView _obj = (SlidboardView)e.TagVisualization;
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
