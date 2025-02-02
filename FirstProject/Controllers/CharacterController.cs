﻿
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet("GetAll")]    //GET request
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()
        {
            return Ok(await _characterService.GetAllCharacters());
        }


        [HttpGet("{id}")]    //GET request
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id)
        {
            return Ok(await _characterService.GetCharacterById(id));
        }

        [HttpPost]

        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        { 
              return Ok(await _characterService.AddCharacter(newCharacter));
        }

        [HttpPut]

        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var response = await _characterService.UpdateCharacter(updatedCharacter);

            if (response.Data is null) {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var response = await _characterService.DeleteCharacter(id);

            if (response.Data is null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost("Skill")]

        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddCharacterSkill(AddCharacterskillDto newCharacterSkill)
        {
            return Ok(await _characterService.AddCharacterSkill(newCharacterSkill));
        }
     }
}
