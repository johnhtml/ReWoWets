using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReWoWets.Models;

namespace ReWoWets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicalServiceController : ControllerBase
    {
        private readonly ReWowDbContext _context;
        public MedicalServiceController(ReWowDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalServiceDto>>> GetMedicalServices()
        {
            return await _context.MedicalServices
                .Select(m => new MedicalServiceDto
                {
                    Id = m.Id,
                    PetId = m.PetId,
                    ServiceType = m.ServiceType,
                    Date = m.Date
                }).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalServiceDto>> GetMedicalService(int id)
        {
            var m = await _context.MedicalServices.FindAsync(id);
            if (m == null) return NotFound();
            return new MedicalServiceDto
            {
                Id = m.Id,
                PetId = m.PetId,
                ServiceType = m.ServiceType,
                Date = m.Date
            };
        }

        [HttpPost]
        public async Task<ActionResult<MedicalServiceDto>> CreateMedicalService(MedicalServiceDto dto)
        {
            var service = new MedicalService
            {
                PetId = dto.PetId,
                ServiceType = dto.ServiceType,
                Date = dto.Date
            };
            _context.MedicalServices.Add(service);
            await _context.SaveChangesAsync();
            dto.Id = service.Id;
            return CreatedAtAction(nameof(GetMedicalService), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicalService(int id, MedicalServiceDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var service = await _context.MedicalServices.FindAsync(id);
            if (service == null) return NotFound();
            service.PetId = dto.PetId;
            service.ServiceType = dto.ServiceType;
            service.Date = dto.Date;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalService(int id)
        {
            var service = await _context.MedicalServices.FindAsync(id);
            if (service == null) return NotFound();
            _context.MedicalServices.Remove(service);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 