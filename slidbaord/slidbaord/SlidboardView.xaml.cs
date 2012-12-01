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
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.Collections;
using System.Threading;
using System.Windows.Threading;

namespace slidboard
{
    /// <summary>
    /// Veiw components. 
    /// contains dynamically generated view elements
    /// </summary>
    public partial class SlidboardView : TagVisualization
    {

        private String DEVICE_ID = "";

        private const String PLAYING = "PLAYING";
        private const String STOPPED = "STOPPED";

        public SlidboardView()
        {
            InitializeComponent();
        }

        private void ObjectVisualization_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO: customize ObjectVisualization's UI based on this.VisualizedTag here
            
        }

        /// <summary>
        ///  Factory method. Creates ScatterViewItemViews of items out from given ls
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="deviceName"></param>
        /// <param name="startingPosition"></param>
        /// <returns></returns>
        public ScatterViewItem[] createFileList(ArrayList ls, String deviceName)
        {
            ls.TrimToSize();
            ScatterViewItem[] items = new ScatterViewItem[ls.Count];
            for (int i = 0; i < ls.Count; i++)
            {
                IndexObject item = ls[i] as IndexObject;
                ScatterViewItem temp = this.getItemView(item, deviceName);
                if (temp != null)
                {
                    items[i] = temp;
                }
            }
                return items;
        }

        private double yOffset = 0;
        private double xOffset = -1;

        private ScatterViewItem getItemView(IndexObject indexedItem, String deviceName)
        {

            if (indexedItem == null)
            {
                return null;
            }

            if (xOffset == -1)
            {
                this.xOffset = this.Center.X;
            }

            DEVICE_ID = DEVICE_ID.Equals("") ? indexedItem.deviceId : DEVICE_ID;

            ScatterViewItem item = new ScatterViewItem();
            //Set properties

            Console.WriteLine(this.Orientation);
            
            //Orientation should be either 0 or 180 depending on the device's orientation

            //[0,90] || [270,360]
            if ((0 <= this.Orientation && this.Orientation <= 90) || (270 <= this.Orientation && this.Orientation <= 360))
            {
             item.Orientation = 0;
                Point deviceLocation = new Point(xOffset + 350, this.Center.Y - 200 + yOffset);
                yOffset += 30;
                if (yOffset > 400)
                {
                    //reset
                    yOffset = 0;
                    xOffset += 100;
                }
                //Folder card displayed according to the location of the device
                item.Center = deviceLocation;
            }
            else
            {
                item.Orientation = 180;
                Point deviceLocation = new Point(xOffset - 350, this.Center.Y + 200 + yOffset);
                yOffset += 30;
                if (yOffset > 400)
                {
                    //reset
                    yOffset = 0;
                    xOffset += 100;
                }
                //Folder card displayed according to the location of the device
                item.Center = deviceLocation;
            }

            item.MinWidth = 250;
            item.MinHeight = 120;
            item.HorizontalContentAlignment = HorizontalAlignment.Center;
            item.VerticalContentAlignment = VerticalAlignment.Top;
            item.Background = Brushes.Transparent;
            item.Foreground = Brushes.White;
            item.FontWeight = FontWeights.UltraBold;
            
            //Add grid view for sub-directory list
            Grid subDirectoryList = new Grid();
            subDirectoryList.ColumnDefinitions.Add(new ColumnDefinition());
            subDirectoryList.RowDefinitions.Add(new RowDefinition());
            subDirectoryList.Name = "subDirectoryList";

            Label device_name = new Label();
            device_name.Name = "deviceName";
            device_name.Content = deviceName;
            device_name.HorizontalContentAlignment = HorizontalAlignment.Center;
            device_name.VerticalContentAlignment = VerticalAlignment.Center;
            device_name.Height = 40;

            Label path = new Label();
            path.Name = "path";
            path.Content = indexedItem.fullPath;
            path.MaxHeight = 0;
            path.Visibility = Visibility.Collapsed;

            Label deviceId = new Label();
            deviceId.Name = "deviceId";
            deviceId.Content = DEVICE_ID;
            deviceId.HorizontalContentAlignment = HorizontalAlignment.Center;
            deviceId.VerticalContentAlignment = VerticalAlignment.Center;
            deviceId.MaxHeight = 0;
            deviceId.Visibility = Visibility.Collapsed;

            SurfaceButton sb = new SurfaceButton();
            //Style the buttons
            sb.Name = "button";
            sb.MinHeight = 30;
            sb.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            sb.VerticalContentAlignment = VerticalAlignment.Center;
            sb.Background = Brushes.DodgerBlue;
            sb.FontWeight = FontWeights.UltraBold;
            sb.Foreground = Brushes.WhiteSmoke;

            if (indexedItem.type.Equals("DIR"))
            {
                sb.Content = indexedItem.parent + "/" + indexedItem.name + "      >>";
            }
            else
            {
                //sb.Content = "File : " + indexedItem.parent + "/" + indexedItem.name;
                return null;
            }

            sb.Click += new RoutedEventHandler(this.open);

            //Grid view to hold all the UI elements inside scatterViewitem
            Grid grid = new Grid();

            grid.ColumnDefinitions.Add(new ColumnDefinition());
            RowDefinition deviceNameRow = new RowDefinition();
            deviceNameRow.MaxHeight = 0;
            grid.RowDefinitions.Add(deviceNameRow);

            //Row def for button
            RowDefinition button_def = new RowDefinition();
            button_def.MaxHeight = 35;
            grid.RowDefinitions.Add(button_def);

            //Row def for subdirectory list
            grid.RowDefinitions.Add(new RowDefinition());
            RowDefinition hiddenRow = new RowDefinition();
            hiddenRow.MaxHeight = 0;
            RowDefinition hiddenRow1 = new RowDefinition();
            hiddenRow1.MaxHeight = 0;

            grid.RowDefinitions.Add(hiddenRow);
            grid.RowDefinitions.Add(hiddenRow1);

            Grid.SetColumn(device_name, 0);
            Grid.SetRow(device_name, 0);
            Grid.SetColumn(sb, 0);
            Grid.SetRow(sb, 1);
            Grid.SetColumn(subDirectoryList, 0);
            Grid.SetRow(subDirectoryList, 2);
            Grid.SetColumn(path, 0);
            Grid.SetRow(path, 3);
            Grid.SetColumn(deviceId, 0);
            Grid.SetRow(deviceId, 4);
            
            grid.Children.Add(device_name);
            grid.Children.Add(subDirectoryList);
            grid.Children.Add(sb);
            grid.Children.Add(path);
            grid.Children.Add(deviceId);

            item.Content = grid;

            return item;
        }

