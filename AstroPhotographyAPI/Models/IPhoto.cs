using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstroPhotographyAPI.Models
{
    public interface IPhoto
    {
        DBPhoto GetPhotoByID(int id);
        IEnumerable<DBPhoto> GetAllPhotos();
    }
}
