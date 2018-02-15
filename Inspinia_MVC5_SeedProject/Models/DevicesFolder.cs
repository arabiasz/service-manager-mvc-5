using ServiceManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.Models
{
    public class DevicesFolder
    {
        public int DevicesFolderId { get; set; }

        [Required]
        [Display(Name = "Nazwa")]
        [RemoteClientServer("NameExists", "DevicesFolders", ErrorMessage = "Kartoteka z proponowaną nazwą już istnieje", AdditionalFields = "DevicesFolderId")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Producent")]
        public int ProducerId { get; set; }

        [Required]
        [Display(Name = "Grupa")]
        public int GroupId { get; set; }


        public Producer Producer { get; set; }
        public Group Group { get; set; }
    }
}