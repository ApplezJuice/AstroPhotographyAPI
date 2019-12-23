using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Net;

namespace AstroPhotographyAPI.Models
{
    public class CloudDataJson
    {
        public Dictionary<string, List<CloudData>> cloudData { get; set; }
    }
    public class CloudData
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Hour { get; set; }
        public string CloudCover { get; set; }
        public string TransparencyCover { get; set; }
        public Rgba32 TransparencyRGBa { get; set; }
        public Rgba32 RGBa { get; set; }
        public string SeeingCover { get; set; }
        public Rgba32 SeeingRGBa { get; set; }

        public static Rgba32 GetAnzaCloudCoverRGBA(string dateString, string time)
        {
            string imagePath = GetImageFromURL($"https://weather.gc.ca/data/prog/regional/{dateString}/{dateString}_054_R1_north@america@southwest_I_ASTRO_nt_0{time}.png");

            using (Image<Rgba32> image = (Image<Rgba32>)Image.Load(imagePath))
            {
                //var size = image.Size();
                var anzaColor = image[225, 479];

                System.IO.File.Delete(imagePath);
                return anzaColor;
            }
        }

        public static Rgba32 GetAnzaTransparencyCoverRGBA(string dateString, string time)
        {
            string imagePath = GetImageFromURL($"https://weather.gc.ca/data/prog/regional/{dateString}/{dateString}_054_R1_north@america@astro_I_ASTRO_transp_0{time}.png");

            using (Image<Rgba32> image = (Image<Rgba32>)Image.Load(imagePath))
            {
                //var size = image.Size();
                var anzaColor = image[175, 514];

                System.IO.File.Delete(imagePath);
                return anzaColor;
            }
        }

        public static Rgba32 GetAnzaSeeingCoverRGBA(string dateString, string time)
        {
            //https://weather.gc.ca/data/prog/regional/2019122312/2019122312_054_R1_north@america@astro_I_ASTRO_seeing_012.png
            string imagePath = GetImageFromURL($"https://weather.gc.ca/data/prog/regional/{dateString}/{dateString}_054_R1_north@america@astro_I_ASTRO_seeing_0{time}.png");

            using (Image<Rgba32> image = (Image<Rgba32>)Image.Load(imagePath))
            {
                //var size = image.Size();
                var anzaColor = image[175, 514];

                System.IO.File.Delete(imagePath);
                return anzaColor;
            }
        }

        public static string GetImageFromURL(string url)
        {
            using (WebClient client = new WebClient())
            {
                var imageGuid = Guid.NewGuid();
                var imageString = "test" + imageGuid.ToString() + ".png";
                client.DownloadFile(new Uri(url), imageString);

                return imageString;
            }
        }

        public static Tuple<Rgba32, string> TransparencyCoverData(Rgba32 input)
        {
            Dictionary<Rgba32, string> colorLegend = new Dictionary<Rgba32, string>();

            colorLegend.Add(new Rgba32(254, 254, 254, 255), "Too Cloudy to forcast");
            colorLegend.Add(new Rgba32(198, 198, 198, 255), "Poor");
            colorLegend.Add(new Rgba32(148, 212, 212, 255), "Below Average");
            colorLegend.Add(new Rgba32(98, 162, 226, 255), "Average");
            colorLegend.Add(new Rgba32(43, 107, 171, 255), "Above Average");
            colorLegend.Add(new Rgba32(0, 62, 126, 255), "Transparent");

            string value;
            colorLegend.TryGetValue(input, out value);

            return Tuple.Create(input, value);
        }

        public static Tuple<Rgba32, string> SeeingCoverData(Rgba32 input)
        {
            Dictionary<Rgba32, string> colorLegend = new Dictionary<Rgba32, string>();

            colorLegend.Add(new Rgba32(254, 254, 254, 255), "Too Cloudy to forcast");
            colorLegend.Add(new Rgba32(198, 198, 198, 255), "Bad");
            colorLegend.Add(new Rgba32(148, 212, 212, 255), "Poor");
            colorLegend.Add(new Rgba32(98, 162, 226, 255), "Average");
            colorLegend.Add(new Rgba32(43, 107, 171, 255), "Good");
            colorLegend.Add(new Rgba32(0, 62, 126, 255), "Excellent");

            string value;
            colorLegend.TryGetValue(input, out value);

            return Tuple.Create(input, value);
        }

