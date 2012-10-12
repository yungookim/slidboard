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
using SoftwareLab.Sys.BusinessObjects;
using SurfaceBluetoothV2.Metadata;
using System.ComponentModel;

namespace SurfaceBluetoothV2.Controls
{
	/// <summary>
	/// Interaction logic for LibraryContents.xaml
	/// </summary>
	public partial class LibraryContents : UserControl
	{
        private BusinessCollection<ContentItem> presentationCollection = new BusinessCollection<ContentItem>();
		public LibraryContents()
		{
			this.InitializeComponent();

            presentationCollection = ContentItemsDispenser.GetData();
            LibraryItems.DataContext = "libraryContainer1";
            ICollectionView view1 = CollectionViewSource.GetDefaultView(presentationCollection);
            view1.GroupDescriptions.Add(new PropertyGroupDescription("MediaType"));
            LibraryItems.ItemsSource = view1;
		}

        private void LibraryItems_DragEnter(object sender, Microsoft.Surface.Presentation.SurfaceDragDropEventArgs e)
        {
            // prevent scatterview items that aren't ContentItem to be dropped
            // http://msdn.microsoft.com/en-us/library/ff727837.aspx
            if (e.Cursor.Data is ContentItem == false)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        public void SetIsItemDataEnabled(ContentItem content)
        {
            LibraryItems.SetIsItemDataEnabled(content, true);
        }
	}
}