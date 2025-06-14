using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repsitory
{
    public interface ICarRepository
    {
        public Car getCarById(int id);
        public List<Car> getAllCars();
        public List<Car> getCarByType(string Type);
        public bool addCar(CarDTO car);
        public bool editCar(int id, CarDTO _car);
        public bool removeCar(int id);

    }
}
