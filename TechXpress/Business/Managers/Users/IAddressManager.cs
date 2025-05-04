using Business.DTOs.Users;
using DataAccess.Entities;

namespace Business.Managers.Users;

public interface IAddressManager
{
    Task AddAddress(int userId, AddressDto addressDto);
    Task DeleteAddress(int userId, int addressId);
    Task<List<AddressDto>> GetAllAddresses();
    Task<List<UserAddress>> GetAddresses(int userId);



}