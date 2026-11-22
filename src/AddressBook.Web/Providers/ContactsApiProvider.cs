using System.Net;
using System.Net.Http.Json;
using AddressBook.Contracts;

namespace AddressBook.Web.Providers;

public sealed class ContactsApiProvider(HttpClient http) : IContactsApiProvider
{
    public async Task<PagedResult<ContactDto>> GetPageAsync(string? search, int page, int pageSize)
    {
        var url = $"api/contacts?page={page}&pageSize={pageSize}";
        if (!string.IsNullOrWhiteSpace(search))
            url += "&search=" + Uri.EscapeDataString(search.Trim());
        return await http.GetFromJsonAsync<PagedResult<ContactDto>>(url)
            ?? throw new InvalidOperationException("The API did not return paginated data.");
    }

    public async Task<ContactDto?> GetByIdAsync(Guid id)
    {
        using var response = await http.GetAsync($"api/contacts/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ContactDto>();
    }

    public async Task<ContactDto> CreateAsync(CreateContactRequest request)
    {
        using var response = await http.PostAsJsonAsync("api/contacts", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ContactDto>()
            ?? throw new InvalidOperationException("The API did not return the created contact.");
    }
}
