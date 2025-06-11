namespace LeadManagementApi.Models
{
    public class UpdateLeadRequest
    {
        public string ContactFirstName { get; set; }
        public string Suburb { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}