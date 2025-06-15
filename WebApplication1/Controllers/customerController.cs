using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repsitory;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class customerController : ControllerBase
    {
        IcustomerRepository customerRepository;
        public customerController(IcustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }
        [Authorize(Roles ="User")]
        [HttpPost(nameof(addCarRequest))]
        public ActionResult addCarRequest(carRequestDto Rent)
        {
            if (ModelState.IsValid&&customerRepository.addRequest(User.FindFirstValue(ClaimTypes.NameIdentifier),Rent))
            {
                return Ok("your request is pending please be wait patiently until the request be accepted");
            }
            return BadRequest("there is a problem in your request or maybe the car is not available righ now!!");
        }

        [Authorize(Roles ="User")]
        [HttpPost(nameof(RemoveCarRequest))]
        public ActionResult RemoveCarRequest(int  id)
        {
            if (ModelState.IsValid&&customerRepository.removeRequest(id))
            {
                return Ok("your pending request is removed!!");
            }
            return BadRequest("there is error in removing rent request or car is not pending!!");
        }

        [Authorize(Roles = "User")]
        [HttpPost(nameof(editCarRequest))]
        public ActionResult editCarRequest(int id,carRequestDto Rent)
        {
            if (ModelState.IsValid && customerRepository.editRequest(id, Rent))
            {
                return Ok("your request is Edited and pending please be wait patiently until the request be accepted");
            }
            return BadRequest("there is a problem in your request edit or maybe car is not pending!!!!");
        }

        [Authorize(Roles = "User")]
        [HttpPost(nameof(getAllRentRequests))]
        public ActionResult getAllRentRequests()
        {
            if (ModelState.IsValid)
            {
                return Ok(customerRepository.getAllRentRequests(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }
            return BadRequest("there is a problem in your request!!");
        }

        [Authorize(Roles = "User")]
        [HttpPost(nameof(getAllRentRequestsByStatus))]
        public ActionResult getAllRentRequestsByStatus(string status)
        {
            if (ModelState.IsValid)
            {
                return Ok(customerRepository.getAllRentRequestsByStatus(status));
            }
            return BadRequest($"there is a problem in your request or there is no requests in the {status} request");
        }
    }
}
