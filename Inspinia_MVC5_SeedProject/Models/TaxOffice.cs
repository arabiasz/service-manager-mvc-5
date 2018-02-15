using ServiceManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.Models
{
    public class TaxOffice
    {
        [Display(Name = "Urząd Skarbowy")]
        public int TaxOfficeId { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Nazwa")]
        [RemoteClientServerAttribute("NameExists", "TaxOffices", ErrorMessage = "Urząd Skarbowy o podanej nazwie już istnieje", AdditionalFields = "TaxOfficeId")]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Nr domu")]
        public string HomeNumber { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Kod pocztowy")]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Miejscowość")]
        public string City { get; set; }
    }
}