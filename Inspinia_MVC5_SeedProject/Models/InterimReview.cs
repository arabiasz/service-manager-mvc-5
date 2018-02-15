using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.Models
{
    public class InterimReview
    {
        public int InterimReviewId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateOfReview { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime NextReview { get; set; }

        public string Comments { get; set; }
        public string City { get; set; }
        public string ServiceMan { get; set; }

        [Display(Name = "Model")]
        public int DeviceId { get; set; }

        public Device Device { get; set; }

        public int ModuleId { get; set; }
        public Module Module { get; set; }
    }
}