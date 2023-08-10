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
    public class AbilitiesController : ControllerBase
    {
        private readonly IAbilityRepository _repo;
        protected ResponseHandler _response;

        public AbilitiesController(IAbilityRepository repo)
        {
            _repo = repo;
            _response = new ResponseHandler();
        }

        [HttpGet]
        public async Task<ActionResult<ResponseHandler>> GetAbilities()
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
        public async Task<ActionResult<ResponseHandler>> GetAbility(int id)
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

                var ability = await _repo.Get(a => a.AbilityId == id);

                if (ability == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = ability;
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
        public async Task<ActionResult<ResponseHandler>> GetAbilitiesByName([FromQuery] string name)
        {
            try
            {
                if (_repo == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = await _repo.GetAll(a => a.Name.StartsWith(name));
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
        public async Task<ActionResult<ResponseHandler>> PutAbility(int id, Ability ability)
        {
            try
            {
                if (id != ability.AbilityId)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest();
                }

                try
                {
                    await _repo.Update(ability);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AbilityExists(id))
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
        public async Task<ActionResult<ResponseHandler>> PostAbility(Ability ability)
        {
            try
            {
                await _repo.Create(ability);
                _response.Result = ability;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtAction("GetAbility", new { id = ability.AbilityId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }

            return _response;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ResponseHandler>> DeleteAbility(int id)
        {
            try
            {
                if (_repo == null)
                {
                    return NotFound();
                }

                var ability = await _repo.Get(c => c.AbilityId == id);

                if (ability == null)
                {
                    return NotFound();
                }

                await _repo.Remove(ability);
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

        private bool AbilityExists(int id)
        {
            var ability = _repo.Get(a => a.AbilityId == id);

            return ability != null;
        }
    }
}
