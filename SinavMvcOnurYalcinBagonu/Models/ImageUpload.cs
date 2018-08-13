using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace SinavMvcOnurYalcinBagonu.Helpers
{
    public class ImageUpload
    {
        string UploadedFileName;
        //Tam boyutlu resimlerin yolu
        public const string PATH = "/Content/images/Upload/";
        //Thumbnails yolu
        public const string PATHt = "/Content/images/Thumbnails/";

        private void CheckFilePaths()
        {
            var pathsT = PATHt.Split('/');
            var pathSumT = "/";
            foreach (var item in pathsT)
            {
                var pathCombined = HttpContext.Current.Server.MapPath(pathSumT);
                if (!Directory.Exists(pathCombined))
                {
                    Directory.CreateDirectory(pathCombined);
                }
                pathSumT += item + "/";
            }

            var paths = PATH.Split('/');
            var pathSum = "/";
            foreach (var item in paths)
            {
                var pathCombined = HttpContext.Current.Server.MapPath(pathSum);
                if (!Directory.Exists(pathCombined))
                {
                    Directory.CreateDirectory(pathCombined);
                }
                pathSum += item + "/";
            }
        }
        internal Tuple<string, string> ImageResize(HttpPostedFileBase FileUpload1, int genislik, int yukseklik, int buyukGenislik, int buyukYukseklik)
        {
            CheckFilePaths();
            string dosyaAdi = (Guid.NewGuid().ToString() + FileUpload1.FileName).Replace(" ", "").Replace("-", String.Empty);
            UploadedFileName = HttpContext.Current.Server.MapPath("~" + PATH + dosyaAdi);
            // UploadedFileName = HttpContext.Current.Server.MapPath("~/Content/images/Upload/" + DateTime.Now.ToString("dd_MM_yyyy").Trim().Replace(':', '_').Replace('.', '_') + DateTime.Now.ToString("dd_MM_yyyy").Trim().Replace(':', '_').Replace('.', '_') + dosyaAdi);
            //string fileType = FileUpload1.FileName.Split('.')[FileUpload1.FileName.Split('.').Length - 1];
            //string resim = string.Empty;
            //Bitmap yeniresim = null;
            //yeniresim = ResimBoyutlandir(FileUpload1.InputStream, buyukGenislik, buyukYukseklik);//yeni resim için boyut veriyoruz..
            //yeniresim.Save(UploadedFileName, ImageFormat.Jpeg);
            Bitmap bmp1 = ResimBoyutlandir(FileUpload1.InputStream, buyukGenislik, buyukYukseklik);
            ImageCodecInfo jgpEncoder = GetEncoder(bmp1.RawFormat.Equals(ImageFormat.Png) ? ImageFormat.Png : ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder,
                50L);
            myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            using (FileStream stream = File.Create(UploadedFileName))
            {
                bmp1.Save(stream, jgpEncoder, myEncoderParameters);
            }
            UploadedFileName = "~" + PATH + UploadedFileName.Split('\\')[UploadedFileName.Split('\\').Length - 1].ToString();


            string imageUrlThumbnail = HttpContext.Current.Server.MapPath("~" + PATHt + dosyaAdi);
            System.Drawing.Image i = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(UploadedFileName));
            System.Drawing.Image thumbnail = new System.Drawing.Bitmap(genislik, yukseklik);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(thumbnail);
            g.DrawImage(i, 0, 0, genislik, yukseklik);

            thumbnail.Save(imageUrlThumbnail);
            return new Tuple<string, string>(PATH + dosyaAdi, PATHt + dosyaAdi);
        }
        internal string ImageResize(HttpPostedFileBase FileUpload1, int genislik, int yukseklik)
        {
            CheckFilePaths();
            string dosyaAdi = FileUpload1.FileName.Replace(" ", "");
            UploadedFileName = HttpContext.Current.Server.MapPath("~" + PATHt + dosyaAdi);
            //Bitmap yeniresim = null;
            //yeniresim = ResimBoyutlandir(FileUpload1.InputStream, genislik, yukseklik);//yeni resim için boyut veriyoruz..
            //yeniresim.Save(UploadedFileName, ImageFormat.Jpeg);
            //Bitmap bmp1 = new Bitmap(FileUpload1.InputStream);
            Bitmap bmp1 = ResimBoyutlandir(FileUpload1.InputStream, genislik, yukseklik);
            ImageCodecInfo jgpEncoder = GetEncoder(bmp1.RawFormat.Equals(ImageFormat.Png) ? ImageFormat.Png : ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder,
                50L);
            myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            using (FileStream stream = File.Create(UploadedFileName))
            {
                bmp1.Save(stream, jgpEncoder, myEncoderParameters);
            }
            return PATHt + dosyaAdi;
        }
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        private Bitmap ResimBoyutlandir(Stream resim, int genislik, int yukseklik)
        {
            Bitmap orjinalresim = new Bitmap(resim);
            int yenigenislik = orjinalresim.Width;
            int yeniyukseklik = orjinalresim.Height;
            double enboyorani = Convert.ToDouble(orjinalresim.Width) / Convert.ToDouble(orjinalresim.Height);

            if (enboyorani <= 1 && orjinalresim.Width > genislik)
            {
                yenigenislik = genislik;
                yeniyukseklik = Convert.ToInt32(Math.Round(yenigenislik / enboyorani));
            }
            else if (enboyorani > 1 && orjinalresim.Height > yukseklik)
            {
                yeniyukseklik = yukseklik;
                yenigenislik = Convert.ToInt32(Math.Round(yeniyukseklik * enboyorani));
            }
            return new Bitmap(orjinalresim, yenigenislik, yeniyukseklik);
        }

        internal static bool DeleteByPath(string Path)
        {
            FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath("~" + Path));
            if (file.Exists)
            {
                file.Delete();
                //System.IO.File.Delete("~" + Path);
                return true;
            }
            return false;


        }

        internal static System.Drawing.Image Base64ToImage(string base64String)
        {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                return image;
            }
        }
    }
}