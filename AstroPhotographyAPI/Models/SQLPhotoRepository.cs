using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstroPhotographyAPI.Models
{
    public class SQLPhotoRepository : IPhoto
    {
        private readonly AppDbContext Context;
        public SQLPhotoRepository(AppDbContext context)
        {
            Context = context;
        }
        public IEnumerable<DBPhoto> GetAllPhotos()
        {
            return Context.Photos;
        }

        public DBPhoto GetPhotoByID(int id)
        {
            return Context.Photos.Find(id);
        }
    }
}
