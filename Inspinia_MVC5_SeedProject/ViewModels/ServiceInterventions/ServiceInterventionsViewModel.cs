using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.ViewModels.ServiceInterventions
{
    public class ServiceInterventionsViewModel
    {
        public int ServiceInterventionId { get; set; }

        [Display(Name = "Model")]
        public string ModelName { get; set; }

        [Display(Name = "Numer Fabryczny")]
        public string SerialNumber { get; set; }

        [Display(Name = "Numer unikatowy")]
        public string UniqueNumber { get; set; }

        [Display(Name = "Podatnik")]
        public string CustomerName { get; set; }

        [Display(Name = "NIP")]
        public string CustomerNip { get; set; }


        [Display(Name = "Data rozpoczęcia naprawy:")]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime InterventionStart { get; set; }

        [Display(Name = "Data zakończenia naprawy:")]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? InterventionEnd { get; set; }

        public string Status { get; set; }
    }
}