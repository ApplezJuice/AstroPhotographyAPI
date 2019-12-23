using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Net;

namespace AstroPhotographyAPI.Models
{
    public class TransparencyDataJson
    {
        public Dictionary<string, List<TransparencyData>> transparecnyData { get; set; }
    }
    public class TransparencyData
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Hour { get; set; }
        public string TransparencyCover { get; set; }
        public Rgba32 RGBa { get; set; }

        public static Rgba32 GetAnzaRGBA(string dateString, string time)
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
            colorLegend.Add(new Rgba32(199, 199, 199, 255), "Poor");
            colorLegend.Add(new Rgba32(149, 213, 213, 255), "Below Average");
            colorLegend.Add(new Rgba32(99, 163, 227, 255), "Average");
            colorLegend.Add(new Rgba32(44, 108, 172, 255), "Above Average");
            colorLegend.Add(new Rgba32(0, 63, 127, 255), "Transparent");

            foreach (var color in colorLegend)
            {
                if (color.Key == input)
                {
                    return Tuple.Create(color.Key, color.Value);
                }
            }

            return Tuple.Create(new Rgba32(), "No color in legend");
        }
    }
}
