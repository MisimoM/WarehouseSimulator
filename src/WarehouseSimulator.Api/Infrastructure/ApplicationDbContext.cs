using Microsoft.EntityFrameworkCore;

namespace WarehouseSimulator.Api.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
}
