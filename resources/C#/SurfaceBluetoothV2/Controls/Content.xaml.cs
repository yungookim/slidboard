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
using Microsoft.Surface.Presentation.Controls;
using SurfaceBluetoothV2.Metadata;

namespace SurfaceBluetoothV2.Controls
{
	public partial class Content : UserControl
	{
        public Content()
		{
			this.InitializeComponent();
            this.DataContextChanged += new DependencyPropertyChangedEventHandler(Content_DataContextChanged);
            this.Unloaded += new RoutedEventHandler(Content_Unloaded);
		}


        #region uc events
        void Content_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext != null && DataContext is ContentItem)
            {
                if (((ContentItem)DataContext).MediaType == ContentItem.CONTENT_RING)
                    MediaControls.Visibility = System.Windows.Visibility.Visible;
                else
                    MediaControls.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        void Content_Unloaded(object sender, RoutedEventArgs e)
        {
            Player.Pause();
        }
        #endregion


        #region media player events

        private void OnPauseButtonPressed(object sender, RoutedEventArgs e)
        {
            Player.Pause();
            PlayButton.Visibility = System.Windows.Visibility.Visible;
            PauseButton.Visibility = System.Windows.Visibility.Hidden;
        }

        private void OnPlayButtonPressed(object sender, RoutedEventArgs e)
        {
            Player.Play();
            PlayButton.Visibility = System.Windows.Visibility.Hidden;
            PauseButton.Visibility = System.Windows.Visibility.Visible;
        }

        private void OnMediaEnded(object sender, RoutedEventArgs e)
        {

        }
        #endregion
	}
}