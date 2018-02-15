using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.Models
{
    public class Customer
    {
        [Display(Name = "Podatnik")]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Nazwa podatnika")]
        //[RemoteClientServerAttribute("NameExists", "Producers", ErrorMessage = "Producent o podanej nazwie już istnieje", AdditionalFields = "ProducerId")]
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

        [Required]
        [Display(Name = "NIP")]
        public string Nip { get; set; }

        public string Regon { get; set; }

        [StringLength(30)]
        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        [Url]
        [StringLength(255)]
        [Display(Name = "Strona firmowa")]
        public string Web { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [StringLength(30)]
        [Display(Name = "Kraj")]
        public string Country { get; set; }

        [StringLength(30)]
        [Display(Name = "Województwo")]
        public string Province { get; set; }

        [StringLength(30)]
        [Display(Name = "Gmina")]
        public string Community { get; set; }


        public virtual ICollection<Address> Addresses { get; set; }

    }
}