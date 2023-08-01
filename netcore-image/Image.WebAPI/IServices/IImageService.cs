using System.Collections.Generic;
using System.Threading.Tasks;
using Image.WebAPI.Data;

namespace Image.WebAPI.IServices
{
    public interface IImageService
    {
        Task<List<ImageDetail>> GetAllDetails();
        Task<ImageDetail> GetDetailById(int id);
        Task AddDetail(ImageDetail detail);
        Task UpdateDetail(ImageDetail detail);
        Task DeleteDetail(int id);
        Task DeleteAllDetails();
        // Task<List<ImageDetail>> SearchDetails(string searchTerm);
    
        // Additional CRUD operations for updating and deleting tasks can be defined here.
    }
}
