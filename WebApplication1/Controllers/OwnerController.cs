using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Repsitory;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        public ICarRepository _carRepo;
        public IOwnerRepository _ownerRepo;
        public OwnerController(ICarRepository carRepo,IOwnerRepository ownerRepo)
        {
        _carRepo = carRepo;

        _ownerRepo = ownerRepo;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost(nameof(customerRequestProcess))]
        public async Task <ActionResult> customerRequestProcess(int rentID,string requestStatus)
        {

            if (await _ownerRepo.customerRequestProcess(rentID,requestStatus))
                return Ok("Your request Status"+requestStatus+" is updated!!");
            return BadRequest("Failed to update request");
        }



        [Authorize(Roles ="Admin")]
        [HttpPost(nameof(addCar))]
        public ActionResult addCar(CarDTO _car)
        {
            if (_carRepo.addCar(_car))
                return Ok("Car is added!");
            return BadRequest("There is no Car added!");
        }
        [Authorize(Roles = "Admin")]

        [HttpPost(nameof(editCar))]
        public ActionResult editCar(int id, CarDTO _car)
        {
            if (_carRepo.editCar(id, _car))
                return Ok("Car is updated!");
            return BadRequest("There is no Cars updated!");

        }
        [Authorize(Roles = "Admin")]

        [HttpPost(nameof(removeCar))]
        public ActionResult removeCar(int id)
        {
            if (_carRepo.removeCar(id))
                return Ok("Car is removed!!!");
            return BadRequest("There is no Cars removed!!!");
        }







    }
}
