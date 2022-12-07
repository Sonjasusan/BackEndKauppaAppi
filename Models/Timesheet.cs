using System;
using System.Collections.Generic;

namespace RuokaAppiBackend.Models
{
    public partial class Timesheet
    {
        public int IdTimesheet { get; set; }
        public int IdKavija { get; set; }
        public int IdKauppaOstos { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public string? Comments { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? Active { get; set; }
        public string? StopLatitude { get; set; }
        public string? StopLongitude { get; set; }
        public string? StartLatitude { get; set; }
        public string? StartLongitude { get; set; }

        public virtual KauppaOstokset IdKauppaOstosNavigation { get; set; } = null!;
        public virtual Kaupassakavijat IdKavijaNavigation { get; set; } = null!;
    }
}
