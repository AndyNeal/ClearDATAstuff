using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Validator.Repositories;

namespace Validator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BenchmarkController : ControllerBase
    {
        static IValidationRepository _validationRepository;

        public BenchmarkController(IValidationRepository validationRepository)
        {
            _validationRepository = validationRepository;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public ActionResult<object> GetBenchmark([Required] string input)
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            _validationRepository.ValidateRegex(input);
            sw.Stop();
            var timespanRegex = sw.Elapsed;

            sw.Restart();
            _validationRepository.ValidateLoop(input);
            sw.Stop();
            var timespanLoop = sw.Elapsed;

            sw.Restart();
            _validationRepository.ValidateAnd(input);
            sw.Stop();
            var timespanAnd = sw.Elapsed;

            sw.Restart();
            _validationRepository.ValidateLinq(input);
            sw.Stop();
            var timespanLinq = sw.Elapsed;

            return new
            {
                RegexTime = timespanRegex,
                LoopTime = timespanLoop,
                AndTime = timespanAnd,
                LinqTime = timespanLinq
            };
        }

    }
}