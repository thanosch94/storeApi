using Microsoft.EntityFrameworkCore;
using StoreApi.Data;

namespace DataNex.Data
{
    public class MsSqlDbContext:ApplicationDbContext
    {
        public MsSqlDbContext(DbContextOptions<ApplicationDbContext> options):base(options) 
        {
            
        }
    }
}
