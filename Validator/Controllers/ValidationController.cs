using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Validator.Repositories;

namespace Validator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        static IValidationRepository _validationRepository;
        public ValidationController(IValidationRepository validationRepository)
        {
            _validationRepository = validationRepository;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public ActionResult<bool> GetFromParameter([Required] string input, bool base64 = false)
        {
            if (base64)
            {
                string decodedInput = "";
                try
                {
                    decodedInput = DecodeBase64(input);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

                return _validationRepository.ValidateRegex(decodedInput);
            }
            else
            {
                return _validationRepository.ValidateRegex(input);
            }
        }

        // While more aligned with RESTful architechture - message bodies under GET aren't kosher to all consumers
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[HttpGet]
        [HttpPost]
        public ActionResult<ValidationResponse> PostFromBody([Required] [FromBody] ValidationRequest request)
        {
            ValidationResponse response = new ValidationResponse();

            if (request.base64)
            {
                string decodedInput = "";
                try
                {
                    decodedInput = DecodeBase64(request.input);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

                response.result = _validationRepository.ValidateRegex(decodedInput);
            }
            else
            {
                response.result = _validationRepository.ValidateRegex(request.input);
            }

            return response;
        }
        private string DecodeBase64(string input)
        {
            byte[] data = System.Convert.FromBase64String(input);
            return System.Text.Encoding.UTF8.GetString(data);
        }
    }

    public class ValidationRequest
    {
        [Required] public string input { get; set; }
        [Required] public bool base64 { get; set; }
    }
    public class ValidationResponse
    {
        public bool result { get; set; }
    }
}