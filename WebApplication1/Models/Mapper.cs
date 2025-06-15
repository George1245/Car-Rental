using AutoMapper;
using WebApplication1.DTO;

namespace WebApplication1.Models
{
    public class Mapper : Profile
    {
        public Mapper()
        {

            CreateMap<CarDTO, Car>();
        }
    }
}
