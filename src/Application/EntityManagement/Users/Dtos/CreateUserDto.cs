using Application.EntityManagement.Addresses.Dtos;
using Application.EntityManagement.PhoneNumbers.Dtos;

namespace Application.EntityManagement.Users.Dtos;

public sealed record CreateUserDto
(
    string FirstName,
    string LastName,
    DateTime BirthDate,
    string Username,
    string Password,
    string PasswordConfirmation,
    string Email,
    IEnumerable<AddressDto> AddressDtos,
    IEnumerable<PhoneNumberDto> PhoneNumberDtos
);