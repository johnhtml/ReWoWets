using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReWoWets.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReWoWets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetController : ControllerBase
    {
        private readonly ReWowDbContext _context;
        public PetController(ReWowDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetDto>>> GetPets()
        {
            var pets = await _context.Pets.Include(p => p.Vaccinations).Include(p => p.MedicalServices).ToListAsync();
            return pets.Select(p => ToPetDto(p)).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PetDto>> GetPet(int id)
        {
            var pet = await _context.Pets.Include(p => p.Vaccinations).Include(p => p.MedicalServices).FirstOrDefaultAsync(p => p.Id == id);
            if (pet == null) return NotFound();
            return ToPetDto(pet);
        }

        [HttpPost]
        public async Task<ActionResult<PetDto>> CreatePet(PetDto petDto)
        {
            var pet = new Pet
            {
                Name = petDto.Name,
                Type = petDto.Type,
                Size = petDto.Size,
                Description = petDto.Description,
                OwnerId = petDto.OwnerId
            };
            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();
            
            pet = await _context.Pets.Include(p => p.Vaccinations).Include(p => p.MedicalServices).FirstOrDefaultAsync(p => p.Id == pet.Id);
            return CreatedAtAction(nameof(GetPet), new { id = pet.Id }, ToPetDto(pet));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePet(int id, PetDto petDto)
        {
            if (id != petDto.Id) return BadRequest();
            var pet = await _context.Pets.Include(p => p.Vaccinations).Include(p => p.MedicalServices).FirstOrDefaultAsync(p => p.Id == id);
            if (pet == null) return NotFound();
            pet.Name = petDto.Name;
            pet.Type = petDto.Type;
            pet.Size = petDto.Size;
            pet.Description = petDto.Description;
            pet.OwnerId = petDto.OwnerId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePet(int id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null) return NotFound();
            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private static PetDto ToPetDto(Pet pet)
        {
            return new PetDto
            {
                Id = pet.Id,
                Name = pet.Name,
                Type = pet.Type,
                Size = pet.Size,
                Description = pet.Description,
                OwnerId = pet.OwnerId,
                Vaccinations = pet.Vaccinations?.Select(v => new VaccinationDto
                {
                    Id = v.Id,
                    Number = v.Number,
                    Type = v.Type,
                    Date = v.Date
                }).ToList() ?? new List<VaccinationDto>(),
                MedicalServices = pet.MedicalServices?.Select(m => new MedicalServiceDto
                {
                    Id = m.Id,
                    ServiceType = m.ServiceType,
                    Date = m.Date
                }).ToList() ?? new List<MedicalServiceDto>()
            };
        }
    }
} 