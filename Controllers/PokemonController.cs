using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    //THE CONTROLLER IS USED TO EXPOSE THE FUNCTIONALITY OF THE APPLICATION WHICH CAN BE
    //USED BY EXTERNAL ENTITIES
    

    //THESE ARE ATTRIBUTES WHICH ARE GOING TO MAKE A CONTROLLER - A CONTROLLER
    //OTHERWISE THE WEB API WON'T BE ABLE TO RECOGNIZE THESE AS CONTROLLER
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        //BRINGING IN OUR REPOSITORY
        public PokemonController(IPokemonRepository pokemonRepository, 
            IOwnerRepository ownerRepository, IReviewRepository reviewRepository, 
            IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _ownerRepository = ownerRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());
            //MODELSTATE CHECKS IF THE DATA RETRIEVED IS CORRECT OR NOT
            //IT IS A FORM OF VALIDATION
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);   
        }

        [HttpGet("{pokeId}")]
        //PRODUCESRESPONSETYPE JUST MAKES YOUR API ENDPOINT LOOK BETTER THAT'S IT
        //IT DOESN'T REALLY DO ANYTHING SIGNIFICANT AND IS NOT ALWAYS NECESSARY
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            return Ok(pokemon);
        }

        [HttpGet("{pokeId}/Rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            var rating = _pokemonRepository.GetPokemonRating(pokeId);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(rating);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, 
            [FromBody] PokemonDto pokemonCreate)
        {
            if (pokemonCreate == null)
                return BadRequest(ModelState);

            var pokemons = _pokemonRepository.GetPokemons().Where(p => p.Name.Trim().ToUpper() ==
            pokemonCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if(pokemons != null)
            {
                ModelState.AddModelError("", "Pokemon Already Exists!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);

            if(!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving!");
                return StatusCode(422, ModelState);
            }

            return Ok("Successfully created!");
        }

        [HttpPut("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int pokeId, [FromQuery] int ownerId,
            [FromQuery] int categoryId, [FromBody] PokemonDto updatedPokemon)
        {
            if (updatedPokemon == null)
                return BadRequest();

            if (pokeId != updatedPokemon.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(updatedPokemon);

            if(!_pokemonRepository.UpdatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Successfully");
        }

        [HttpDelete("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            var reviewsToDelete = _reviewRepository.GetReviewOfAPokemon(pokeId);
            var pokemonToDelete = _pokemonRepository.GetPokemon(pokeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }


            if(!_pokemonRepository.DeletePokemon(pokemonToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted Successfully");
        }
    }
}
