using AddressBook.Contracts;
using AddressBook.Web.Providers;

namespace AddressBook.Web.Facades;

public sealed class ContactsUiFacade(IContactsApiProvider provider) : IContactsUiFacade
{
    public Task<PagedResult<ContactDto>> SearchAsync(string? search, int page, int pageSize) =>
        provider.GetPageAsync(search, page, pageSize);
    public Task<ContactDto?> GetDetailsAsync(Guid id) => provider.GetByIdAsync(id);
    public Task<ContactDto> AddAsync(CreateContactRequest request) => provider.CreateAsync(request);
}
