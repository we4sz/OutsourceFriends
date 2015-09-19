using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using OutsourceFriends.Context;
using OutsourceFriends.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace OutsourceFriends.Helpers
{
    public class ImageHelper
    {

        public static async Task setImageFromUrl(DomainManager manager, string url, Guide g)
        {
            string imageurl = await setImageFromUrl(url, "G-" + g.UserId);

            await UpdateImageUrl(manager, g, imageurl);
        }

        public static async Task setImageFromUrl(DomainManager manager, string url, Traveler t)
        {

            string imageurl = await setImageFromUrl(url, "T-" + t.UserId);

            await UpdateImageUrl(manager, t, imageurl);
        }

        public static async Task<string> setImageFromUrl(string url, string id)
        {
            return await importImage(url, id);
        }

        public static async Task UpdateImageUrl(DomainManager manager, Guide g, string url)
        {
            g.ImageUrl = url + "?r=" + DateTime.UtcNow.Ticks;

            manager.updateEntity(g);
            await manager.save();
        }


        public static async Task UpdateImageUrl(DomainManager manager, Traveler t, string url)
        {
            t.ImageUrl = url + "?r=" + DateTime.UtcNow.Ticks;
            manager.updateEntity(t);
            await manager.save();
        }



        private static async Task<string> importImage(string url, string MovieId)
        {
            string imageName = null;
            await Task.Run(() =>
            {
                try
                {
                    imageName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + ".jpg";
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile(url, imageName);
                }
                catch (Exception)
                {
                    imageName = null;
                }
            });
            if (imageName != null)
            {
                return await uploadImage(imageName, MovieId);
            }
            return null;
        }



        private static async Task<string> uploadImage(string url, string movieid)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("CloudStorageConnectionString"));

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("imagefiles");
            container.CreateIfNotExists();

            ICloudBlob blob = container.GetBlockBlobReference(movieid);

            Bitmap bmp = new Bitmap(url);

            MemoryStream memoryStream = ImageHelper.rotateAndResizeBitmap(bmp, null, null, null, null, null);

            blob.Properties.ContentType = "image/jpeg";
            await blob.UploadFromStreamAsync(memoryStream);

            File.Delete(url);
            return blob.Uri.AbsoluteUri.Replace("http:","https:");
        }

        public static MemoryStream rotateAndResizeBitmap(Bitmap bmp, int? x, int? y, int? width, int? height, int? rotate)
        {
            MemoryStream memoryStream = new MemoryStream();
            PropertyItem item = bmp.PropertyItems.Where(xx => xx.Id == 0x112).FirstOrDefault();
            int? orientation = null;
            if (item != null)
            {
                orientation = (int)item.Value[0];
            }

            if (x.HasValue && y.HasValue && height.HasValue && width.HasValue)
            {
                Rectangle cropRect = new Rectangle(x.Value, y.Value, width.Value, height.Value);
                Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);
                using (Graphics gr = Graphics.FromImage(target))
                {
                    gr.SmoothingMode = SmoothingMode.HighQuality;
                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    gr.DrawImage(bmp, new Rectangle(0, 0, target.Width, target.Height), cropRect, GraphicsUnit.Pixel);
                }

                bmp.Dispose();
                bmp = target;
            }

            if (orientation.HasValue)
            {
                bmp.RotateFlip(OrientationToFlipType(orientation.Value));
                item.Value = new byte[] { 0, 0 };
                bmp.SetPropertyItem(item);
            }


            double ratioX = (double)500 / (double)bmp.Width;
            double ratioY = (double)500 / (double)bmp.Height;
            double ratio = ratioX < ratioY ? ratioX : ratioY;

            if (ratio < 1)
            {
                int nh = (int)(bmp.Height * ratio);
                int nw = (int)(bmp.Width * ratio);

                Bitmap newImage = new Bitmap(nw, nh);
                using (Graphics gr = Graphics.FromImage(newImage))
                {
                    gr.SmoothingMode = SmoothingMode.HighQuality;
                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    gr.DrawImage(bmp, new Rectangle(0, 0, nw, nh));
                }

                bmp.Dispose();
                bmp = newImage;
            }

            bmp.Save(memoryStream, ImageFormat.Jpeg);
            memoryStream.Position = 0;
            bmp.Dispose();
            return memoryStream;
        }


        public static RotateFlipType OrientationToFlipType(int orientation)
        {
            switch (orientation)
            {
                case 1:
                    return RotateFlipType.RotateNoneFlipNone;
                case 2:
                    return RotateFlipType.RotateNoneFlipX;
                case 3:
                    return RotateFlipType.Rotate180FlipNone;
                case 4:
                    return RotateFlipType.Rotate180FlipX;
                case 5:
                    return RotateFlipType.Rotate90FlipX;
                case 6:
                    return RotateFlipType.Rotate90FlipNone;
                case 7:
                    return RotateFlipType.Rotate270FlipX;
                case 8:
                    return RotateFlipType.Rotate270FlipNone;
                default:
                    return RotateFlipType.RotateNoneFlipNone;
            }
        }



    }
}