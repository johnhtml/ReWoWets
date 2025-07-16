namespace ReWoWets.Models
{
    public class PetOwner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Pet>? Pets { get; set; } = new List<Pet>();
    }

    public class PetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public string Description { get; set; }
        public int OwnerId { get; set; }
        public List<VaccinationDto> Vaccinations { get; set; } = new();
        public List<MedicalServiceDto> MedicalServices { get; set; } = new();
    }

    public class PetOwnerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PetDto> Pets { get; set; } = new();
    }
} 