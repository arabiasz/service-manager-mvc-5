using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.Models
{
    public class Module
    {
        public int ModuleId { get; set; }
        public int DeviceId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Numer unikatowy")]
        [RegularExpression("^([A-Za-z]{3}\\s?[0-9]{8}|[A-Za-z]{2}\\s?[0-9]{8}|[A-Za-z]{3}\\s?[0-9]{10})$", ErrorMessage = "Błędny numer unikatowy")]
        public string UniqueNumber { get; set; }

        public bool Active { get; set; }
        public string Status { get; set; }

        public virtual Device Device { get; set; }
    }
}