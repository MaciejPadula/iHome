using iHome.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace iHome.Core.Repositories;
public class DevicesDataContext : DbContext
{
    public DbSet<Device> Devices => Set<Device>();

    public DevicesDataContext(DbContextOptions<DevicesDataContext> options)
        : base(options)
    {}
}
