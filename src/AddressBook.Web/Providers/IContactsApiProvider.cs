using AddressBook.Contracts;

namespace AddressBook.Web.Providers;

public interface IContactsApiProvider
{
    Task<PagedResult<ContactDto>> GetPageAsync(string? search, int page, int pageSize);
    Task<ContactDto?> GetByIdAsync(Guid id);
    Task<ContactDto> CreateAsync(CreateContactRequest request);
}
