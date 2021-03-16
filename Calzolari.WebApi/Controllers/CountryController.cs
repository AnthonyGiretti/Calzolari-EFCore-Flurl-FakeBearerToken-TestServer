using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Calzolari.WebApi.Database;
using Calzolari.WebApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace Calzolari.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly DemoDbContext _dbContext;

        public CountryController(DemoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<Country> Get()
        {
            return _dbContext.Countries;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var country = _dbContext.Countries.FirstOrDefault(c => c.CountryId == id);

            if (country != null)
                return Ok(country);

            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Post([FromBody] Country country)
        {
            _dbContext.Countries.Add(country);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = country.CountryId }, country);
        }
    }
}