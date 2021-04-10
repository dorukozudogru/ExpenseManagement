﻿// <auto-generated />
using System;
using ExpenseManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ExpenseManagement.Migrations
{
    [DbContext(typeof(ExpenseContext))]
    partial class ExpenseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ExpenseManagement.Models.AccountTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("AccountTypes");
                });

            modelBuilder.Entity("ExpenseManagement.Models.AppIdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("ExpenseManagement.Models.AppIdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsAdmin");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ExpenseManagement.Models.Audit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Action");

                    b.Property<DateTime>("DateTime");

                    b.Property<string>("EntityName");

                    b.Property<string>("KeyValues");

                    b.Property<string>("NewValues");

                    b.Property<string>("OldValues");

                    b.Property<string>("TableName");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Audits");
                });

            modelBuilder.Entity("ExpenseManagement.Models.BankBranches", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("BankBranches");
                });

            modelBuilder.Entity("ExpenseManagement.Models.BankVaults", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountTypeId");

                    b.Property<double>("Amount");

                    b.Property<byte>("AmountCurrency");

                    b.Property<int>("BankBranchId");

                    b.HasKey("Id");

                    b.HasIndex("AccountTypeId");

                    b.HasIndex("BankBranchId");

                    b.ToTable("BankVaults");
                });

            modelBuilder.Entity("ExpenseManagement.Models.CarBrands", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("CarBrands");
                });

            modelBuilder.Entity("ExpenseManagement.Models.CarModels", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CarBrandId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CarBrandId");

                    b.ToTable("CarModels");
                });

            modelBuilder.Entity("ExpenseManagement.Models.DepositAccounts", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount");

                    b.Property<byte>("AmountCurrency");

                    b.Property<int>("BankBranchId");

                    b.Property<int>("DayOfDeposit");

                    b.Property<DateTime>("FinishDate");

                    b.Property<double>("Interest");

                    b.Property<double>("Profit");

                    b.Property<byte>("ProfitCurrency");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("BankBranchId");

                    b.ToTable("DepositAccounts");
                });

            modelBuilder.Entity("ExpenseManagement.Models.EnergyDaily", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<double>("Kw");

                    b.HasKey("Id");

                    b.ToTable("EnergyDailies");
                });

            modelBuilder.Entity("ExpenseManagement.Models.EnergyLuytobFiles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("Invoice");

                    b.Property<string>("InvoiceFormat");

                    b.Property<byte[]>("Luytob");

                    b.Property<string>("LuytobFormat");

                    b.HasKey("Id");

                    b.ToTable("EnergyLuytobFiles");
                });

            modelBuilder.Entity("ExpenseManagement.Models.EnergyLuytobs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EnergyLuytobFileId");

                    b.Property<byte?>("Month")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("EnergyLuytobFileId");

                    b.ToTable("EnergyLuytobs");
                });

            modelBuilder.Entity("ExpenseManagement.Models.EnergyMonthlies", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount");

                    b.Property<double>("ConsumedKw");

                    b.Property<double>("DistributionFee");

                    b.Property<byte?>("Month")
                        .IsRequired();

                    b.Property<double>("ProducedKw");

                    b.Property<double>("TAX");

                    b.HasKey("Id");

                    b.ToTable("EnergyMonthlies");
                });

            modelBuilder.Entity("ExpenseManagement.Models.ExpenseDocuments", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ExpenseId");

                    b.Property<byte[]>("InvoiceImage");

                    b.Property<string>("InvoiceImageFormat");

                    b.HasKey("Id");

                    b.ToTable("ExpenseDocuments");
                });

            modelBuilder.Entity("ExpenseManagement.Models.Expenses", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double?>("Amount");

                    b.Property<byte?>("AmountCurrency");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("Date");

                    b.Property<string>("Definition")
                        .IsRequired();

                    b.Property<byte>("ExpenseType");

                    b.Property<byte[]>("InvoiceImage");

                    b.Property<string>("InvoiceImageFormat");

                    b.Property<DateTime?>("LastPaymentDate");

                    b.Property<byte?>("Month");

                    b.Property<double?>("SalaryAmount");

                    b.Property<byte?>("SalaryAmountCurrency");

                    b.Property<int>("SectorId");

                    b.Property<int>("State");

                    b.Property<string>("SupplierDef");

                    b.Property<double?>("TAX");

                    b.Property<double?>("TAXCurrency");

                    b.HasKey("Id");

                    b.HasIndex("SectorId");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("ExpenseManagement.Models.IncomeDocuments", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IncomeId");

                    b.Property<byte[]>("InvoiceImage");

                    b.Property<string>("InvoiceImageFormat");

                    b.HasKey("Id");

                    b.ToTable("IncomeDocuments");
                });

            modelBuilder.Entity("ExpenseManagement.Models.Incomes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount");

                    b.Property<byte>("AmountCurrency");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Definition")
                        .IsRequired();

                    b.Property<byte[]>("InvoiceImage");

                    b.Property<string>("InvoiceImageFormat");

                    b.Property<int>("SectorId");

                    b.Property<int>("State");

                    b.Property<double>("TAX");

                    b.Property<double>("TAXCurrency");

                    b.HasKey("Id");

                    b.HasIndex("SectorId");

                    b.ToTable("Incomes");
                });

            modelBuilder.Entity("ExpenseManagement.Models.NewVehicleSales", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<DateTime>("PurchaseDate");

                    b.Property<double>("SaleAmount");

                    b.Property<byte>("SaleAmountCurrency");

                    b.Property<DateTime>("SaleDate");

                    b.Property<double>("SalesmanBonus");

                    b.Property<byte>("SalesmanBonusCurrency");

                    b.Property<int>("SalesmanId");

                    b.Property<double>("VehicleCost");

                    b.Property<byte>("VehicleCostCurrency");

                    b.Property<int>("VehiclePurchaseId");

                    b.Property<string>("WarrantyPlus");

                    b.HasKey("Id");

                    b.HasIndex("SalesmanId");

                    b.HasIndex("VehiclePurchaseId");

                    b.ToTable("NewVehicleSales");
                });

            modelBuilder.Entity("ExpenseManagement.Models.Salesmans", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Salesmans");
                });

            modelBuilder.Entity("ExpenseManagement.Models.Sectors", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Sectors");
                });

            modelBuilder.Entity("ExpenseManagement.Models.Suppliers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("ExpenseManagement.Models.ToDoLists", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount");

                    b.Property<byte>("AmountCurrency");

                    b.Property<DateTime>("CollectAt");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("Debtor")
                        .IsRequired();

                    b.Property<int>("SectorId");

                    b.Property<bool>("State");

                    b.HasKey("Id");

                    b.HasIndex("SectorId");

                    b.ToTable("ToDoLists");
                });

            modelBuilder.Entity("ExpenseManagement.Models.UsedVehicleSales", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("KM")
                        .IsRequired();

                    b.Property<string>("LicencePlate")
                        .IsRequired();

                    b.Property<double>("PurchaseAmount");

                    b.Property<byte>("PurchaseAmountCurrency");

                    b.Property<DateTime>("PurchaseDate");

                    b.Property<double>("PurchasedSalesmanBonus");

                    b.Property<byte>("PurchasedSalesmanBonusCurrency");

                    b.Property<int>("PurchasedSalesmanId");

                    b.Property<double>("SaleAmount");

                    b.Property<byte>("SaleAmountCurrency");

                    b.Property<DateTime>("SaleDate");

                    b.Property<double>("SoldSalesmanBonus");

                    b.Property<byte>("SoldSalesmanBonusCurrency");

                    b.Property<int?>("SoldSalesmanId");

                    b.Property<int>("VehiclePurchaseId");

                    b.HasKey("Id");

                    b.HasIndex("PurchasedSalesmanId");

                    b.HasIndex("SoldSalesmanId");

                    b.HasIndex("VehiclePurchaseId");

                    b.ToTable("UsedVehicleSales");
                });

            modelBuilder.Entity("ExpenseManagement.Models.VehiclePurchases", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CarModelId");

                    b.Property<string>("Chassis")
                        .IsRequired();

                    b.Property<double?>("IncludingRegistrationFee");

                    b.Property<bool>("IsNew");

                    b.Property<bool>("IsSold");

                    b.Property<double?>("KDV");

                    b.Property<double?>("OTV");

                    b.Property<int?>("OTVPercent");

                    b.Property<double>("PurchaseAmount");

                    b.Property<byte>("PurchaseAmountCurrency");

                    b.Property<DateTime>("PurchaseDate");

                    b.Property<int?>("RegistrationFee");

                    b.Property<double?>("SaleAmount");

                    b.Property<byte?>("SaleAmountCurrency")
                        .IsRequired();

                    b.Property<DateTime?>("SaleDate");

                    b.HasKey("Id");

                    b.HasIndex("CarModelId");

                    b.ToTable("VehiclePurchases");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ExpenseManagement.Models.BankVaults", b =>
                {
                    b.HasOne("ExpenseManagement.Models.AccountTypes", "AccountType")
                        .WithMany()
                        .HasForeignKey("AccountTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ExpenseManagement.Models.BankBranches", "BankBranch")
                        .WithMany()
                        .HasForeignKey("BankBranchId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ExpenseManagement.Models.CarModels", b =>
                {
                    b.HasOne("ExpenseManagement.Models.CarBrands", "CarBrand")
                        .WithMany()
                        .HasForeignKey("CarBrandId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ExpenseManagement.Models.DepositAccounts", b =>
                {
                    b.HasOne("ExpenseManagement.Models.BankBranches", "BankBranch")
                        .WithMany()
                        .HasForeignKey("BankBranchId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ExpenseManagement.Models.EnergyLuytobs", b =>
                {
                    b.HasOne("ExpenseManagement.Models.EnergyLuytobFiles", "EnergyLuytobFile")
                        .WithMany()
                        .HasForeignKey("EnergyLuytobFileId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ExpenseManagement.Models.Expenses", b =>
                {
                    b.HasOne("ExpenseManagement.Models.Sectors", "Sector")
                        .WithMany()
                        .HasForeignKey("SectorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ExpenseManagement.Models.Incomes", b =>
                {
                    b.HasOne("ExpenseManagement.Models.Sectors", "Sector")
                        .WithMany()
                        .HasForeignKey("SectorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ExpenseManagement.Models.NewVehicleSales", b =>
                {
                    b.HasOne("ExpenseManagement.Models.Salesmans", "Salesman")
                        .WithMany()
                        .HasForeignKey("SalesmanId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ExpenseManagement.Models.VehiclePurchases", "VehiclePurchase")
                        .WithMany()
                        .HasForeignKey("VehiclePurchaseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ExpenseManagement.Models.ToDoLists", b =>
                {
                    b.HasOne("ExpenseManagement.Models.Sectors", "Sector")
                        .WithMany()
                        .HasForeignKey("SectorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ExpenseManagement.Models.UsedVehicleSales", b =>
                {
                    b.HasOne("ExpenseManagement.Models.Salesmans", "PurchasedSalesman")
                        .WithMany()
                        .HasForeignKey("PurchasedSalesmanId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ExpenseManagement.Models.Salesmans", "SoldSalesman")
                        .WithMany()
                        .HasForeignKey("SoldSalesmanId");

                    b.HasOne("ExpenseManagement.Models.VehiclePurchases", "VehiclePurchase")
                        .WithMany()
                        .HasForeignKey("VehiclePurchaseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ExpenseManagement.Models.VehiclePurchases", b =>
                {
                    b.HasOne("ExpenseManagement.Models.CarModels", "CarModel")
                        .WithMany("VehiclePurchases")
                        .HasForeignKey("CarModelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("ExpenseManagement.Models.AppIdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ExpenseManagement.Models.AppIdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ExpenseManagement.Models.AppIdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("ExpenseManagement.Models.AppIdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ExpenseManagement.Models.AppIdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ExpenseManagement.Models.AppIdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
