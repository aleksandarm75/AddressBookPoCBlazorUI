using AddressBook.Api.Models;
using AddressBook.Api.Providers;
using AddressBook.Contracts;

namespace AddressBook.Api.Facades;

public sealed class ContactsFacade(IContactsProvider provider) : IContactsFacade
{
    public async Task<PagedResult<ContactDto>> GetPageAsync(
        string? search, int page, int pageSize, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await provider.GetPageAsync(search, page, pageSize, cancellationToken);
        return new PagedResult<ContactDto>(items.Select(ToDto).ToList(), page, pageSize, totalCount);
    }

    public async Task<ContactDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var contact = await provider.GetByIdAsync(id, cancellationToken);
        return contact is null ? null : ToDto(contact);
    }

    public async Task<ContactDto> CreateAsync(CreateContactRequest request, CancellationToken cancellationToken)
    {
        var contact = new Contact
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Street = request.Street.Trim(),
            City = request.City.Trim(),
            Phone = request.Phone.Trim(),
            Email = request.Email.Trim()
        };
        return ToDto(await provider.AddAsync(contact, cancellationToken));
    }

    private static ContactDto ToDto(Contact c) =>
        new(c.Id, c.FirstName, c.LastName, c.Street, c.City, c.Phone, c.Email);
}
