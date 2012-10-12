using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;

namespace SurfaceBluetoothV2.Metadata
{
    public class StreamHelper
    {
        public static void WriteToStream(Stream stream, ContentItem item)
        {
            switch (item.MediaType)
            { 
                case ContentItem.CONTENT_IMAGE:
                    WriteImageToStream(stream, item);
                    break;
                case ContentItem.CONTENT_CONTACT:
                    WriteContactToStream(stream, item);
                    break;
                case ContentItem.CONTENT_RING:
                    WriteAudioToStream(stream, item);
                    break;
            }
        }

        private static void WriteImageToStream(Stream stream, ContentItem item)
        {
            JpegBitmapEncoder jbe = new JpegBitmapEncoder();
            BitmapFrame bf = BitmapFrame.Create(new Uri(item.Media));
            // Add single frame to encoder
            jbe.Frames.Add(bf);
            // Save JPEG image to our memory buffer
            jbe.Save(stream);
        }

        private static void WriteAudioToStream(Stream stream, ContentItem item)
        {
            using (FileStream fs = new FileStream(item.Media, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[4092];
                int bytesRead = fs.Read(buffer, 0, buffer.Length);
                while (bytesRead > 0)
                {
                    stream.Write(buffer, 0, bytesRead);
                    bytesRead = fs.Read(buffer, 0, buffer.Length);
                }
            }
        }

        /// <summary>
        /// Writes a vCard to the specified stream using the stored contact properties.
        /// </summary>
        /// <param name="s">A writable stream.</param>
        public static void WriteContactToStream(System.IO.Stream s, ContentItem item)
        {
            // Write a basic vcard item to the stream (in this sample it will be an ObexWebRequest but it's just bytes so any stream will do)
            // Could write to a filestream for example
            System.IO.StreamWriter sw = new System.IO.StreamWriter(s, System.Text.Encoding.ASCII);
            sw.NewLine = "\r\n";
            // This tag indicates the start of a vCard item - after the content is added an END:VCARD must follow
            sw.WriteLine("BEGIN:VCARD");
            // Version 2.1 of the vCard spec is widely used and our contact types are quite simple
            sw.WriteLine("VERSION:2.1");

            if (!string.IsNullOrEmpty(item.Contact.FirstName) || !string.IsNullOrEmpty(item.Contact.LastName))
            {
                // Display name "Firstname Lastname"
                sw.WriteLine("FN:" + (string.IsNullOrEmpty(item.Contact.FirstName) ? "" : item.Contact.FirstName) + " " + (string.IsNullOrEmpty(item.Contact.LastName) ? "" : item.Contact.LastName));
                // Full name is "Lastname;Firstname"
                sw.WriteLine("N:" + (string.IsNullOrEmpty(item.Contact.LastName) ? "" : item.Contact.LastName) + ";" + (string.IsNullOrEmpty(item.Contact.FirstName) ? "" : item.Contact.FirstName));
            }
            if (!string.IsNullOrEmpty(item.Contact.MobileTelephoneNumber))
            {
                // Telephone of sub-type cellular, voice
                sw.WriteLine("TEL;CELL;VOICE:" + item.Contact.MobileTelephoneNumber);
            }
            if (!string.IsNullOrEmpty(item.Contact.EmailAddress))
            {
                // Marked as the preferred email even though we are only storing one
                sw.WriteLine("EMAIL;PREF;INTERNET:" + item.Contact.EmailAddress);
            }
            if (item.Contact.PictureUri != null)
            {
                // Always generate a jpeg from the bitmap and encode as base64 string
                sw.WriteLine("PHOTO;TYPE=JPEG;ENCODING=BASE64:");

                // Here we process the picture
                // MemoryStream provides a resizable buffer which we can write the image to
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                // Using JPEG encoder
                JpegBitmapEncoder jbe = new JpegBitmapEncoder();

                BitmapFrame bf = BitmapFrame.Create(new Uri(item.Contact.PictureUri));
                // Add single frame to encoder
                jbe.Frames.Add(bf);
                // Save JPEG image to our memory buffer
                jbe.Save(ms);

                // Access the raw bytes of the JPEG
                byte[] rawImage = ms.ToArray();
                // In a Base64 string each 3 bytes is converted to 4 characters so create a new buffer to store the Base64 version
                char[] b64Image = new char[(int)(Math.Ceiling(rawImage.Length / 3d) * 4d)];
                // Convert to the Base64 characters in our new buffer
                Convert.ToBase64CharArray(rawImage, 0, rawImage.Length, b64Image, 0);

                // Loop through the buffer to write each 76 characters to a new line
                for (int i = 0; i < b64Image.Length; i += 76)
                {
                    int charsLeft = b64Image.Length - (i);
                    // For vCard the line must be preceded with a space
                    sw.WriteLine(" " + new string(b64Image, i, charsLeft > 76 ? 76 : charsLeft));
                }
                // Must be followed by a blank line to indicate the end of the image
                sw.WriteLine();
            }

            // Final tag marks the end of the vCard item
            sw.WriteLine("END:VCARD");
            sw.Flush();
        }
    }
}
