using ServiceManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.Models
{
    public class Group
    {
        [Display(Name = "Grupa")]
        public int GroupId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nazwa")]
        [RemoteClientServer("NameExists", "Groups", ErrorMessage = "Grupa z proponowaną nazwą już istnieje", AdditionalFields = "GroupId")]
        public string Name { get; set; }
    }
}