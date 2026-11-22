using AddressBook.Api.Models;

namespace AddressBook.Api.Providers;

public interface IContactsProvider
{
    Task<(IReadOnlyList<Contact> Items, int TotalCount)> GetPageAsync(
        string? search, int page, int pageSize, CancellationToken cancellationToken);
    Task<Contact?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Contact> AddAsync(Contact contact, CancellationToken cancellationToken);
}
