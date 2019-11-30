using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCodeApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdventOfCodeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdventOfCodeController : ControllerBase
    {
        private readonly IAdventOfCodeService _adventOfCodeService;

        public AdventOfCodeController(IAdventOfCodeService adventOfCodeService)
        {
            _adventOfCodeService = adventOfCodeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.AocMember>>> Get()
        {
            return await _adventOfCodeService.GetAsync();
        }
    }
}
