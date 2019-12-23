using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AstroPhotographyAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace AstroPhotographyAPI.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class AstroPhotographyAPIController : ControllerBase
    {
        private readonly AppDbContext _dbcontext;

        public AstroPhotographyAPIController(AppDbContext context)
        {
            _dbcontext = context;
        }

        [EnableCors("AllowOrigin")]
        [HttpGet("/api/v1/GetAllPhotos")]
        public async Task<ActionResult<IEnumerable<DBPhoto>>> GetAllItems()
        {
            return await _dbcontext.Photos.ToListAsync();
        }

        [EnableCors("AllowOrigin")]
        [HttpPost("/api/v1/UploadPhoto")]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            if (file != null)
            {
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                using (var stream = new FileStream(pathToSave, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return Ok();
        }
        [EnableCors("AllowOrigin")]
        [HttpGet("/api/v1/GenerateCloudData")]
        public async Task<IActionResult> GenCloudData()
        {
            CloudDataJson cloudDataJson = new CloudDataJson();
            cloudDataJson.cloudData = new Dictionary<string, List<CloudData>>();

            List<CloudData> _data = new List<CloudData>();

                DateTime time = DateTime.Now;
                string generatedString = time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + "12";

                // convert to UTC before passing, so if you want 5PM (1700) (+8 then -12) subtract 4 so 13
                var day = time.Day;
                var timeToPrint = time.Hour;

                var inc = (time.Hour - 4);

                for (int i = inc; i < 49; i++)
                {
                    string timeToGet = i.ToString();

                    if (i < 10)
                    {
                        timeToGet = "0" + i.ToString();
                    }

                    var anzaColor = CloudData.GetAnzaRGBA(generatedString, timeToGet);
                    var cloudCover = CloudData.CloudCoverData(anzaColor);

                    if ((i + 4) == 24 || (i + 4) == 48)
                    {
                        day++;
                        timeToPrint = timeToPrint - 24;
                    }

                    _data.Add(new CloudData()
                    {
                        Day = day,
                        Month = time.Month,
                        Year = time.Year,
                        Hour = timeToPrint,
                        CloudCover = cloudCover.Item2,
                        RGBa = cloudCover.Item1
                    });

                    if (cloudDataJson.cloudData.ContainsKey(time.Year + "/" + time.Month + "/" + day))
                    {
                        // item exists already
                        foreach (var item in cloudDataJson.cloudData)
                        {
                            if (item.Key == (time.Year + "/" + time.Month + "/" + day))
                            {
                                item.Value.Add(new CloudData()
                                {
                                    Day = day,
                                    Month = time.Month,
                                    Year = time.Year,
                                    Hour = timeToPrint,
                                    CloudCover = cloudCover.Item2,
                                    RGBa = cloudCover.Item1
                                });
                            }
                        }
                    }
                    else
                    {
                        // create new key pair
                        List<CloudData> newItem = new List<CloudData>();
                        newItem.Add(new CloudData()
                        {
                            Day = day,
                            Month = time.Month,
                            Year = time.Year,
                            Hour = timeToPrint,
                            CloudCover = cloudCover.Item2,
                            RGBa = cloudCover.Item1
                        });

                        cloudDataJson.cloudData.Add((time.Year + "/" + time.Month + "/" + day), newItem);


                    }
                    //Console.WriteLine(time.Month.ToString() + "/" + day + "/" + time.Year + " Time: " + timeToPrint + ":00" + " " + cloudCover.Item2);
                    timeToPrint++;
                }

                string json = JsonConvert.SerializeObject(cloudDataJson.cloudData, Formatting.Indented);

                // write string to file
                System.IO.File.WriteAllText("cloudData.json", json);

                List<string> listOfData = new List<string>();

                foreach (var item in _data)
                {
                    listOfData.Add(item.Month.ToString() + "/" + item.Day.ToString() + "/" + item.Year.ToString() + " - " + "Time: " + item.Hour + " - " + item.CloudCover);
                }

                return Ok(cloudDataJson.cloudData);
        }

        [EnableCors("AllowOrigin")]
        [HttpGet("/api/v1/GetCloudCoverData")]
        public async Task<IActionResult> GetCloudData()
        {
            string jsonString = System.IO.File.ReadAllText("cloudData.json");

            return Ok(jsonString);
        }

        [EnableCors("AllowOrigin")]
        [HttpGet("/api/v1/GenerateTransparency")]
        public async Task<IActionResult> GenTransparency()
        {
            TransparencyDataJson transparencyDataJson = new TransparencyDataJson();
            transparencyDataJson.transparecnyData = new Dictionary<string, List<TransparencyData>>();

            List<TransparencyData> _data = new List<TransparencyData>();

            DateTime time = DateTime.Now;
            string generatedString = time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + "12";

            // convert to UTC before passing, so if you want 5PM (1700) (+8 then -12) subtract 4 so 13
            var day = time.Day;
            var timeToPrint = time.Hour;

            var inc = (time.Hour - 4);

            for (int i = inc; i < 49; i++)
            {
                string timeToGet = i.ToString();

                if (i < 10)
                {
                    timeToGet = "0" + i.ToString();
                }

                var anzaColor = TransparencyData.GetAnzaRGBA(generatedString, timeToGet);
                var transparecnyCloudCover = TransparencyData.TransparencyCoverData(anzaColor);

                if ((i + 4) == 24 || (i + 4) == 48)
                {
                    day++;
                    timeToPrint = timeToPrint - 24;
                }

                _data.Add(new TransparencyData()
                {
                    Day = day,
                    Month = time.Month,
                    Year = time.Year,
                    Hour = timeToPrint,
                    TransparencyCover = transparecnyCloudCover.Item2,
                    RGBa = transparecnyCloudCover.Item1
                });

                if (transparencyDataJson.transparecnyData.ContainsKey(time.Year + "/" + time.Month + "/" + day))
                {
                    // item exists already
                    foreach (var item in transparencyDataJson.transparecnyData)
                    {
                        if (item.Key == (time.Year + "/" + time.Month + "/" + day))
                        {
                            item.Value.Add(new TransparencyData()
                            {
                                Day = day,
                                Month = time.Month,
                                Year = time.Year,
                                Hour = timeToPrint,
                                TransparencyCover = transparecnyCloudCover.Item2,
                                RGBa = transparecnyCloudCover.Item1
                            });
                        }
                    }
                }
                else
                {
                    // create new key pair
                    List<TransparencyData> newItem = new List<TransparencyData>();
                    newItem.Add(new TransparencyData()
                    {
                        Day = day,
                        Month = time.Month,
                        Year = time.Year,
                        Hour = timeToPrint,
                        TransparencyCover = transparecnyCloudCover.Item2,
                        RGBa = transparecnyCloudCover.Item1
                    });

                    transparencyDataJson.transparecnyData.Add((time.Year + "/" + time.Month + "/" + day), newItem);


                }
                //Console.WriteLine(time.Month.ToString() + "/" + day + "/" + time.Year + " Time: " + timeToPrint + ":00" + " " + cloudCover.Item2);
                timeToPrint++;
            }

            string json = JsonConvert.SerializeObject(transparencyDataJson.transparecnyData, Formatting.Indented);

            // write string to file
            System.IO.File.WriteAllText("transparencyData.json", json);

            List<string> listOfData = new List<string>();

            foreach (var item in _data)
            {
                listOfData.Add(item.Month.ToString() + "/" + item.Day.ToString() + "/" + item.Year.ToString() + " - " + "Time: " + item.Hour + " - " + item.TransparencyCover);
            }

            return Ok(transparencyDataJson.transparecnyData);
        }

        [EnableCors("AllowOrigin")]
        [HttpGet("/api/v1/GetTransparencyData")]
        public async Task<IActionResult> GetTranspData()
        {
            string jsonString = System.IO.File.ReadAllText("transparencyData.json");

            return Ok(jsonString);
        }
    }
}