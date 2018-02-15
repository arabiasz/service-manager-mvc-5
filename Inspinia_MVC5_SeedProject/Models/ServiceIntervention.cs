using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.Models
{
    public class ServiceIntervention
    {
        public int ServiceInterventionId { get; set; }
        public int ModuleId { get; set; }

        [Display(Name = "Data rozpoczęcia naprawy:")]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime InterventionStart { get; set; }

        [Display(Name = "Data zakończenia naprawy:")]
        public DateTime? InterventionEnd { get; set; }

        [Display(Name = "Paragonów fiskalnych")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Dopuszczalne tylko nieujemne liczby całkowite")]
        public string ReceiptsFiscalCountStart { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Dopuszczalne tylko nieujemne liczby całkowite")]
        public string ReceiptsFiscalCountEnd { get; set; }

        [Display(Name = "Raportu dob. fisk.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Dopuszczalne tylko nieujemne liczby całkowite")]
        public string FiscalDailyReportStart { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Dopuszczalne tylko nieujemne liczby całkowite")]
        public string FiscalDailyReportEnd { get; set; }

        [Display(Name = "Zerowania RAM")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Dopuszczalne tylko nieujemne liczby całkowite")]
        public string ResettingRamCountStart { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Dopuszczalne tylko nieujemne liczby całkowite")]
        public string ResettingRamCountEnd { get; set; }

        [Display(Name = "Niefiskalnego (ogółem)")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Dopuszczalne tylko nieujemne liczby całkowite")]
        public string ReceiptsCountAllStart { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Dopuszczalne tylko nieujemne liczby całkowite")]
        public string ReceiptsCountAllEnd { get; set; }

        [Display(Name = "Opis zgłoszonych nieprawidłowości działania kasy rejestrującej")]
        public string ProblemsDescrtiption { get; set; }

        [Display(Name = "Stan plomb kasy")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Dopuszczalne tylko nieujemne liczby całkowite")]
        public string SealCount { get; set; }

        [Display(Name = "Stan plomb")]
        public bool SealCondition { get; set; }

        [Display(Name = "Elementy kasy rejestrującej wymienione przy naprawie")]
        public string RepairedComponents { get; set; }

        [Display(Name = "Wyszczególnienie drukowanych dokumentów fiskalnych")]
        public string FiscalDocPrinted { get; set; }

        [Display(Name = "Przyczyny, z powodu których niemożliwa jest naprawa kasy na miejscu")]
        public string WhyCantRepairAtCustomer { get; set; }

        [Display(Name = "Miejsce naprawy / adres serwisu")]
        public string PlaceOfRepair { get; set; }

        [Display(Name = "Data odbioru kasy")]
        public DateTime? ConfirmationOfReceipt { get; set; }

        [Display(Name = "Wpisz nr strony z książki serwisowej")]
        public string ServiceBookPageNumber { get; set; }

        //[Display(Name = "Serwisant")]
        //public string ServiceMan { get; set; }

        public virtual Module Module { get; set; }

        public ServiceIntervention()
        {
            SealCondition = false;
            InterventionStart = DateTime.Now;
        }
    }
}