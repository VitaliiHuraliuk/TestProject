﻿using Microsoft.AspNetCore.Mvc;
using TestProject.Domain.Interfaces;
using TestProject.Models;

namespace TestProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogsController : ControllerBase
    {
        private readonly IDogsService _dogsService;

        public DogsController(IDogsService dogsService)
        {
            _dogsService = dogsService;
        }
        [HttpGet("get-dogs")]
        public async Task<IActionResult> GetDogs(string attribute = "weight", string order = "asc", int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Invalid pageNumber or pageSize values.");
            }

            var dogs = await _dogsService.GetDogs(attribute, order, pageNumber, pageSize);
            return Ok(dogs);
        }

        [HttpPost("create-dog")]
        public async Task<IActionResult> CreateDog([FromBody] DogEntity dog)
        {
            try
            {
                var createdDog = await _dogsService.CreateDog(dog);
                return Ok("The dog was successfully created.");
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is ArgumentException)
                {
                    return BadRequest(ex.Message);
                }

                return BadRequest("Error creating dog.");
            }
        }

        [HttpGet("ping")]
        public async Task<IActionResult> Ping()
        {
            var message = _dogsService.Ping();
            return Ok(message);
        }
    }
}
