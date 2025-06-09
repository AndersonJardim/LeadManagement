using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadManagementApi.Entities
{
    public class Lead
    {
        public Guid Id { get; set; }
        public string ContactFirstName { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public string Suburb { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}