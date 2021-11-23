using System;
using System.Linq;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using ExpenseManagement.Models;
using ExpenseManagement.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ExpenseManagement.Data
{
    public class ExpenseContext : IdentityDbContext<AppIdentityUser, AppIdentityRole, string>
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ExpenseContext(DbContextOptions<ExpenseContext> options, ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            this.loggerFactory = loggerFactory;
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var temoraryAuditEntities = await AuditNonTemporaryProperties();
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await AuditTemporaryProperties(temoraryAuditEntities);
            return result;
        }

        private async Task<IEnumerable<Tuple<EntityEntry, Audit>>> AuditNonTemporaryProperties()
        {
            ChangeTracker.DetectChanges();
            var entitiesToTrack = ChangeTracker.Entries().Where(e => !(e.Entity is Audit) && e.State != EntityState.Detached && e.State != EntityState.Unchanged);

            if (ChangeTracker.QueryTrackingBehavior != QueryTrackingBehavior.NoTracking)
            {
                await Audits.AddRangeAsync(
                    entitiesToTrack.Where(e => !e.Properties.Any(p => p.IsTemporary)).Select(e => new Audit()
                    {
                        TableName = e.Metadata.Relational().TableName,
                        Action = Enum.GetName(typeof(EntityState), e.State),
                        DateTime = DateTime.Now.ToUniversalTime(),
                        Username = this.httpContextAccessor?.HttpContext?.User?.Identity?.Name,
                        KeyValues = JsonConvert.SerializeObject(e.Properties.Where(p => p.Metadata.IsPrimaryKey()).ToDictionary(p => p.Metadata.Name, p => p.CurrentValue).NullIfEmpty()),
                        NewValues = JsonConvert.SerializeObject(e.Properties.Where(p => e.State == EntityState.Added || e.State == EntityState.Modified).ToDictionary(p => p.Metadata.Name, p => p.CurrentValue).NullIfEmpty()),
                        OldValues = JsonConvert.SerializeObject(e.Properties.Where(p => e.State == EntityState.Deleted || e.State == EntityState.Modified).ToDictionary(p => p.Metadata.Name, p => p.OriginalValue).NullIfEmpty())
                    }).ToList()
                );
            }
            return entitiesToTrack.Where(e => e.Properties.Any(p => p.IsTemporary))
                     .Select(e => new Tuple<EntityEntry, Audit>(
                         e,
                     new Audit()
                     {
                         TableName = e.Metadata.Relational().TableName,
                         Action = Enum.GetName(typeof(EntityState), e.State),
                         DateTime = DateTime.Now.ToUniversalTime(),
                         Username = this.httpContextAccessor?.HttpContext?.User?.Identity?.Name,
                         NewValues = JsonConvert.SerializeObject(e.Properties.Where(p => !p.Metadata.IsPrimaryKey()).ToDictionary(p => p.Metadata.Name, p => p.CurrentValue).NullIfEmpty())
                     }
                     )).ToList();
        }

        async Task AuditTemporaryProperties(IEnumerable<Tuple<EntityEntry, Audit>> temporatyEntities)
        {
            if (temporatyEntities != null && temporatyEntities.Any())
            {
                await Audits.AddRangeAsync(
                temporatyEntities.ForEach(t => t.Item2.KeyValues = JsonConvert.SerializeObject(t.Item1.Properties.Where(p => p.Metadata.IsPrimaryKey()).ToDictionary(p => p.Metadata.Name, p => p.CurrentValue).NullIfEmpty()))
                    .Select(t => t.Item2)
                );
                await SaveChangesAsync();
            }
            await Task.CompletedTask;
        }

        public DbSet<Audit> Audits { get; set; }

        public DbSet<AccountTypes> AccountTypes { get; set; }

        public DbSet<BankVaults> BankVaults { get; set; }

        public DbSet<Expenses> Expenses { get; set; }

        public DbSet<ExpenseDocuments> ExpenseDocuments { get; set; }

        public DbSet<Incomes> Incomes { get; set; }

        public DbSet<IncomeDocuments> IncomeDocuments { get; set; }

        public DbSet<Sectors> Sectors { get; set; }

        public DbSet<BankBranches> BankBranches { get; set; }

        public DbSet<ToDoLists> ToDoLists { get; set; }

        public DbSet<CarBrands> CarBrands { get; set; }

        public DbSet<CarModels> CarModels { get; set; }

        public DbSet<VehiclePurchases> VehiclePurchases { get; set; }

        public DbSet<Suppliers> Suppliers { get; set; }

        public DbSet<Salesmans> Salesmans { get; set; }

        public DbSet<NewVehicleSales> NewVehicleSales { get; set; }

        public DbSet<UsedVehicleSales> UsedVehicleSales { get; set; }

        public DbSet<DepositAccounts> DepositAccounts { get; set; }

        public DbSet<EnergyDaily> EnergyDailies { get; set; }

        public DbSet<EnergyMonthlies> EnergyMonthlies { get; set; }

        public DbSet<EnergyLuytobs> EnergyLuytobs { get; set; }

        public DbSet<EnergyLuytobFiles> EnergyLuytobFiles { get; set; }

        public DbSet<PointOfSale> PointOfSale { get; set; }

        public DbSet<FuelTypes> FuelTypes { get; set; }

        public DbSet<FuelSales> FuelSales { get; set; }

        public DbSet<RaiseDiscountTracking> RaiseDiscountTracking { get; set; }

        public DbSet<RegistrationFees> RegistrationFees { get; set; }

        public DbSet<Bonuses> Bonuses { get; set; }

        public DbSet<BonusDocuments> BonusDocuments { get; set; }

        public DbSet<UsedVehiclePurchases> UsedVehiclePurchases { get; set; }
    }
}
