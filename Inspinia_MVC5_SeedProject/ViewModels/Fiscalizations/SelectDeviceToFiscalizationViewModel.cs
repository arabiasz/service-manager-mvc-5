using Inspinia_MVC5_SeedProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.ViewModels.Fiscalizations
{
    public class SelectDeviceToFiscalizationViewModel
    {
        public int DeviceId { get; set; }
        public int ModuleId { get; set; }
        public string DeviceName { get; set; }

        [LocalizationValidate]
        public bool Selected { get; set; }

        public string SerialNumber { get; set; }
        public string UniqueNumber { get; set; }
        public string Status { get; set; }

        public int AddressId { get; set; }

        public IEnumerable<Address> Addresses { get; set; }

        public SelectDeviceToFiscalizationViewModel()
        {
            Addresses = new List<Address>();
        }
    }
}