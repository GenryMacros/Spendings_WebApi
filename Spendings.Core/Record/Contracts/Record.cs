using System;

namespace Spendings.Core.Records.Contracts
{
    public class Record
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public int CategoryId { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }
    }
}
