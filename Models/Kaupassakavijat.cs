using System;
using System.Collections.Generic;

namespace RuokaAppiBackend.Models
{
    public partial class Kaupassakavijat
    {
        public Kaupassakavijat()
        {
            Timesheets = new HashSet<Timesheet>();
        }

        public int IdKavija { get; set; }
        public string Nimi { get; set; } = null!;
        public string? Puhelin { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Timesheet> Timesheets { get; set; }
    }
}
