// using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZenithWebsite.Models.CustomValidation;

namespace ZenithWebsite.Models.ZenithSocietyModels
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        [AfterNow()]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")] // IMPORTANT: This MUST be in this format. Refer to notes.
        [Display(Name = "From")]
        public DateTime FromDate { get; set; }

        [Required]
        [AfterTime("FromDate")]
        [SameDay("FromDate")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        [Display(Name = "To")]
        public DateTime ToDate { get; set; }

        [Required]
        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }  


        [Display(Name = "Activity")]
        public int ActivityId { get; set; }

        [ForeignKey("ActivityId")]
        public Activity Activity { get; set; }
    }
}
