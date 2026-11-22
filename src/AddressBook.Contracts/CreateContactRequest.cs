using System.ComponentModel.DataAnnotations;

namespace AddressBook.Contracts;

public sealed class CreateContactRequest
{
    [Required, StringLength(60)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(60)]
    public string LastName { get; set; } = string.Empty;

    [Required, StringLength(120)]
    public string Street { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string City { get; set; } = string.Empty;

    [Required, StringLength(40)]
    public string Phone { get; set; } = string.Empty;

    [EmailAddress, StringLength(120)]
    public string Email { get; set; } = string.Empty;
}
