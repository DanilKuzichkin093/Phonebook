using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BookingAppStore.Models
{
    public class BookContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Purchase> Purchase { get; set; }
    }

    public class BookDbInitualizer: DropCreateDatabaseIfModelChanges<BookContext>
    {

        protected override void Seed(BookContext db)
        {
            base.Seed(db);
        } 
    }
}