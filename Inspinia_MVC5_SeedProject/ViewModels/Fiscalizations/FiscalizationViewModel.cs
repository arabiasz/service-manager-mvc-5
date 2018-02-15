using Inspinia_MVC5_SeedProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.ViewModels.Fiscalizations
{
    public class FiscalizationViewModel
    {
        public int FiscalizationId { get; set; }

        //[Required]
        [Display(Name = "Podatnik")]
        public int CustomerId { get; set; }

        //[Required]
        [Display(Name = "Urząd Skarbowy")]
        public int TaxOfficeId { get; set; }

        [Required]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Data fiskalizacji")]
        public string FiscalizationDate { get; set; }

        [Required]
        [Display(Name = "Data przeglądu")]
        public string RevievDate { get; set; }

        [Required]
        [Display(Name = "Data obowiązku...")]
        public string UseOfTheDeviceDate { get; set; }

        [Required]
        [Display(Name = "Miejscowość")]
        public string City { get; set; }

        public string OrderNumber { get; set; }

        //[Required]
        [Display(Name = "Serwisant")]
        public string Servisman { get; set; }

        public int ValidationMessage { get; set; }

        public List<SelectDeviceToFiscalizationViewModel> Devices { get; set; }

        public FiscalizationViewModel()
        {
            Devices = new List<SelectDeviceToFiscalizationViewModel>();
        }

        public IEnumerable<DeviceToAddress> GetSelectedIds()
        {
            return (from d in Devices where d.Selected select new DeviceToAddress() { DeviceId = d.DeviceId, ModuleId = d.ModuleId, AddressId = d.AddressId }).ToList();
        }
    }
}