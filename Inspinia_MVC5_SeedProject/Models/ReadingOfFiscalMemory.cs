using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.Models
{
    public class ReadingOfFiscalMemory
    {
        //public ReadingOfFiscalMemory()
        //{
        //    VAT_G_new = VAT_G_old = "zw"; 
        //}
        public int ReadingOfFiscalMemoryId { get; set; }
        public int ModuleId { get; set; }
        public Module Module { get; set; }

        [Required]
        [Display(Name = "Imię i nazwisko lub nazwa podatnika")]
        public string CustomerName { get; set; }

        [Required]
        [Display(Name = "NIP")]
        public string CustomerNIP { get; set; }

        [Required]
        [Display(Name = "Adres siedziby lub miejsce zamieszkania podatnika")]
        public string CustomerAddress { get; set; }

        [Required]
        [Display(Name = "Miejsce instalacji kasy")]
        public string InstallationAddress { get; set; }

        [Required]
        [Display(Name = "Numer unikatowy")]
        public string UniqueNumber { get; set; }

        [Required]
        [Display(Name = "Numer fabryczny")]
        public string SerialNumber { get; set; }

        [StringLength(50)]
        [Display(Name = "Numer ewidencyjny")]
        public string RegistrationNumber { get; set; }

        [Required]
        [Display(Name = "Data fiskalizacji")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateOfFiscalization { get; set; }

        [Display(Name = "Powód odczytu")]
        public string ReadingReason { get; set; }

        [Display(Name = "Typ raportu")]
        public int ReadingType { get; set; }

        [Required]
        [Display(Name = "Od raportu")]
        public int FromReport { get; set; }

        [Required]
        [Display(Name = "Do raportu")]
        public int ToReport { get; set; }

        [Required]
        [Display(Name = "Data początkowa")]
        public DateTime FromReportDate { get; set; }

        [Required]
        [Display(Name = "Data końcowa")]
        public DateTime? ToReportDate { get; set; }

        [Display(Name = "Sprzedaż wg stawki A")]
        public string S_PTU_A_old { get; set; }
        public string S_PTU_B_old { get; set; }
        public string S_PTU_C_old { get; set; }
        public string S_PTU_D_old { get; set; }
        public string S_PTU_E_old { get; set; }
        public string S_PTU_F_old { get; set; }
        public string S_PTU_G_old { get; set; }

        public string VAT_A_old { get; set; }
        public string VAT_B_old { get; set; }
        public string VAT_C_old { get; set; }
        public string VAT_D_old { get; set; }
        public string VAT_E_old { get; set; }
        public string VAT_F_old { get; set; }
        public string VAT_G_old { get; set; }

        public string PTU_A_old { get; set; }
        public string PTU_B_old { get; set; }
        public string PTU_C_old { get; set; }
        public string PTU_D_old { get; set; }
        public string PTU_E_old { get; set; }
        public string PTU_F_old { get; set; }

        public string S_PTU_A_new { get; set; }
        public string S_PTU_B_new { get; set; }
        public string S_PTU_C_new { get; set; }
        public string S_PTU_D_new { get; set; }
        public string S_PTU_E_new { get; set; }
        public string S_PTU_F_new { get; set; }
        public string S_PTU_G_new { get; set; }

        public string VAT_A_new { get; set; }
        public string VAT_B_new { get; set; }
        public string VAT_C_new { get; set; }
        public string VAT_D_new { get; set; }
        public string VAT_E_new { get; set; }
        public string VAT_F_new { get; set; }
        public string VAT_G_new { get; set; }

        public string PTU_A_new { get; set; }
        public string PTU_B_new { get; set; }
        public string PTU_C_new { get; set; }
        public string PTU_D_new { get; set; }
        public string PTU_E_new { get; set; }
        public string PTU_F_new { get; set; }

        [Required]
        [Display(Name = "Łączna należność")]
        public string TotalAmount { get; set; }

        [Display(Name = "Łączna kwota PTU")]
        public string TotalAmountPTU { get; set; }

        [Display(Name = "Liczba zerowań RAM")]
        public string MemoryResetCount { get; set; }

        [Display(Name = "Liczba pragonów fiskalnych")]
        public string NumberOfFiscalRecipt { get; set; }

        [Display(Name = "Liczba anulowanych paragonów")]
        public string NumberOfReciptCanceled { get; set; }

        [Display(Name = "Wartość anulowanych aragonów")]
        public string ValueOfCanceledRecipt { get; set; }

        [Display(Name = "Uwagi")]
        public string Addnotations { get; set; }

        [Display(Name = "Wykonane przeglądy")]
        public string DatesOfInterimReviews { get; set; }

        [Required]
        [Display(Name = "Miejscowość")]
        public string City { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Data odczytu")]
        public DateTime? DateOfCompletion { get; set; }
    }
}