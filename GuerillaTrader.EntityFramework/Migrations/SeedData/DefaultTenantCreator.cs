using System.Linq;
using GuerillaTrader.EntityFramework;
using GuerillaTrader.MultiTenancy;

namespace GuerillaTrader.Migrations.SeedData
{
    public class DefaultTenantCreator
    {
        private readonly GuerillaTraderDbContext _context;

        public DefaultTenantCreator(GuerillaTraderDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateUserAndRoles();
        }

        private void CreateUserAndRoles()
        {
            //Default tenant

            var defaultTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == Tenant.DefaultTenantName);
            if (defaultTenant == null)
            {
                _context.Tenants.Add(new Tenant {TenancyName = Tenant.DefaultTenantName, Name = Tenant.DefaultTenantName});
                _context.SaveChanges();
            }
        }
    }
}
