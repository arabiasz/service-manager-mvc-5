using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.Models
{
    public class Address
    {
        public int AddressId { get; set; }

        [Required]
        [Display(Name = "Nazwa")]
        //     [RemoteClientServerAttribute("NameExists", "Producers", ErrorMessage = "Producent o podanej nazwie już istnieje", AdditionalFields = "ProducerId")]
        public string Name { get; set; }

        [StringLength(50)]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [StringLength(5)]
        [Display(Name = "Nr domu")]
        public string HomeNumber { get; set; }

        [StringLength(5)]
        [Display(Name = "Nr lokalu")]
        public string PlaceNumber { get; set; }

        [Required]
        [StringLength(6)]
        [Display(Name = "Kod pocztowy")]
        [RegularExpression(@"^(\d{2}-\d{3})$", ErrorMessage = "Błędny kod pocztowy")]
        public string ZipCode { get; set; }

        [StringLength(10)]
        [Display(Name = "Skrytka pocztowa")]
        public string PostalBox { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Poczta")]
        public string Post { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Miejscowość")]
        public string City { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}