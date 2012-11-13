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
    /// Interaction logic for ObjectVisualization.xaml
    /// </summary>
    public partial class ObjectVisualization : TagVisualization
    {   

        

        public ObjectVisualization()
        {
            InitializeComponent();
        }

        private void ObjectVisualization_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO: customize ObjectVisualization's UI based on this.VisualizedTag here

        }


        /// <summary>
        /// Factory method. Creates ListBoxItem view of items out from given ls 
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public ScatterViewItem[] createFileList(ArrayList ls, String deviceName, Point startingPosition)
        {
            ls.TrimToSize();
            ScatterViewItem[] items = new ScatterViewItem[ls.Count];
            for (int i = 0; i < ls.Count; i++)
            {
                IndexObject item = ls[i] as IndexObject;
                items[i] = this.getItemView(item, deviceName, startingPosition);
            }
                return items;
        }

        private ScatterViewItem getItemView(IndexObject indexedItem, 
            String deviceName, Point startingPosition)
        {
            ScatterViewItem item = new ScatterViewItem();
            //Set properties
            item.Center = startingPosition;
            item.MinWidth = 250;
            item.MinHeight = 120;
            if (indexedItem.name.Substring(0,1).Equals("."))
            {
                item.Visibility = Visibility.Collapsed;
            }

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
            deviceId.Content = indexedItem.deviceId;
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
                sb.Content = indexedItem.parent + "/" + indexedItem.name;
            }

            sb.Click += new RoutedEventHandler(this.open);

            //Grid view to hold all the UI elements inside scatterViewitem
            Grid grid = new Grid();

            grid.ColumnDefinitions.Add(new ColumnDefinition());
            RowDefinition deviceNameRow = new RowDefinition();
            deviceNameRow.MaxHeight = 35;
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

        private void open(object sender, EventArgs e)
        {
            
            SurfaceButton sb = (SurfaceButton)sender;
            Grid item = (Grid)sb.Parent;
            //((Label)item.FindName("entityId")).GetValue(Label.NameProperty);

            UIElementCollection collection = item.Children;
            String path = "", deviceId = "";
            Grid subDirList = null;

            foreach (UIElement c in collection)
            {
                String name = c.GetValue(Control.NameProperty).ToString();
                if (name.Equals("path"))
                {
                    path = ((Label)c).Content.ToString();
                }
                if (name.Equals("deviceId"))
                {
                    deviceId = ((Label)c).Content.ToString();
                }
                if (name.Equals("subDirectoryList"))
                {
                    subDirList = ((Grid)c);
                }    
            }
            
            ArrayList items = HttpClient.getIndexObject(deviceId, path);
            items.TrimToSize();
            int i = 0;
            if (items.Count > 1)
            {
                foreach (IndexObject io in items)
                {
                    if (io != null)
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

                        Grid.SetColumn(button, 0);
                        Grid.SetRow(button, i++);

                        subDirList.RowDefinitions.Add(new RowDefinition());
                        subDirList.Children.Add(button);
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
    }
}