        /// <summary>
        /// Event handler for opening up a directory or a file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void open(object sender, EventArgs e)
        {
            SurfaceButton sb = (SurfaceButton)sender;
            Grid item = (Grid)sb.Parent;

            UIElementCollection collection = item.Children;
            String path = "";
            Grid subDirList = null;

            //Get the properties of the event sender
            foreach (UIElement c in collection)
            {
                String name = c.GetValue(Control.NameProperty).ToString();
                if (name.Equals("path"))
                {
                    path = ((Label)c).Content.ToString();
                }
                if (name.Equals("deviceId"))
                {
                    DEVICE_ID = DEVICE_ID.Equals("") ? ((Label)c).Content.ToString() : DEVICE_ID;
                }
                if (name.Equals("subDirectoryList"))
                {
                    subDirList = ((Grid)c);
                }
            }

            ArrayList items = HttpClient.getIndexObject(DEVICE_ID, path);

            items.TrimToSize();
            int i = 0;

            //This could be null
            if (items.Count == 1)
            {
                if (((IndexObject)items[0]) == null)
                {
                    items.RemoveAt(0);
                }
            }

            if (items.Count > 0)
            {
                foreach (IndexObject io in items)
                {
                    if (io != null)
                    {
                        if (io.type.Equals("DIR"))
                        {
                            SurfaceButton button = new SurfaceButton();
                            //Style the buttons
                            button.Name = "button";
                            button.Height = 25;
                            button.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                            button.VerticalContentAlignment = VerticalAlignment.Center;
                            button.Background = Brushes.SteelBlue;
                            button.FontWeight = FontWeights.UltraBold;
                            button.Foreground = Brushes.WhiteSmoke;
                            button.Content = io.fullPath;
                            button.Click += new RoutedEventHandler(NewFolderCard);

                            Grid.SetColumn(button, 0);
                            Grid.SetRow(button, i++);
                            subDirList.RowDefinitions.Add(new RowDefinition());

                            subDirList.Children.Add(button);
                        }
                        else if (io.type.Equals("FILE"))
                        {
                            SurfaceButton button = new SurfaceButton();
                            //Style the buttons
                            button.Name = "button";
                            button.Height = 25;
                            button.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                            button.VerticalContentAlignment = VerticalAlignment.Center;
                            button.Background = Brushes.SteelBlue;
                            button.FontWeight = FontWeights.UltraBold;
                            button.Foreground = Brushes.WhiteSmoke;
                            button.Content = io.fullPath;
                            button.Click += new RoutedEventHandler(OpenFile);

                            Grid.SetColumn(button, 0);
                            Grid.SetRow(button, i++);
                            subDirList.RowDefinitions.Add(new RowDefinition());

                            subDirList.Children.Add(button);
                        }
                    }
                }
            }
            else
            {
                Label label = new Label();
                label.Name = "path";
                label.Content = "Empty";
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.VerticalContentAlignment = VerticalAlignment.Center;

                subDirList.Children.Add(label);
            }
        }

