using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstroPhotographyAPI.Models
{
    public class DBPhoto
    {
        public int Id { get; set; }
        public string PhotoName { get; set; }
        public string PhotoLocation { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }
        public string MainCamera { get; set; }
        public string MainTelescope { get; set; }
        public string Mount { get; set; }
        public string GuideScope { get; set; }
        public string GuideCamera { get; set; }
        public string Filters { get; set; }
        public string Other { get; set; }
    }
}
