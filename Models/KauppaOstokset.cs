using System;
using System.Collections.Generic;

namespace RuokaAppiBackend.Models
{
    public partial class KauppaOstokset
    {
        public KauppaOstokset()
        {
            Timesheets = new HashSet<Timesheet>();
        }

        public int IdKauppaOstos { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public bool Active { get; set; }
        public bool Inprogress { get; set; }
        public bool Completed { get; set; }

        public virtual ICollection<Timesheet> Timesheets { get; set; }
    }
}
