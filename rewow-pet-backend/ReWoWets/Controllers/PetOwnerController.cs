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
    public class PetOwnerController : ControllerBase
    {
        private readonly ReWowDbContext _context;
        public PetOwnerController(ReWowDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetOwnerDto>>> GetPetOwners()
        {
            var owners = await _context.PetOwners.Include(o => o.Pets).ThenInclude(p => p.Vaccinations).Include(o => o.Pets).ThenInclude(p => p.MedicalServices).ToListAsync();
            return owners.Select(o => ToPetOwnerDto(o)).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PetOwnerDto>> GetPetOwner(int id)
        {
            var owner = await _context.PetOwners.Include(o => o.Pets).ThenInclude(p => p.Vaccinations).Include(o => o.Pets).ThenInclude(p => p.MedicalServices).FirstOrDefaultAsync(o => o.Id == id);
            if (owner == null) return NotFound();
            return ToPetOwnerDto(owner);
        }

        [HttpPost]
        public async Task<ActionResult<PetOwnerDto>> CreatePetOwner(PetOwnerDto ownerDto)
        {
            var owner = new PetOwner
            {
                Name = ownerDto.Name
            };
            _context.PetOwners.Add(owner);
            await _context.SaveChangesAsync();
            
            owner = await _context.PetOwners.Include(o => o.Pets).ThenInclude(p => p.Vaccinations).Include(o => o.Pets).ThenInclude(p => p.MedicalServices).FirstOrDefaultAsync(o => o.Id == owner.Id);
            return CreatedAtAction(nameof(GetPetOwner), new { id = owner.Id }, ToPetOwnerDto(owner));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePetOwner(int id, PetOwnerDto ownerDto)
        {
            if (id != ownerDto.Id) return BadRequest();
            var owner = await _context.PetOwners.Include(o => o.Pets).FirstOrDefaultAsync(o => o.Id == id);
            if (owner == null) return NotFound();
            owner.Name = ownerDto.Name;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePetOwner(int id)
        {
            var owner = await _context.PetOwners.FindAsync(id);
            if (owner == null) return NotFound();
            _context.PetOwners.Remove(owner);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private static PetOwnerDto ToPetOwnerDto(PetOwner owner)
        {
            return new PetOwnerDto
            {
                Id = owner.Id,
                Name = owner.Name,
                Pets = owner.Pets?.Select(p => new PetDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Type = p.Type,
                    Size = p.Size,
                    Description = p.Description,
                    OwnerId = p.OwnerId,
                    Vaccinations = p.Vaccinations?.Select(v => new VaccinationDto
                    {
                        Id = v.Id,
                        Number = v.Number,
                        Type = v.Type,
                        Date = v.Date
                    }).ToList() ?? new List<VaccinationDto>(),
                    MedicalServices = p.MedicalServices?.Select(m => new MedicalServiceDto
                    {
                        Id = m.Id,
                        ServiceType = m.ServiceType,
                        Date = m.Date
                    }).ToList() ?? new List<MedicalServiceDto>()
                }).ToList() ?? new List<PetDto>()
            };
        }
    }
} 