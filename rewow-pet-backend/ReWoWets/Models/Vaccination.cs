namespace ReWoWets.Models
{
    public class Vaccination
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public Pet? Pet { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
    }

    public class VaccinationDto
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
    }
} 