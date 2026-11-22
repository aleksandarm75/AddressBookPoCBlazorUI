using AddressBook.Api.Data;
using AddressBook.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Api.Providers;

public sealed class EfContactsProvider(ContactsDbContext db) : IContactsProvider
{
    public async Task<(IReadOnlyList<Contact> Items, int TotalCount)> GetPageAsync(
        string? search, int page, int pageSize, CancellationToken cancellationToken)
    {
        IQueryable<Contact> query = db.Contacts.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLowerInvariant();
            query = query.Where(c =>
                c.FirstName.ToLower().Contains(term) ||
                c.LastName.ToLower().Contains(term) ||
                c.Street.ToLower().Contains(term) ||
                c.City.ToLower().Contains(term) ||
                c.Phone.Contains(term));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ThenBy(c => c.Id)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .ToListAsync(cancellationToken);
        return (items, totalCount);
    }

    public Task<Contact?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Contacts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<Contact> AddAsync(Contact contact, CancellationToken cancellationToken)
    {
        db.Contacts.Add(contact);
        await db.SaveChangesAsync(cancellationToken);
        return contact;
    }
}
