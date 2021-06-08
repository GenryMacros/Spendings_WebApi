using System;

namespace Spendings.Orchrestrators.Records.Contracts
{
    public class OutRecord
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }    
        public DateTime Date { get; set; }
        public int Amount { get; set; }
    }
}
