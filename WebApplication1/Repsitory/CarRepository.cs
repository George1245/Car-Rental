using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repsitory
{
    public class CarRepository : ICarRepository
    {
        public IMapper _mapper;

        public AppDbContext _appContext;
        public CarRepository(AppDbContext _appContext, IMapper _mapper)
        {
            this._appContext = _appContext;
            this._mapper = _mapper; 
        }
        public bool addCar(CarDTO _car)
        {
            Car car = _mapper.Map<Car>(_car);

            if (car != null)
            { 
             _appContext.Cars.Add(car);
             _appContext.SaveChanges();
            return true;
           
            }
            return false;

        }

        public bool editCar(int id, CarDTO _car)
        {

            if (id != 0)
            {
               var car= _appContext.Cars.FirstOrDefault(c => c.Id == id);
                if (car != null)
                {

                    Car carMapper = _mapper.Map(_car,car);

               //car.Year = carMapper.Year;
                    //car.Capacity = carMapper.Capacity;
                    //car.Model= carMapper.Model;
                    //car.Availability= _car.Availability;
                    //car.Quantity= _car.Quantity;
                    //car.Licesnse_Plate=_car.Licesnse_Plate;
                    //car.Type= _car.Type;
                    //car.Trasnsmission= _car.Trasnsmission;
                    //car.CompanyName= _car.CompanyName;
                    _appContext.Cars.Update(carMapper);
                    _appContext.SaveChanges();
                    return true;
                }

                return false;

            }
            return false;
        }

        public List<Car> getAllCars()
        {
         return _appContext.Cars.ToList();
        
        }

        public Car getCarById(int id)
        {
            if (id != 0)
            {
                if (_appContext.Cars.FirstOrDefault(c => c.Id == id) != null)
                {
                    return _appContext.Cars.FirstOrDefault(c => c.Id == id);

                }
                return null;

            }
            return null;
        }
        
        public List<Car> getCarByType(string Type)
        {
            if (Type != null)
            {
                return _appContext.Cars.Where(c => c.Type == Type).ToList();
            }
            return null;
        
        }

        public bool removeCar(int id)
        {
            if (id != 0 && _appContext.Cars.FirstOrDefault(c => c.Id == id) != null)
            {
                
                _appContext.Cars.Remove(_appContext.Cars.FirstOrDefault(c => c.Id == id));
                _appContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
