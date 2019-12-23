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
            // 1500 current day needs to be 12
            var day = time.Day;
            var timeToPrint = time.Hour;
            string timeToSendForSeeing;

            var inc = (time.Hour - 4);

            for (int i = inc; i < 49; i++)
            {
                string timeToGet = i.ToString();
                var timeForSeeing = i+4;

                if (timeForSeeing % 3 == 0)
                {
                    timeForSeeing -= 3;
                    timeToSendForSeeing = timeForSeeing.ToString();
                }
                else
                {
                    // not in the 3 hour block
                    var timeToRemove = timeForSeeing % 3;
                    timeToSendForSeeing = ((timeForSeeing - timeToRemove) - 3).ToString();
                }

                if (i < 10)
                {
                    timeToGet = "0" + i.ToString();
                    timeToSendForSeeing = "0" + timeToSendForSeeing;
                }

                var anzaColor = CloudData.GetAnzaCloudCoverRGBA(generatedString, timeToGet);
                var cloudCover = CloudData.CloudCoverData(anzaColor);
                var anzaTransparencyColor = CloudData.GetAnzaTransparencyCoverRGBA(generatedString, timeToGet);
                var transparencyCover = CloudData.TransparencyCoverData(anzaTransparencyColor);

                var anzaSeeingColor = CloudData.GetAnzaSeeingCoverRGBA(generatedString, timeToSendForSeeing);
                var anzaSeeingCover = CloudData.SeeingCoverData(anzaSeeingColor);

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
                    TransparencyCover = transparencyCover.Item2,
                    TransparencyRGBa= transparencyCover.Item1,
                    CloudCover = cloudCover.Item2,
                    RGBa = cloudCover.Item1,
                    SeeingCover = anzaSeeingCover.Item2,
                    SeeingRGBa = anzaSeeingCover.Item1
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
                                TransparencyCover = transparencyCover.Item2,
                                TransparencyRGBa = transparencyCover.Item1,
                                CloudCover = cloudCover.Item2,
                                RGBa = cloudCover.Item1,
                                SeeingCover = anzaSeeingCover.Item2,
                                SeeingRGBa = anzaSeeingCover.Item1
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
                        TransparencyCover = transparencyCover.Item2,
                        TransparencyRGBa = transparencyCover.Item1,
                        CloudCover = cloudCover.Item2,
                        RGBa = cloudCover.Item1,
                        SeeingCover = anzaSeeingCover.Item2,
                        SeeingRGBa = anzaSeeingCover.Item1
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
    }
}