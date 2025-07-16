namespace ReWoWets.Models
{
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public string Description { get; set; }
        public int OwnerId { get; set; }
        public PetOwner? Owner { get; set; }
        public ICollection<Vaccination>? Vaccinations { get; set; } = new List<Vaccination>();
        public ICollection<MedicalService>? MedicalServices { get; set; } = new List<MedicalService>();
    }
} 