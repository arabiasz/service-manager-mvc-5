using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.Models
{
    public class FiscalizationToModules
    {
        public int FiscalizationToModulesId { get; set; }
        public int FiscalizationId { get; set; }
        public int ModuleId { get; set; }
        public int AddressId { get; set; }

        public virtual Fiscalization Fiscalization { get; set; }
        public virtual Module Module { get; set; }
        public virtual Address Address { get; set; }
    }
}