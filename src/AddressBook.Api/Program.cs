using AddressBook.Api.Data;
using AddressBook.Api.Facades;
using AddressBook.Api.Providers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<ContactsDbContext>(options =>
    options.UseInMemoryDatabase("AddressBookPoC"));
builder.Services.AddScoped<IContactsProvider, EfContactsProvider>();
builder.Services.AddScoped<IContactsFacade, ContactsFacade>();
builder.Services.AddCors(options => options.AddPolicy("BlazorClient", policy =>
    policy.WithOrigins("http://localhost:5081", "https://localhost:7081")
          .AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ContactsDbContext>();
    await db.Database.EnsureCreatedAsync();
    await SampleData.SeedAsync(db);
}

app.UseCors("BlazorClient");
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "AddressBook.Api" }));
app.Run();
