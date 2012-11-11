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
            //item.Center = startingPosition;

            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            Label device_name = new Label();
            device_name.Content = deviceName;
            device_name.HorizontalContentAlignment = HorizontalAlignment.Center;
            device_name.VerticalContentAlignment = VerticalAlignment.Center;

            SurfaceButton sb = new SurfaceButton();
            //Style the buttons
            sb.Height = 25;
            sb.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            sb.VerticalContentAlignment = VerticalAlignment.Center;
            sb.Background = Brushes.DodgerBlue;
            sb.FontWeight = FontWeights.UltraBold;
            sb.Foreground = Brushes.WhiteSmoke;

            if (indexedItem.type.Equals("DIR"))
            {
                sb.Content = indexedItem.parent + "/" + indexedItem.name + "      >>";
                item.MaxWidth = 500;
                item.MaxHeight = 120;
            }
            else
            {
                sb.Content = indexedItem.parent + "/" + indexedItem.name;
            }

            Grid.SetColumn(device_name, 0);
            Grid.SetRow(device_name, 0);
            Grid.SetColumn(sb, 0);
            Grid.SetRow(sb, 1);

            //sb.Click += new RoutedEventHandler(blah);
            grid.Children.Add(device_name);
            grid.Children.Add(sb);

            item.Content = grid;

            return item;
        }
    }
}
