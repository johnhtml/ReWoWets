using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReWoWets.Models;

namespace ReWoWets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VaccinationController : ControllerBase
    {
        private readonly ReWowDbContext _context;
        public VaccinationController(ReWowDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VaccinationDto>>> GetVaccinations()
        {
            return await _context.Vaccinations
                .Select(v => new VaccinationDto
                {
                    Id = v.Id,
                    PetId = v.PetId,
                    Number = v.Number,
                    Type = v.Type,
                    Date = v.Date
                }).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VaccinationDto>> GetVaccination(int id)
        {
            var v = await _context.Vaccinations.FindAsync(id);
            if (v == null) return NotFound();
            return new VaccinationDto
            {
                Id = v.Id,
                PetId = v.PetId,
                Number = v.Number,
                Type = v.Type,
                Date = v.Date
            };
        }

        [HttpPost]
        public async Task<ActionResult<VaccinationDto>> CreateVaccination(VaccinationDto dto)
        {
            var vaccination = new Vaccination
            {
                PetId = dto.PetId,
                Number = dto.Number,
                Type = dto.Type,
                Date = dto.Date
            };
            _context.Vaccinations.Add(vaccination);
            await _context.SaveChangesAsync();
            dto.Id = vaccination.Id;
            return CreatedAtAction(nameof(GetVaccination), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVaccination(int id, VaccinationDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var vaccination = await _context.Vaccinations.FindAsync(id);
            if (vaccination == null) return NotFound();
            vaccination.PetId = dto.PetId;
            vaccination.Number = dto.Number;
            vaccination.Type = dto.Type;
            vaccination.Date = dto.Date;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVaccination(int id)
        {
            var vaccination = await _context.Vaccinations.FindAsync(id);
            if (vaccination == null) return NotFound();
            _context.Vaccinations.Remove(vaccination);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 