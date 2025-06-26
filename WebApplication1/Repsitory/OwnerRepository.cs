using MailKit;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Repsitory
{
    public class OwnerRepository : IOwnerRepository
    {
        AppDbContext _context;
        private mailService _mailService;
        public OwnerRepository(AppDbContext context, mailService _mailService) { 
        _context = context;
            this._mailService = _mailService;
        }

        public async Task<bool> customerRequestProcess(int rentId,string requestStatus)
        {
            if (rentId != 0&& requestStatus != null)
            {
                CarRent _carRent= _context.Rents.Include(rent=>rent.User).FirstOrDefault(rent => rent.Id == rentId);
                if (_carRent != null)
                {
           
                        _carRent.requestStatus = requestStatus;
                    _context.Rents.Update(_carRent);
                    _context.SaveChanges();
                    if (requestStatus == "Accepted")
                    {
                        bool checkSend =await _mailService.sendEmail(_carRent.User.Email, "Your Rental Request Is Accepted", _carRent.User.UserName, "Car Rental Request");
                        if (checkSend)
                        {
                            return true;

                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (requestStatus == "Rejected")
                    {
                        bool checkSend = await _mailService.sendEmail(_carRent.User.Email, "Your Rental Request Is Rejected", _carRent.User.UserName, "Car Rental Request");
                        if (checkSend)
                        {
                            return true;

                        }
                        else
                        {
                            return false;
                        }
                    }
                    
                }

            }
            return false;
        }
       
    }
}
