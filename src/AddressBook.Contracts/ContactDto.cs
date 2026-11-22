namespace AddressBook.Contracts;

public sealed record ContactDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Street,
    string City,
    string Phone,
    string Email);
