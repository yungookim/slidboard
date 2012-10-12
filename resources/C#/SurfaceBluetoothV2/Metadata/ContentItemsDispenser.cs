using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoftwareLab.Sys.BusinessObjects;
using System.IO;

namespace SurfaceBluetoothV2.Metadata
{
    public class ContentItemsDispenser
    {
        private const string RESOURCE_FILES = "ContentenItems";

        

        public static BusinessCollection<ContentItem> GetData()
        {
            string filesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RESOURCE_FILES);

            BusinessCollection<ContentItem> retVal = new BusinessCollection<ContentItem>();

            ContactItem contact1 = new ContactItem("John", "Doe", "338998872323", "john.doe@noname.com", Path.Combine(filesPath, "contact1.png"));
            ContactItem contact2 = new ContactItem("Richard", "Roe", "338998872323", "richard.roe@noname.com", Path.Combine(filesPath, "contact2.png"));
            ContactItem contact3 = new ContactItem("Jane", "Doe", "338998872323", "jane.doe@noname.com", Path.Combine(filesPath, "contact3.png"));
            ContactItem contact4 = new ContactItem("Richelle", "Roe", "338998872323", "richelle.doe@noname.com", Path.Combine(filesPath, "contact4.png"));

            ContentItem item1 = new ContentItem(Path.Combine(filesPath, "image1.jpg"), Path.Combine(filesPath, "image1.jpg"), ContentItem.CONTENT_IMAGE);
            ContentItem item2 = new ContentItem(Path.Combine(filesPath, "image2.jpg"), Path.Combine(filesPath, "image2.jpg"), ContentItem.CONTENT_IMAGE);
            ContentItem item3 = new ContentItem(Path.Combine(filesPath, "image3.jpg"), Path.Combine(filesPath, "image3.jpg"), ContentItem.CONTENT_IMAGE);
            ContentItem item5 = new ContentItem(Path.Combine(filesPath, "BeanOneAlbumArt.png"), Path.Combine(filesPath, "BeanOne.mp3"), ContentItem.CONTENT_RING);
            ContentItem item6 = new ContentItem(Path.Combine(filesPath, "SlumberJackAlbumArt.png"), Path.Combine(filesPath, "SlumberJack.mp3"), ContentItem.CONTENT_RING);
            ContentItem item7 = new ContentItem(Path.Combine(filesPath, "contact1.png"), Path.Combine(filesPath, "contact1.png"), ContentItem.CONTENT_CONTACT, contact1);
            ContentItem item8 = new ContentItem(Path.Combine(filesPath, "contact2.png"), Path.Combine(filesPath, "contact2.png"), ContentItem.CONTENT_CONTACT, contact2);
            ContentItem item9 = new ContentItem(Path.Combine(filesPath, "contact3.png"), Path.Combine(filesPath, "contact3.png"), ContentItem.CONTENT_CONTACT, contact3);
            ContentItem item10 = new ContentItem(Path.Combine(filesPath, "contact4.png"), Path.Combine(filesPath, "contact4.png"), ContentItem.CONTENT_CONTACT, contact4);
            retVal.Add(item1);
            retVal.Add(item2);
            retVal.Add(item3);
            retVal.Add(item5);
            retVal.Add(item6);
            retVal.Add(item7);
            retVal.Add(item8);
            retVal.Add(item9);
            retVal.Add(item10);

            return retVal;
        }
    }
}
