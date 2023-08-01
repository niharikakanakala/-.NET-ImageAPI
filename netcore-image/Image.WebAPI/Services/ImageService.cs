using System.Net.Mime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Image.WebAPI.Data;
using Image.WebAPI.IServices;

namespace Image.WebAPI.Services
{
    public class ImageService : IImageService
    {
        private readonly ImageContext _imageContext;

        public ImageService(ImageContext imageContext)
        {
            _imageContext = imageContext;
        }

        public async Task<List<ImageDetail>> GetAllDetails()
        {
            return await _imageContext.Details.ToListAsync();
        }

        public async Task<ImageDetail> GetDetailById(int id)
        {
            return await _imageContext.Details.FindAsync(id);
        }

        public async Task AddDetail(ImageDetail detail)
        {
            _imageContext.Details.Add(detail);
            await _imageContext.SaveChangesAsync();
        }

        public async Task UpdateDetail(ImageDetail detail)
        {
            _imageContext.Details.Update(detail);
            await _imageContext.SaveChangesAsync();
        }

        public async Task DeleteDetail(int id)
        {
            var detail = await _imageContext.Details.FindAsync(id);
            if (detail != null)
            {
                _imageContext.Details.Remove(detail);
                await _imageContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAllDetails()
        {
          var allDetails = await _imageContext.Details.ToListAsync();
          _imageContext.Details.RemoveRange(allDetails);
          await _imageContext.SaveChangesAsync();
        }

       
    }
}
