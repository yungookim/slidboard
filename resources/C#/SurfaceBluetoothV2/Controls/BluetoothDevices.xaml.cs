using System;
using System.Collections.Generic;
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
using System.Windows.Threading;
using System.Collections.ObjectModel;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using Microsoft.Surface.Presentation;
using InTheHand.Net;
using SurfaceBluetoothV2.Metadata;
using System.IO;

namespace SurfaceBluetoothV2.Controls
{
    /// <summary>
    /// Interaction logic for BluetoothDevices.xaml
    /// </summary>
    public partial class BluetoothDevices : UserControl
    {
        
        #region private and public properties
        private DispatcherTimer BlueTimer;
        private ObservableCollection<BluetoothDeviceInfo> _bluetoothDeviceList;
        public ObservableCollection<BluetoothDeviceInfo> BluetoothDeviceList
        {
            set
            {
                _bluetoothDeviceList = value;
            }

            get { return _bluetoothDeviceList; }
        }
        #endregion

        public BluetoothDevices()
        {
            this.InitializeComponent();
            _bluetoothDeviceList = new ObservableCollection<BluetoothDeviceInfo>();
            DataContext = this;

            BlueTimer = new DispatcherTimer(DispatcherPriority.Background);
            BlueTimer.Interval = TimeSpan.FromSeconds(5);
            BlueTimer.Tick += new EventHandler(BlueTimer_Tick);
            BlueTimer.Start();
        }

        void BlueTimer_Tick(object sender, EventArgs e)
        {
            BlueTimer.Stop();
            using (BluetoothComponent bt = new BluetoothComponent())
            {
                bt.DiscoverDevicesComplete += new EventHandler<DiscoverDevicesEventArgs>(bt_DiscoverDevicesComplete);
                bt.DiscoverDevicesAsync(100, false, false, false, true, null);
            }
        }

        void bt_DiscoverDevicesComplete(object sender, DiscoverDevicesEventArgs e)
        {
            _bluetoothDeviceList.Clear();

            foreach (BluetoothDeviceInfo device in e.Devices)
            {
                _bluetoothDeviceList.Add(device);
            }
            BlueTimer.Start();
        }


        private void OnDrop(object sender, SurfaceDragDropEventArgs e)
        {
            FrameworkElement element = e.OriginalSource as FrameworkElement;
            if (element != null)
            {
                ContentItem content = e.Cursor.Data as ContentItem;
                // ensuring that target is a Bluetooth device and data is ContentItem
                if (element.DataContext is BluetoothDeviceInfo && content != null)
                {
                    BluetoothDeviceInfo device = element.DataContext as BluetoothDeviceInfo;

                    // Pause discovery as it interferes with/slows down beam process
                    BlueTimer.Stop();

                    // Create the new request and write the contact details
                    //ObexWebRequest owr = new ObexWebRequest(new Uri("obex://" + device.DeviceAddress.ToString() + "/" + oi.FileName));

                    ObexWebRequest owr = new ObexWebRequest(new Uri(String.Format("obex://{0}/{1}",  device.DeviceAddress, content.ObexFileName)));
                    using (System.IO.Stream s = owr.GetRequestStream())
                    {
                        StreamHelper.WriteToStream(s, content);

                        owr.ContentType = content.ObexContentType;
                        owr.ContentLength = s.Length;
                        s.Close();
                    }

                    // Beam the item on new thread
                    System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(BeamObject), owr);
                }
            }

        }

        // Runs in a threadpool thread and performs the actual obex exchange
        private void BeamObject(object context)
        {
            ObexWebRequest owr = context as ObexWebRequest;

            try
            {
                InTheHand.Net.ObexWebResponse response = (InTheHand.Net.ObexWebResponse)owr.GetResponse();

                // Remove once-off pairing
                BluetoothSecurity.RemoveDevice(BluetoothAddress.Parse(owr.RequestUri.Host));
            }
            catch (System.Net.WebException we)
            {
                System.Diagnostics.Debug.WriteLine(we.ToString());
            }
            finally
            {
                // Restart discovery for new devices
                BlueTimer.Start();
            }
        }

        private void StackPanel_DragEnter(object sender, SurfaceDragDropEventArgs e)
        {
            // prevent scatterview items that aren't ContentItem to be dropped
            // http://msdn.microsoft.com/en-us/library/ff727837.aspx
            if (e.Cursor.Data is ContentItem == false)
            {
                e.Effects = DragDropEffects.None;
            }
        }
    }
}