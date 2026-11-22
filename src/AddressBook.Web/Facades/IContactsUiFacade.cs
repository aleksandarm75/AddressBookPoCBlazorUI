using AddressBook.Contracts;

namespace AddressBook.Web.Facades;

public interface IContactsUiFacade
{
    Task<PagedResult<ContactDto>> SearchAsync(string? search, int page, int pageSize);
    Task<ContactDto?> GetDetailsAsync(Guid id);
    Task<ContactDto> AddAsync(CreateContactRequest request);
}
