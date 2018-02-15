using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.Models
{
    public class Fiscalization
    {
        public int FiscalizationId { get; set; }
        public int CustomerId { get; set; }
        public int TaxOfficeId { get; set; }

        [Display(Name = "Data fiskalizacji")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FiscalizationDate { get; set; }

        [Display(Name = "Data przeglądu")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime RevievDate { get; set; }

        [Display(Name = "Data obowiązku stosowania urządzenia")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime UseOfTheDeviceDate { get; set; }

        [Display(Name = "Miejscowość")]
        public string City { get; set; }

        public string OrderNumber { get; set; }
        public string Servisman { get; set; }

        public Customer Customer { get; set; }
        public TaxOffice TaxOffice { get; set; }

        public virtual ICollection<FiscalizationToModules> FiscalizationToModules { get; set; }
    }
}