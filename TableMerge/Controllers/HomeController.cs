using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TableMerge.Models;
using TableMerge.Repository;

namespace TableMerge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IMergeRepository mergeRepository;

        public HomeController(IMergeRepository merge)
        {
            mergeRepository=merge;
        }

        [HttpPost]
        public async Task<ActionResult> MergeStudentData([FromBody] Student student)
        {
            var res=await mergeRepository.MergeStudentData(student.ID,student.Name);

            if (res != null)
            {
                return Ok(res);
            }
            else
            {
                return StatusCode(500, res);
            }
        }
    }
}
