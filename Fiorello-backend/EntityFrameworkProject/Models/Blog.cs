using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkProject.Models
{
    public class Blog : BaseEntity
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public DateTime Date { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
      
    }
}
