using DataAccess.Entities;

namespace Business.DTOs.Users;

public class AddressDto
{
    public int Id { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string BuildingNumber { get; set; }
    public string ApartmentNumber { get; set; }
}

public static class AddressDtoExtensions
{
    public static AddressDto ToDto(this UserAddress address)
    {
        return new AddressDto
        {
            Id = address.AddressId,
            Country = address.Country,
            City = address.City,
            Street = address.Street,
            BuildingNumber = address.BuildingNumber,
            ApartmentNumber = address.ApartmentNumber
        };
    }

    public static UserAddress ToEntity(this AddressDto dto)
    {
        return new UserAddress
        {
            AddressId = dto.Id,
            Country = dto.Country,
            City = dto.City,
            Street = dto.Street,
            BuildingNumber = dto.BuildingNumber,
            ApartmentNumber = dto.ApartmentNumber
        };
    }
}