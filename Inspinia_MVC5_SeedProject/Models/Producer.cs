using ServiceManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.Models
{
    public class Producer
    {
        [Display(Name = "Producent")]
        public int ProducerId { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Nazwa")]
        [RemoteClientServerAttribute("NameExists", "Producers", ErrorMessage = "Producent o podanej nazwie już istnieje", AdditionalFields = "ProducerId")]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Ulica")]
        public string Street { get; set; }
        [Required]
        [StringLength(10)]
        [Display(Name = "Kod pocztowy")]
        public string ZipCode { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Miejscowość")]
        public string City { get; set; }

        [Display(Name = "NIP")]
        public string Nip { get; set; }
        public string Regon { get; set; }

        [Url]
        [StringLength(255)]
        [Display(Name = "Strona firmowa")]
        public string Web { get; set; }
    }
}