        public static Tuple<Rgba32, string> CloudCoverData(Rgba32 input)
        {
            Dictionary<Rgba32, string> colorLegend = new Dictionary<Rgba32, string>();

            colorLegend.Add(new Rgba32(250, 250, 250, 255), "100%");
            colorLegend.Add(new Rgba32(248, 248, 248, 255), "97.5%");
            colorLegend.Add(new Rgba32(238, 238, 238, 255), "95%");
            colorLegend.Add(new Rgba32(233, 233, 233, 255), "92.5%");
            colorLegend.Add(new Rgba32(223, 223, 223, 255), "90%");
            colorLegend.Add(new Rgba32(218, 218, 218, 255), "87.5%");
            colorLegend.Add(new Rgba32(203, 203, 203, 255), "85%");
            colorLegend.Add(new Rgba32(193, 193, 193, 255), "82.5%");
            colorLegend.Add(new Rgba32(188, 252, 252, 255), "80%");
            colorLegend.Add(new Rgba32(183, 247, 247, 255), "77.5%");
            colorLegend.Add(new Rgba32(178, 242, 242, 255), "75%");
            colorLegend.Add(new Rgba32(173, 237, 237, 255), "72.5%");
            colorLegend.Add(new Rgba32(168, 232, 232, 255), "70%");
            colorLegend.Add(new Rgba32(163, 227, 227, 255), "67.5%");
            colorLegend.Add(new Rgba32(158, 222, 222, 255), "65%");
            colorLegend.Add(new Rgba32(153, 217, 217, 255), "62.5%");
            colorLegend.Add(new Rgba32(148, 212, 212, 255), "60%");
            colorLegend.Add(new Rgba32(128, 192, 192, 255), "57.5%");
            colorLegend.Add(new Rgba32(123, 187, 251, 255), "55%");
            colorLegend.Add(new Rgba32(118, 182, 246, 255), "52.5%");
            colorLegend.Add(new Rgba32(113, 177, 241, 255), "50%");
            colorLegend.Add(new Rgba32(108, 172, 236, 255), "47.5%");
            colorLegend.Add(new Rgba32(103, 167, 231, 255), "45%");
            colorLegend.Add(new Rgba32(98, 162, 226, 255), "42.5%");
            colorLegend.Add(new Rgba32(93, 157, 221, 255), "40%");
            colorLegend.Add(new Rgba32(88, 152, 216, 255), "37.5%");
            colorLegend.Add(new Rgba32(83, 147, 211, 255), "35%");
            colorLegend.Add(new Rgba32(78, 142, 206, 255), "32.5%");
            colorLegend.Add(new Rgba32(68, 132, 196, 255), "30%");
            colorLegend.Add(new Rgba32(48, 112, 176, 255), "27.5%");
            colorLegend.Add(new Rgba32(43, 107, 171, 255), "25%");
            colorLegend.Add(new Rgba32(38, 102, 166, 255), "22.5%");
            colorLegend.Add(new Rgba32(33, 97, 161, 255), "20%");
            colorLegend.Add(new Rgba32(28, 92, 156, 255), "17.5%");
            colorLegend.Add(new Rgba32(23, 87, 151, 255), "15%");
            colorLegend.Add(new Rgba32(18, 82, 146, 255), "12.5%");
            colorLegend.Add(new Rgba32(13, 77, 141, 255), "10%");
            colorLegend.Add(new Rgba32(8, 72, 136, 255), "7.5%");
            colorLegend.Add(new Rgba32(3, 67, 131, 255), "5%");
            colorLegend.Add(new Rgba32(0, 62, 126, 255), "2.5% - 0%");

            string value;
            colorLegend.TryGetValue(input, out value);

            return Tuple.Create(input, value);
        }
    }
}
