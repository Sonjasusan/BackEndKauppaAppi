using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuokaAppiBackend.Models
{
    public class KauppaostosData
    {

        public int IdKauppaOstos { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool InProgress { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public bool Active { get; set; }
        public bool Completed { get; set; }

    }
}
