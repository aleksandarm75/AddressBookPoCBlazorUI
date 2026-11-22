using AddressBook.Contracts;
using AddressBook.Web.Facades;

namespace AddressBook.Web.Controllers;

// Presentation controller (as opposed to the ASP.NET Core API controller on the backend).
public sealed class ContactsPageController(IContactsUiFacade facade)
{
    public const int DefaultPageSize = 10;
    public IReadOnlyList<ContactDto> Contacts { get; private set; } = [];
    public ContactDto? SelectedContact { get; private set; }
    public string? Error { get; private set; }
    public bool IsBusy { get; private set; }
    public int Page { get; private set; } = 1;
    public int PageSize => DefaultPageSize;
    public int TotalCount { get; private set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public int FirstItem => TotalCount == 0 ? 0 : (Page - 1) * PageSize + 1;
    public int LastItem => Math.Min(Page * PageSize, TotalCount);
    public string? CurrentSearch { get; private set; }

    public async Task LoadAsync(string? search, int page = 1)
    {
        if (page < 1) return;
        IsBusy = true;
        Error = null;
        try
        {
            var result = await facade.SearchAsync(search, page, PageSize);
            // If the last contact was deleted or the number of pages decreased,
            // go back to the last valid page.
            if (result.TotalCount > 0 && page > result.TotalPages)
                result = await facade.SearchAsync(search, result.TotalPages, PageSize);
            Contacts = result.Items;
            Page = result.Page;
            TotalCount = result.TotalCount;
            CurrentSearch = search;
            SelectedContact = null;
        }
        catch (Exception ex) { Error = "Failed to load contacts: " + ex.Message; }
        finally { IsBusy = false; }
    }

    public async Task SelectAsync(Guid id)
    {
        Error = null;
        try { SelectedContact = await facade.GetDetailsAsync(id); }
        catch (Exception ex) { Error = "Unable to load contact details: " + ex.Message; }
    }

    public void ClearSelection() => SelectedContact = null;

    public async Task<bool> AddAsync(CreateContactRequest request, string? currentSearch)
    {
        Error = null;
        try
        {
            await facade.AddAsync(request);
            await LoadAsync(currentSearch, Page);
            return Error is null;
        }
        catch (Exception ex)
        {
            Error = "Failed to add contact: " + ex.Message;
            return false;
        }
    }
}
