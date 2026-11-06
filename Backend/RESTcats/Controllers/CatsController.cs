using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RESTcats.Models;
using RESTcats.Repositories;

namespace RESTcats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatsController : ControllerBase
    {
        private CatsRepositoryList _repo;

        public CatsController(CatsRepositoryList repo)
        {
            _repo = repo;
        }

        // GET: api/<CatsController>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        
        [Authorize(Roles = "User, Admin")]
        public ActionResult<IEnumerable<Cat>> Get(
            [FromQuery] int minimumweight,
            [FromQuery] int? maximumweight,
            [FromQuery] string? nameFilter)
        {
            try
            {
                IEnumerable<Cat> result = _repo.GetAllCats(minimumweight
                    , maximumweight, nameFilter);
                if (result == null || result.Count() == 0)
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<CatsController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        [Authorize(Roles = "User, Admin")]
        public ActionResult<Cat> Get(int id)
        {
            Cat? cat = _repo.GetCatById(id);
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }

        // POST api/<CatsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Roles = "Admin")]
        public ActionResult<Cat> Post([FromBody] Cat newCat)
        {
            Cat theCat = _repo.AddCat(newCat);
            string uri = Url.RouteUrl(RouteData.Values) + "/" + theCat.Id;
            return Created(uri, theCat);
        }

        // PUT api/<CatsController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public ActionResult<Cat> Put(int id, [FromBody] Cat value)
        {
            Cat? updatedCat= _repo.UpdateCat(id, value);
            if (updatedCat == null) { return NotFound("No such cat, id: " + id); }
            return Ok(updatedCat);
        }

        // DELETE api/<CatsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public ActionResult<Cat> Delete(int id)
        {
            Cat? removedCat =_repo.RemoveCat(id);
            if (removedCat == null) { return NotFound("No such cat, id: " + id); }
            return Ok(removedCat);
        }

        /*[HttpOptions]
        public void Options()
        {
        }*/

    }
}
