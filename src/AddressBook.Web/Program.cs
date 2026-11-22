using AddressBook.Web;
using AddressBook.Web.Controllers;
using AddressBook.Web.Facades;
using AddressBook.Web.Providers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5080/";
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
builder.Services.AddScoped<IContactsApiProvider, ContactsApiProvider>();
builder.Services.AddScoped<IContactsUiFacade, ContactsUiFacade>();
builder.Services.AddScoped<ContactsPageController>();

await builder.Build().RunAsync();
