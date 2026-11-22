using AddressBook.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Api.Data;

public static class SampleData
{
    public static async Task SeedAsync(ContactsDbContext db)
    {
        if (await db.Contacts.AnyAsync()) return;
        // 30 potpuno fiktivnih USA kontakata. 555-01xx je rezervisan za fikciju.
        var contacts = new List<Contact>
        {
            new() { FirstName = "Emily", LastName = "Johnson", Street = "125 Maple Avenue", City = "New York, NY", Phone = "+1 (212) 555-0101", Email = "contact01@example.test" },
            new() { FirstName = "Michael", LastName = "Smith", Street = "84 Oak Street", City = "Los Angeles, CA", Phone = "+1 (213) 555-0102", Email = "contact02@example.test" },
            new() { FirstName = "Olivia", LastName = "Williams", Street = "330 Pine Road", City = "Chicago, IL", Phone = "+1 (312) 555-0103", Email = "contact03@example.test" },
            new() { FirstName = "James", LastName = "Brown", Street = "71 Cedar Lane", City = "Houston, TX", Phone = "+1 (713) 555-0104", Email = "contact04@example.test" },
            new() { FirstName = "Sophia", LastName = "Jones", Street = "42 Lakeview Drive", City = "Phoenix, AZ", Phone = "+1 (602) 555-0105", Email = "contact05@example.test" },
            new() { FirstName = "William", LastName = "Garcia", Street = "905 Sunset Boulevard", City = "Philadelphia, PA", Phone = "+1 (215) 555-0106", Email = "contact06@example.test" },
            new() { FirstName = "Ava", LastName = "Miller", Street = "18 Willow Court", City = "San Antonio, TX", Phone = "+1 (210) 555-0107", Email = "contact07@example.test" },
            new() { FirstName = "Benjamin", LastName = "Davis", Street = "207 Elm Street", City = "San Diego, CA", Phone = "+1 (619) 555-0108", Email = "contact08@example.test" },
            new() { FirstName = "Isabella", LastName = "Rodriguez", Street = "609 Birch Avenue", City = "Dallas, TX", Phone = "+1 (214) 555-0109", Email = "contact09@example.test" },
            new() { FirstName = "Lucas", LastName = "Martinez", Street = "53 Highland Road", City = "San Jose, CA", Phone = "+1 (408) 555-0110", Email = "contact10@example.test" },
            new() { FirstName = "Mia", LastName = "Hernandez", Street = "760 Forest Drive", City = "Austin, TX", Phone = "+1 (512) 555-0111", Email = "contact11@example.test" },
            new() { FirstName = "Henry", LastName = "Lopez", Street = "14 Spruce Street", City = "Jacksonville, FL", Phone = "+1 (904) 555-0112", Email = "contact12@example.test" },
            new() { FirstName = "Charlotte", LastName = "Gonzalez", Street = "284 Park Avenue", City = "Fort Worth, TX", Phone = "+1 (817) 555-0113", Email = "contact13@example.test" },
            new() { FirstName = "Alexander", LastName = "Wilson", Street = "16 River Road", City = "Columbus, OH", Phone = "+1 (614) 555-0114", Email = "contact14@example.test" },
            new() { FirstName = "Amelia", LastName = "Anderson", Street = "805 Hillcrest Lane", City = "Indianapolis, IN", Phone = "+1 (317) 555-0115", Email = "contact15@example.test" },
            new() { FirstName = "Daniel", LastName = "Thomas", Street = "63 Magnolia Street", City = "Charlotte, NC", Phone = "+1 (704) 555-0116", Email = "contact16@example.test" },
            new() { FirstName = "Harper", LastName = "Taylor", Street = "145 Bay Street", City = "San Francisco, CA", Phone = "+1 (415) 555-0117", Email = "contact17@example.test" },
            new() { FirstName = "Matthew", LastName = "Moore", Street = "904 Cherry Lane", City = "Seattle, WA", Phone = "+1 (206) 555-0118", Email = "contact18@example.test" },
            new() { FirstName = "Evelyn", LastName = "Jackson", Street = "38 Rosewood Avenue", City = "Denver, CO", Phone = "+1 (303) 555-0119", Email = "contact19@example.test" },
            new() { FirstName = "Joseph", LastName = "Martin", Street = "755 Grand Avenue", City = "Washington, DC", Phone = "+1 (202) 555-0120", Email = "contact20@example.test" },
            new() { FirstName = "Abigail", LastName = "Lee", Street = "502 Meadow Road", City = "Nashville, TN", Phone = "+1 (615) 555-0121", Email = "contact21@example.test" },
            new() { FirstName = "David", LastName = "Perez", Street = "24 Ocean Drive", City = "Boston, MA", Phone = "+1 (617) 555-0122", Email = "contact22@example.test" },
            new() { FirstName = "Ella", LastName = "Thompson", Street = "351 Cypress Street", City = "Portland, OR", Phone = "+1 (503) 555-0123", Email = "contact23@example.test" },
            new() { FirstName = "Samuel", LastName = "White", Street = "89 Garden Lane", City = "Las Vegas, NV", Phone = "+1 (702) 555-0124", Email = "contact24@example.test" },
            new() { FirstName = "Scarlett", LastName = "Harris", Street = "612 Summit Road", City = "Detroit, MI", Phone = "+1 (313) 555-0125", Email = "contact25@example.test" },
            new() { FirstName = "Jackson", LastName = "Sanchez", Street = "20 Ridge Avenue", City = "Memphis, TN", Phone = "+1 (901) 555-0126", Email = "contact26@example.test" },
            new() { FirstName = "Grace", LastName = "Clark", Street = "411 Walnut Street", City = "Louisville, KY", Phone = "+1 (502) 555-0127", Email = "contact27@example.test" },
            new() { FirstName = "Sebastian", LastName = "Ramirez", Street = "706 Walnut Avenue", City = "Baltimore, MD", Phone = "+1 (410) 555-0128", Email = "contact28@example.test" },
            new() { FirstName = "Chloe", LastName = "Lewis", Street = "93 Sycamore Drive", City = "Milwaukee, WI", Phone = "+1 (414) 555-0129", Email = "contact29@example.test" },
            new() { FirstName = "Logan", LastName = "Robinson", Street = "255 Aspen Lane", City = "Albuquerque, NM", Phone = "+1 (505) 555-0130", Email = "contact30@example.test" },
        };
        await db.Contacts.AddRangeAsync(contacts);
        await db.SaveChangesAsync();
    }
}
