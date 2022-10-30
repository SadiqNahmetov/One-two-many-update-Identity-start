using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.ViewModels.SliderViewModels
{
    public class SliderCreateVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Can't be empty")]
        public List<IFormFile> Photos { get; set; }
    }
}
