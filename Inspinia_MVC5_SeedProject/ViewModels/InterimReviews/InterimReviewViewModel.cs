using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.ViewModels.InterimReviews
{
    public class InterimReviewViewModel
    {
        public int InterimReviewId { get; set; }
        public int DeviceId { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data przeglądu")]
        public DateTime DateOfReview { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Następny przegląd")]
        public DateTime NextReview { get; set; }

        [Display(Name = "Miejscowość")]
        public string City { get; set; }

        [Display(Name = "Uwagi")]
        public string Comments { get; set; }

        [Display(Name = "Model")]
        public string DeviceName { get; set; }

        [Display(Name = "Numer fabryczny")]
        public string SerialNumber { get; set; }

        [Display(Name = "Numer unikatowy")]
        public string UniqueNumber { get; set; }

        [Display(Name = "Serwisant")]
        public string ServiceMan { get; set; }

        [Display(Name = "DPT")]
        public int Dpt { get; set; }
    }
}