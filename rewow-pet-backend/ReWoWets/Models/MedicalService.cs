namespace ReWoWets.Models
{
    public class MedicalService
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public Pet? Pet { get; set; }
        public string ServiceType { get; set; }
        public DateTime Date { get; set; }
    }

    public class MedicalServiceDto
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public string ServiceType { get; set; }
        public DateTime Date { get; set; }
    }
} 