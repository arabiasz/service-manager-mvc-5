using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.Models
{
    public class Device
    {
        public int DeviceId { get; set; }

        [Required]
        [Display(Name = "Model")]
        public int DevicesFolderId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Numer fabryczny")]
        public string SerialNumber { get; set; }

        [StringLength(50)]
        [Display(Name = "Numer ewidencyjny")]
        public string RegistrationNumber { get; set; } //numer ewidencyjny

        [Range(1, 48)]
        [Display(Name = "Przegląd")]
        public int ReviewInterval { get; set; }        //przeglady okresowe

        [Range(1, 48)]
        [Display(Name = "Gwarancja")]
        public int WarrantyInterval { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Data przeglądu")]
        public DateTime InterimReviewDate { get; set; }

        public DevicesFolder DevicesFolder { get; set; }

        public virtual ICollection<Module> Modules { get; set; }

        public Device()
        {
            InterimReviewDate = DateTime.Now;
        }
    }
}