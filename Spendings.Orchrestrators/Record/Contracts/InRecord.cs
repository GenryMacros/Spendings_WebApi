using System.ComponentModel.DataAnnotations;
using System;

namespace Spendings.Orchrestrators.Records.Contracts
{
   public class InRecord
    {
        public int CategoryId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Wrong amount ")]
        public int Amount { get; set; }
    }
}