        /// <summary>
        /// Creates a new Folder Card and append to the main view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewFolderCard(object sender, EventArgs e)
        {
            SurfaceButton sb = (SurfaceButton)sender;

            //Get path for query
            String path = sb.Content.ToString();

            //Subdirectory list
            Grid item = (Grid)sb.Parent;

            //Directory card. Root for any given subdirectory
            Grid topDirectoryCard = (Grid)item.Parent;
            UIElementCollection collection = topDirectoryCard.Children;
            String deviceName = "";

            //Get the device id of the querying item
            foreach (UIElement c in collection)
            {
                String name = c.GetValue(Control.NameProperty).ToString();
                if (name.Equals("deviceId"))
                {
                    DEVICE_ID = DEVICE_ID.Equals("") ? ((Label)c).Content.ToString() : DEVICE_ID;
                }
                if (name.Equals("deviceName"))
                {
                    deviceName = ((Label)c).Content.ToString();
                }
            }

            ArrayList response = HttpClient.getIndexObject(DEVICE_ID, path);

            if (((IndexObject)response[0]) == null)
            {
                String content_temp = (String)sb.Content;
                sb.Content = "EMPTY Folder";
                return;
            }

            foreach (ScatterViewItem i in this.createFileList(response, deviceName))
            {
                SurfaceWindow1.GlobalDirList.Items.Add(i);
            }
        }

        private void OpenFile(object sender, EventArgs e)
        {
            SurfaceButton sb = (SurfaceButton)sender;

            String originalFileFullPath = sb.Content.ToString();

            //Create a ScatterViewItem to pass on to the worker thread.
            ScatterViewItem dynamicItem = new ScatterViewItem();
            dynamicItem.HorizontalContentAlignment = HorizontalAlignment.Center;
            dynamicItem.VerticalContentAlignment = VerticalAlignment.Center;
            dynamicItem.Background = Brushes.Transparent;
            dynamicItem.Foreground = Brushes.White;
            dynamicItem.FontWeight = FontWeights.UltraBold;
            dynamicItem.Orientation = this.Orientation;

            //tell the user that the content is loading in the background
            Label loading = new Label();
            loading.Content = "Loading...";
            dynamicItem.Content = loading;
            SurfaceWindow1.GlobalDirList.Items.Add(dynamicItem);

            FileFetcher fetcher = new FileFetcher(DEVICE_ID, dynamicItem, originalFileFullPath, this);
            Thread workerThread = new Thread(fetcher.fetchfile);
            workerThread.SetApartmentState(ApartmentState.STA);
            workerThread.Start();
        }

