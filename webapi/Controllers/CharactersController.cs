using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DnDAPI.Models;
using System.Net;
using DnDAPI.Contracts;

namespace DnDApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterRepository _repo;
        protected ResponseHandler _response;

        public CharactersController(ICharacterRepository repo)
        {
            _repo = repo;
            _response = new ResponseHandler();
        }

        // Need to make a DTO for this its wasteful responding with stats when they're not displayed
        [HttpGet]
        public async Task<ActionResult<ResponseHandler>> GetCharacters()
        {
            try
            {
                _response.Result = await _repo.GetAll();
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }

            return _response;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResponseHandler>> GetCharacter(int id)
        {
            try
            {
                if (_repo == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var character = await _repo.Get(c => c.CharacterId == id);

                if (character == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = character;
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }

            return _response;
        }

        [HttpGet("search")]
        public async Task<ActionResult<ResponseHandler>> GetCharactersByName([FromQuery] string name)
        {
            try
            {
                if (_repo == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = await _repo.GetAll(c => c.Name.StartsWith(name));
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }

            return _response;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ResponseHandler>> PutCharacter(int id, Character character)
        {
            try
            {
                if (id != character.CharacterId)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest();
                }

                try
                {
                    await _repo.Update(character);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacterExists(id))
                    {
                        _response.StatusCode = HttpStatusCode.NotFound;
                        return NotFound(_response);
                    }
                    else
                    {
                        throw;
                    }
                }

                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }

            return _response;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseHandler>> PostCharacter(Character character)
        {
            try
            {
                await _repo.Create(character);
                _response.Result = character;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtAction("GetCharacter", new { id = character.CharacterId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }

            return _response;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ResponseHandler>> DeleteCharacter(int id)
        {
            try
            {
                if (_repo == null)
                {
                    return NotFound();
                }

                var character = await _repo.Get(c => c.CharacterId == id);

                if (character == null)
                {
                    return NotFound();
                }

                await _repo.Remove(character);
                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }

            return _response;
        }

        private bool CharacterExists(int id)
        {
            var character = _repo.Get(c => c.CharacterId == id);

            return character != null;
        }
    }
}