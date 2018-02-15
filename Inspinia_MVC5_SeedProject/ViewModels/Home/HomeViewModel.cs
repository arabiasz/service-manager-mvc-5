using Inspinia_MVC5_SeedProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.ViewModels.Home
{
    public class HomeViewModel
    {
        public virtual IEnumerable<InterimReview> interimReviews { get; set; }
        public virtual IEnumerable<ServiceIntervention> serviesInterventions { get; set; }
     //   public virtual ICollection<InterimReviews> interimReviews { get; set; }

    }
}