        public void toggleMedia(object sender, EventArgs e)
        {
            Grid grid = (Grid)(((SurfaceButton)sender).Parent);
            UIElementCollection collection = grid.Children;

            MediaElement temp;
            Boolean isPlaying = true;

            foreach (UIElement c in collection) 
            {
                if (c.GetType().Equals((new MediaElement()).GetType())) 
                {
                    String name = c.GetValue(Control.NameProperty).ToString();
                    temp = ((MediaElement)c);

                    if (name.Equals(PLAYING))
                    {
                        temp.Stop();
                        temp.Name = STOPPED;
                        isPlaying = false;
                    }
                    else
                    {
                        temp.Play();
                        temp.Name = PLAYING;
                        isPlaying = true;
                    }
                }
            }

            foreach (UIElement c in collection)
            {
                if (c.GetType().Equals((new SurfaceButton()).GetType()))
                {
                    String name = c.GetValue(Control.NameProperty).ToString();
                    if (name.Equals("status")) {
                        if (isPlaying)
                        {
                            ((SurfaceButton)c).Content = PLAYING;
                        }
                        else
                        {
                            ((SurfaceButton)c).Content = STOPPED;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Background file fetcher. 
    /// Located as a seperated module for readability purpose
    /// </summary>
    public class FileFetcher
    {
        private volatile ScatterViewItem item;
        private volatile String originalFileFullPath;
        private volatile String DEVICE_ID;
        private volatile SlidboardView view;

        private const String PLAYING = "PLAYING";
        private const String STOPPED = "STOPPED";

        public FileFetcher(String deviceId, ScatterViewItem item, String originalFileFullPath, 
            SlidboardView view)
        {
            this.DEVICE_ID = deviceId;
            this.item = item;
            this.originalFileFullPath = originalFileFullPath;
            this.view = view;
        }

        public void fetchfile()
        {
            //uri is the path to the tmp storage on the current machine
            //Warning : the file name is a GUID to avoid collision.
            //For original file name, use originalFileFullPath
            String uri = HttpClient.getFile(DEVICE_ID, originalFileFullPath);

            if (uri.Equals("DNF")) 
            {
                
                //Connectivity problem
                //Update the UI in the main thread
                Action action = delegate
                {
                    Label err = new Label();
                    err.Content = "Please check the connection";
                    item.Content = err;
                };
                view.Dispatcher.Invoke(action);
                return;
            }

            int extLength = uri.Length - uri.LastIndexOf(".");
            String fileExt = uri.Substring(uri.LastIndexOf("."), extLength);

            //mp3 file. play
            if (fileExt.Equals(".mp3"))
            {
                //Update the UI in the main thread
                Action action = delegate
                {
                    Grid grid = new Grid();
                    grid.Background = Brushes.Transparent;

                    //Holder for file name
                    grid.RowDefinitions.Add(new RowDefinition());
                    //Holder for MediaElement
                    grid.RowDefinitions.Add(new RowDefinition());
                    //Holder for MediaElement status
                    grid.RowDefinitions.Add(new RowDefinition());
                    //The grid is going to be single columned
                    grid.ColumnDefinitions.Add(new ColumnDefinition());

                    //File name label
                    Label fileName = new Label();
                    fileName.Name = "name";
                    fileName.Content = originalFileFullPath.Split('/')
                        [originalFileFullPath.Split('/').Length - 1].ToUpper();
                    fileName.FontWeight = FontWeights.UltraBold;
                    fileName.Background = Brushes.Transparent;
                    fileName.Foreground = Brushes.White;

                    //media element to play audio files
                    MediaElement media = new MediaElement();
                    media.Name = PLAYING;
                    media.LoadedBehavior = MediaState.Manual;
                    media.Source = new Uri(uri, UriKind.Absolute);
                    media.Play();

                    //MediaElement status
                    SurfaceButton status = new SurfaceButton();
                    status.Name = "status";
                    status.Foreground = Brushes.White;
                    status.FontWeight = FontWeights.UltraBold;
                    status.Background = Brushes.Transparent;
                    status.VerticalContentAlignment = VerticalAlignment.Center;
                    status.HorizontalContentAlignment = HorizontalAlignment.Center;
                    status.Content = PLAYING;
                    status.Click += new RoutedEventHandler(view.toggleMedia);

                    Grid.SetRow(fileName, 0);
                    Grid.SetColumn(fileName, 0);
                    Grid.SetRow(media, 1);
                    Grid.SetColumn(media, 0);
                    Grid.SetRow(status, 2);
                    Grid.SetColumn(status, 0);

                    grid.Children.Add(fileName);
                    grid.Children.Add(media);
                    grid.Children.Add(status);

                    item.Content = grid;
                };
                view.Dispatcher.Invoke(action);
            }
            else
            {
                //Assume its an image file
                //Update the UI in the main thread
                Action action = delegate {
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri(uri, UriKind.Absolute));
                    item.Content = img;
                };
                view.Dispatcher.Invoke(action);
            }
        }
    }
}
