using AddressBook.Api.Facades;
using AddressBook.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Api.Controllers;

[ApiController]
[Route("api/contacts")]
public sealed class ContactsController(IContactsFacade facade) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<ContactDto>>> GetPage(
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (page < 1 || pageSize < 1 || pageSize > 100)
            return BadRequest("page must be >= 1 and pageSize must be between 1 and 100.");
        return Ok(await facade.GetPageAsync(search, page, pageSize, cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ContactDto>> GetById(
        Guid id, CancellationToken cancellationToken)
    {
        var contact = await facade.GetByIdAsync(id, cancellationToken);
        return contact is null ? NotFound() : Ok(contact);
    }

    [HttpPost]
    public async Task<ActionResult<ContactDto>> Create(
        [FromBody] CreateContactRequest request, CancellationToken cancellationToken)
    {
        var created = await facade.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}
