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

namespace slidbaord
{
    /// <summary>
    /// Veiw components for ObjectVisualization.xaml
    /// </summary>
    public partial class ObjectVisualization : TagVisualization
    {

        private String DEVICE_ID = "";

        public ObjectVisualization()
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

        private ScatterViewItem getItemView(IndexObject indexedItem, String deviceName)
        {

            if (indexedItem == null)
            {
                return null;
            }

            DEVICE_ID = DEVICE_ID.Equals("") ? indexedItem.deviceId : DEVICE_ID;

            ScatterViewItem item = new ScatterViewItem();
            //Set properties
            //item.Center = startingPosition;
            item.MinWidth = 250;
            //item.MaxWidth = 600;
            item.MinHeight = 120;
            
            /*
            if (indexedItem.name.Substring(0,1).Equals("."))
            {
                item.Visibility = Visibility.Collapsed;
            }
            */

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

            Console.WriteLine(indexedItem.type);

            if (indexedItem.type.Equals("DIR"))
            {
                sb.Content = indexedItem.parent + "/" + indexedItem.name + "      >>";
            }
            else
            {
                sb.Content = "File : " + indexedItem.parent + "/" + indexedItem.name;
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
            if (items.Count > 1)
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
                sb.Content = "EMPTY Folder : " + content_temp;
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

            String response = HttpClient.getFile(DEVICE_ID, sb.Content.ToString());
            Console.WriteLine(response);
        }
    }
}
