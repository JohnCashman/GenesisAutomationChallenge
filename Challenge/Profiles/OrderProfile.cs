using AutoMapper;
using Challenge.DTOs;
using Challenge.Entities;

namespace Challenge.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderSummaryDTO>().ForMember(m => m.FullName, o => o.MapFrom(f => f.Customer.FirstName + " " + f.Customer.LastName));
        }
    }
}
