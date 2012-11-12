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
                item.Visibility = Visibility.Hidden;
            }

            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            Label device_name = new Label();
            device_name.Name = "deviceName";
            device_name.Content = deviceName;
            device_name.HorizontalContentAlignment = HorizontalAlignment.Center;
            device_name.VerticalContentAlignment = VerticalAlignment.Center;

            Label entityId = new Label();
            entityId.Name = "entityId";
            entityId.Content = indexedItem.id;
            entityId.HorizontalContentAlignment = HorizontalAlignment.Center;
            entityId.VerticalContentAlignment = VerticalAlignment.Center;
            entityId.Visibility = Visibility.Hidden;

            SurfaceButton sb = new SurfaceButton();
            //Style the buttons
            sb.Name = "button";
            sb.Height = 25;
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

            Grid.SetColumn(device_name, 0);
            Grid.SetRow(device_name, 0);
            Grid.SetColumn(sb, 0);
            Grid.SetRow(sb, 1);
            Grid.SetColumn(entityId, 0);
            Grid.SetRow(entityId, 2);
            
            grid.Children.Add(device_name);
            grid.Children.Add(sb);
            grid.Children.Add(entityId);

            item.Content = grid;

            return item;
        }

        private void open(object sender, EventArgs e)
        {
            
            SurfaceButton sb = (SurfaceButton)sender;
            Grid item = (Grid)sb.Parent;
            //((Label)item.FindName("entityId")).GetValue(Label.NameProperty);

            UIElementCollection collection = item.Children;

            foreach (UIElement c in collection)
            {
                String name = c.GetValue(Control.NameProperty).ToString();
                if (name.Equals("entityId"))
                {
                    String uuid = ((Label)c).Content.ToString();

                    Console.WriteLine(uuid);
                }
            }
        }
    }
}
