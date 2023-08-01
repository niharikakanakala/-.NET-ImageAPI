using System;
using System.ComponentModel.DataAnnotations;

namespace Image.WebAPI.Data
{
    public class ImageDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ImageName { get; set; }

        public string ImageURL { get; set; }

        public string ImageDescription { get; set; }
    }
}
