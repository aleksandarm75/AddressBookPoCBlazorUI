using AddressBook.Contracts;

namespace AddressBook.Api.Facades;

public interface IContactsFacade
{
    Task<PagedResult<ContactDto>> GetPageAsync(string? search, int page, int pageSize, CancellationToken cancellationToken);
    Task<ContactDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ContactDto> CreateAsync(CreateContactRequest request, CancellationToken cancellationToken);
}
