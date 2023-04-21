using System;
using System.Collections.Generic;

#nullable disable

namespace BackgroundServiceQueue.Models
{
    public partial class Worker
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime RunDate { get; set; }
        public DateTime FutureDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int Frequency { get; set; }
        public bool IsActive { get; set; }
    }
}

