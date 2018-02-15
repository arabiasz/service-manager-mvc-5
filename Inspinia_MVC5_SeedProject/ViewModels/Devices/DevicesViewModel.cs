using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.ViewModels.Devices
{
    public class DevicesViewModel
    {
        public int DeviceId { get; set; }

        [Required]
        [Display(Name = "Model")]
        public int DevicesFolderId { get; set; }

        [Display(Name = "Model")]
        public string DeviceName { get; set; }

        [Required]
        [Display(Name = "Numer fabryczny")]
        public string SerialNumber { get; set; }

        [Required]
        [Display(Name = "Numer unikatowy")]
        [RegularExpression("^([A-Za-z]{3}\\s?[0-9]{8}|[A-Za-z]{2}\\s?[0-9]{8}|[A-Za-z]{3}\\s?[0-9]{10})$", ErrorMessage = "Błędny numer unikatowy")]
        public string UniqueNumber { get; set; }

        [Display(Name = "Numer ewidencyjny")]
        public string RegistrationNumber { get; set; }

        [Required]
        [Display(Name = "Przegląd (m-ce)")]
        public int ReviewInterval { get; set; }

        [Display(Name = "Gwarancja (m-ce)")]
        public int WarrantyInterval { get; set; }


        public string Status { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Następny przegląd")]
        public DateTime InterimReviewDate { get; set; }
    }
}