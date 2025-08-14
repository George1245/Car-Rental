using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repsitory;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        ICarRepository _carRepo;
        public CarController(ICarRepository carRepo)
        { 
        _carRepo = carRepo;
        }

        [Authorize(Roles = "User")]

        [HttpGet(nameof(getCarById))]
        public ActionResult getCarById(int id)
        {
            if(_carRepo.getCarById(id) != null)
            return Ok(_carRepo.getCarById(id));

            return BadRequest("There is no car with this Id");
        }

        [Authorize(Roles ="User,Admin")]
        [HttpGet(nameof(getAllCars))]
        public ActionResult getAllCars()
        {
            if(_carRepo.getAllCars()!= null)
            return Ok(_carRepo.getAllCars());
            return BadRequest("There is no Cars yet!!!");
        }
        [Authorize(Roles = "User")]

        [HttpGet(nameof(getCarByType))]
        public ActionResult getCarByType(string Type)
        {
            if (_carRepo.getCarByType(Type) != null)
                return Ok(_carRepo.getCarByType(Type));
            return BadRequest("There is no Cars yet!!!");

        }

     


    }
}
