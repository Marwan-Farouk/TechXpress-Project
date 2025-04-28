using System.ComponentModel.DataAnnotations;

namespace Presentation.ActionRequests;

public class AddAddressActionRequest
{
    [Required]
    public string Country { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string Street { get; set; }
    [Required]
    public string BuildingNumber { get; set; }
    [Required]
    public string ApartmentNumber { get; set; }
}