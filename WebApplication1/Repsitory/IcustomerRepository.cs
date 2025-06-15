using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repsitory
{
    public interface IcustomerRepository
    {

        //get all requests
        //get all requests -->request status
        //add request 
        //Edit --> if it pending
        //delete
        public List<CarRent> getAllRentRequests(string id);
        public List<CarRent> getAllRentRequestsByStatus(string status);

        public bool addRequest(string ID, carRequestDto request);
        public bool editRequest(int id, carRequestDto request);
        public bool removeRequest(int id);



    }
}
