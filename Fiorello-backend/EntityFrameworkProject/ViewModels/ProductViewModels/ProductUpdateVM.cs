using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.ViewModels.ProductViewModels
{
    public class ProductUpdateVM
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; }
        public string CategoryName { get; set; }

        [Required]
        public ICollection<IFormFile> Photos { get; set; }
    }
}
