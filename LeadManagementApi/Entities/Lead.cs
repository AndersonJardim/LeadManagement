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

        // Novos campos para o desafio
        public string Status { get; set; } = "Invited"; // Invited, Accepted, Declined
        public string? ContactFullName { get; set; } // Nullable, preenchido ao aceitar
        public string? ContactPhoneNumber { get; set; } // Nullable, preenchido ao aceitar
        public string? ContactEmail { get; set; } // Nullable, preenchido ao aceitar
    }
}