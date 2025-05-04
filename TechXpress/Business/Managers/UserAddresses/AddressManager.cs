using Business.DTOs.Users;
using DataAccess.Entities;
using DataAccess.Repositories.USERADDRESS;
namespace Business.Managers.Users;

public class AddressManager : IAddressManager
{
    private readonly IUserAddressRepository _userAddressRepository;

    public AddressManager(IUserAddressRepository userAddressRepository)
    {
        _userAddressRepository = userAddressRepository;
    }
    public async Task AddAddress(int userId ,AddressDto addressDto )
    {
        var userAddress = addressDto.ToEntity();
        userAddress.UserId = userId;
        // userAddress.AddressId = await _userAddressRepository.GetMaxId() + 1;
        await _userAddressRepository.Add(userAddress);
    }
    public async Task DeleteAddress(int userId, int addressId)
    {
        await _userAddressRepository.Delete(userId, addressId);
    }
    public async Task<List<UserAddress>> GetAddresses(int userId)
    {
        return await _userAddressRepository.GetAddressesByUserId(userId);
    }

    public async Task<List<AddressDto>> GetAddressesByUserId(int id)
    {
        var addresses = await _userAddressRepository.GetAddressesByUserId(id);
        var dtos = addresses.Select(address => address.ToDto()).ToList();
        return dtos;
    }

    public async Task<List<AddressDto>> GetAllAddresses()
    {
        var addressesList = await _userAddressRepository.GetAll();
        var dtos = addressesList.Select(address => address.ToDto()).ToList();
        return dtos;
    }
}