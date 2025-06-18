using WebApplication1.Models;

namespace WebApplication1.Repsitory
{
    public class OwnerRepository : IOwnerRepository
    {
        AppDbContext _context;
        public OwnerRepository(AppDbContext context) { 
        _context = context;
        
        }

        public bool customerRequestProcess(int rentId,string requestStatus)
        {
            if (rentId != 0&& requestStatus != null)
            {
                CarRent _carRent= _context.Rents.FirstOrDefault(rent => rent.Id == rentId);
                if (_carRent != null)
                {
                    _carRent.requestStatus = requestStatus;
                    _context.Rents.Update(_carRent);
                    _context.SaveChanges();
                    return true;
                }

            }
            return false;
        }
       
    }
}
