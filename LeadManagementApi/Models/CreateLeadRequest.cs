using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadManagementApi.Models
{
    public class CreateLeadRequest
    {
        public string ContactFirstName { get; set; }
        public string Suburb { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}