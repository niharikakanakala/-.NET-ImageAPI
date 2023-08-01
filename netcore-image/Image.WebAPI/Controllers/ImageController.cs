// ToDoController.cs

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Image.WebAPI.Data;
using Image.WebAPI.IServices;

namespace Image.WebAPI.Controllers
{
    [Route("api/image")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ImageDetail>>> GetAllImages()
        {
            var details = await _imageService.GetAllDetails();
            return Ok(details);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ImageDetail>> GetDetailById(int id)
        {
            var detail = await _imageService.GetDetailById(id);
            if (detail == null)
            {
                return NotFound();
            }
            return Ok(detail);
        }

        [HttpPost]
        public async Task<IActionResult> AddDetail(ImageDetail detail)
        {
            await _imageService.AddDetail(detail);
            return CreatedAtAction(nameof(GetDetailById), new { id = detail.Id }, detail);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDetail(int id, ImageDetail detail)
        {
            if (id != detail.Id)
            {
                return BadRequest();
            }

            var existingDetail = await _imageService.GetDetailById(id);
            if (existingDetail == null)
            {
                return NotFound();
            }

            existingDetail.ImageName = detail.ImageName;
            existingDetail.ImageURL = detail.ImageURL;
            existingDetail.ImageDescription = detail.ImageDescription;

            await _imageService.UpdateDetail(existingDetail);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetail(int id)
        {
            var detail = await _imageService.GetDetailById(id);
            if (detail == null)
            {
                return NotFound();
            }

            await _imageService.DeleteDetail(id);

            return NoContent();
        }

         [HttpDelete]
        public async Task<IActionResult> DeleteAllDetails()
        {
            
                await _imageService.DeleteAllDetails();
                return NoContent();;
        }
    }
}
