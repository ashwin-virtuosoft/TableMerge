using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TableMerge.Models;
using TableMerge.Repository;

namespace TableMerge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckSumController : ControllerBase
    {
        private readonly CheckSumRepository _checkSumGenerator;
        private readonly ILogger<CheckSumController> _logger;
        public CheckSumController(CheckSumRepository checkSumGenerator, ILogger<CheckSumController> logger)
        {
            _checkSumGenerator = checkSumGenerator;
            _logger = logger;

        }

        [HttpPost("CheckSum")]

        public IActionResult Post([FromBody] Person person)
        {
            if (person == null)
            {
                return BadRequest("Person object is null");
            }

            _checkSumGenerator.InsertPerson(person);
            return Ok("Person record inserted successfully");
        }


        [HttpGet("compare")]
        public IActionResult Compare(int personId, string firstName, string lastName, string birthDate)
        {
            bool isMatch = _checkSumGenerator.CompareChecksum(personId, firstName, lastName, birthDate);

            if (isMatch)
            {
                _logger.LogInformation("Checksum matches for PersonID {PersonID}", personId);
                return Ok("Checksum matches.");
            }
            else
            {
                _logger.LogWarning("Checksum does not match for PersonID {PersonID}", personId);
                return BadRequest("Checksum does not match.");
            }
        }
    }
}
