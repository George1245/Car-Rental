
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repsitory
{
    public class CustomerRepository:IcustomerRepository 
    {
        public AppDbContext _Context;
        CarRent Rent;
        public CustomerRepository(AppDbContext _Context,CarRent Rent)
        {
            this._Context = _Context;
            this.Rent = Rent;
        }
        //get all requests
        //get all requests -->request status
        //add request 
        //Edit --> if it pending
        //delete

        //get all rental requests
        public bool addRequest(string ID,carRequestDto request)
        {
            if (request != null)
            {
                Car _car = _Context.Cars.FirstOrDefault(x => x.Type == request.Type && x.Model== request.Model);       
                var dbCarCount=_Context.Rents.Where(rent => rent.CarId == _car.Id).Count();
                //check if all cars quantity and cars in db is rented then make this car availability false
                if (_car != null && dbCarCount<=_car.Quantity&&dbCarCount!=0)
                {
                    Rent.car = _car;
                    Rent.RentalDuration = request.RentalDuration;
                    Rent.requestStatus = "pending";
                    Rent.UserId = ID;
                    Rent.totalCost = _car.costPerHour * request.RentalDuration;
                    _Context.Rents.Add(Rent);
                    _Context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool editRequest(int id, carRequestDto request)
        {
            if (id != 0)
            {
                CarRent rent = _Context.Rents.Include(c => c.car).FirstOrDefault(x => x.Id == id);
                if (request != null && rent.requestStatus == "pending")
                {
                    Car _car = _Context.Cars.FirstOrDefault(x => x.Type == request.Type && x.Model == request.Model);
                    if (_car != null)
                    {
                        rent.car = _car;
                        rent.RentalDuration = request.RentalDuration;
                        rent.totalCost = request.RentalDuration * _car.costPerHour;
                        _Context.Rents.Update(rent);
                        _Context.SaveChanges();
                        return true;
                    }
                }
            }
            return false;

        }

        public List<CarRent> getAllRentRequests(string id)
        {
            return _Context.Rents.Include(c => c.car).Where(rent=>rent.UserId==id).ToList();
        }

        public List<CarRent> getAllRentRequestsByStatus(string status)
        {
            if(status!=null)
            {
                     return _Context.Rents.Include(c => c.car).Where(c=>c.requestStatus==status).ToList();

            }
            return null;
        }

        public bool removeRequest(int id)
        {
            CarRent rentRequest = _Context.Rents.FirstOrDefault(Rent => Rent.Id == id);
           if(rentRequest != null&& rentRequest.requestStatus=="pending")
            {
                _Context.Rents.Remove(rentRequest);
                _Context.SaveChanges();
                return true;
            }
            return false;
        }




    }
}
