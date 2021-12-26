using ExpenseManagement.Enums;
using ExpenseManagement.Models;
using ExpenseManagement.Models.ViewModels;
using System.Collections.Generic;
using static ExpenseManagement.Models.ViewModels.ReportViewModel;

namespace ExpenseManagement.Helpers
{
    public static class GetAllEnumNamesHelper
    {
        public static List<VehiclePurchases> GetEnumName(List<VehiclePurchases> vehiclePurchases)
        {
            foreach (var vehiclePurchase in vehiclePurchases)
            {
                if (vehiclePurchase.SaleAmountCurrency == 0)
                {
                    vehiclePurchase.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                }
                if (vehiclePurchase.SaleAmountCurrency == 1)
                {
                    vehiclePurchase.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                }
                if (vehiclePurchase.SaleAmountCurrency == 2)
                {
                    vehiclePurchase.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                }
                if (vehiclePurchase.SaleAmountCurrency == 3)
                {
                    vehiclePurchase.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                }

                if (vehiclePurchase.PurchaseAmountCurrency == 0)
                {
                    vehiclePurchase.PurchaseAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                }
                if (vehiclePurchase.PurchaseAmountCurrency == 1)
                {
                    vehiclePurchase.PurchaseAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                }
                if (vehiclePurchase.PurchaseAmountCurrency == 2)
                {
                    vehiclePurchase.PurchaseAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                }
                if (vehiclePurchase.PurchaseAmountCurrency == 3)
                {
                    vehiclePurchase.PurchaseAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                }
            }
            return vehiclePurchases;
        }

        public static VehiclePurchases GetEnumName(VehiclePurchases vehiclePurchases)
        {
            if (vehiclePurchases.SaleAmountCurrency == 0)
            {
                vehiclePurchases.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
            }
            if (vehiclePurchases.SaleAmountCurrency == 1)
            {
                vehiclePurchases.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
            }
            if (vehiclePurchases.SaleAmountCurrency == 2)
            {
                vehiclePurchases.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
            }
            if (vehiclePurchases.SaleAmountCurrency == 3)
            {
                vehiclePurchases.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
            }

            if (vehiclePurchases.PurchaseAmountCurrency == 0)
            {
                vehiclePurchases.PurchaseAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
            }
            if (vehiclePurchases.PurchaseAmountCurrency == 1)
            {
                vehiclePurchases.PurchaseAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
            }
            if (vehiclePurchases.PurchaseAmountCurrency == 2)
            {
                vehiclePurchases.PurchaseAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
            }
            if (vehiclePurchases.PurchaseAmountCurrency == 3)
            {
                vehiclePurchases.PurchaseAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
            }
            return vehiclePurchases;
        }

        public static List<DepositAccounts> GetEnumName(List<DepositAccounts> depositAccounts)
        {
            foreach (var depositAccount in depositAccounts)
            {
                if (depositAccount.AmountCurrency == 0)
                {
                    depositAccount.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                }
                if (depositAccount.AmountCurrency == 1)
                {
                    depositAccount.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                }
                if (depositAccount.AmountCurrency == 2)
                {
                    depositAccount.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                }
                if (depositAccount.AmountCurrency == 3)
                {
                    depositAccount.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                }

                if (depositAccount.ProfitCurrency == 0)
                {
                    depositAccount.ProfitCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                }
                if (depositAccount.ProfitCurrency == 1)
                {
                    depositAccount.ProfitCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                }
                if (depositAccount.ProfitCurrency == 2)
                {
                    depositAccount.ProfitCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                }
                if (depositAccount.ProfitCurrency == 3)
                {
                    depositAccount.ProfitCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                }
            }
            return depositAccounts;
        }

        public static DepositAccounts GetEnumName(DepositAccounts depositAccounts)
        {
            if (depositAccounts.AmountCurrency == 0)
            {
                depositAccounts.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
            }
            if (depositAccounts.AmountCurrency == 1)
            {
                depositAccounts.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
            }
            if (depositAccounts.AmountCurrency == 2)
            {
                depositAccounts.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
            }
            if (depositAccounts.AmountCurrency == 3)
            {
                depositAccounts.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
            }

            if (depositAccounts.ProfitCurrency == 0)
            {
                depositAccounts.ProfitCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
            }
            if (depositAccounts.ProfitCurrency == 1)
            {
                depositAccounts.ProfitCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
            }
            if (depositAccounts.ProfitCurrency == 2)
            {
                depositAccounts.ProfitCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
            }
            if (depositAccounts.ProfitCurrency == 3)
            {
                depositAccounts.ProfitCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
            }
            return depositAccounts;
        }

        public static List<NewVehicleSales> GetEnumName(List<NewVehicleSales> newVehicleSales)
        {
            foreach (var newVehicleSale in newVehicleSales)
            {
                if (newVehicleSale.SaleAmountCurrency == 0)
                {
                    newVehicleSale.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                }
                if (newVehicleSale.SaleAmountCurrency == 1)
                {
                    newVehicleSale.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                }
                if (newVehicleSale.SaleAmountCurrency == 2)
                {
                    newVehicleSale.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                }
                if (newVehicleSale.SaleAmountCurrency == 3)
                {
                    newVehicleSale.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                }

                if (newVehicleSale.VehicleCostCurrency == 0)
                {
                    newVehicleSale.VehicleCostCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                }
                if (newVehicleSale.VehicleCostCurrency == 1)
                {
                    newVehicleSale.VehicleCostCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                }
                if (newVehicleSale.VehicleCostCurrency == 2)
                {
                    newVehicleSale.VehicleCostCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                }
                if (newVehicleSale.VehicleCostCurrency == 3)
                {
                    newVehicleSale.VehicleCostCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                }
            }
            return newVehicleSales;
        }

        public static NewVehicleSales GetEnumName(NewVehicleSales newVehicleSales)
        {
            if (newVehicleSales.SaleAmountCurrency == 0)
            {
                newVehicleSales.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
            }
            if (newVehicleSales.SaleAmountCurrency == 1)
            {
                newVehicleSales.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
            }
            if (newVehicleSales.SaleAmountCurrency == 2)
            {
                newVehicleSales.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
            }
            if (newVehicleSales.SaleAmountCurrency == 3)
            {
                newVehicleSales.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
            }

            if (newVehicleSales.VehicleCostCurrency == 0)
            {
                newVehicleSales.VehicleCostCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
            }
            if (newVehicleSales.VehicleCostCurrency == 1)
            {
                newVehicleSales.VehicleCostCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
            }
            if (newVehicleSales.VehicleCostCurrency == 2)
            {
                newVehicleSales.VehicleCostCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
            }
            if (newVehicleSales.VehicleCostCurrency == 3)
            {
                newVehicleSales.VehicleCostCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
            }

            return newVehicleSales;
        }

        public static List<UsedVehicleSales> GetEnumName(List<UsedVehicleSales> usedVehicleSales)
        {
            foreach (var usedVehicleSale in usedVehicleSales)
            {
                if (usedVehicleSale.SaleAmountCurrency == 0)
                {
                    usedVehicleSale.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                }
                if (usedVehicleSale.SaleAmountCurrency == 1)
                {
                    usedVehicleSale.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                }
                if (usedVehicleSale.SaleAmountCurrency == 2)
                {
                    usedVehicleSale.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                }
                if (usedVehicleSale.SaleAmountCurrency == 3)
                {
                    usedVehicleSale.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                }

                if (usedVehicleSale.PurchaseAmountCurrency == 0)
                {
                    usedVehicleSale.PurchaseAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                }
                if (usedVehicleSale.PurchaseAmountCurrency == 1)
                {
                    usedVehicleSale.PurchaseAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                }
                if (usedVehicleSale.PurchaseAmountCurrency == 2)
                {
                    usedVehicleSale.PurchaseAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                }
                if (usedVehicleSale.PurchaseAmountCurrency == 3)
                {
                    usedVehicleSale.PurchaseAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                }
            }
            return usedVehicleSales;
        }

        public static UsedVehicleSales GetEnumName(UsedVehicleSales usedVehicleSales)
        {
            if (usedVehicleSales.SaleAmountCurrency == 0)
            {
                usedVehicleSales.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
            }
            if (usedVehicleSales.SaleAmountCurrency == 1)
            {
                usedVehicleSales.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
            }
            if (usedVehicleSales.SaleAmountCurrency == 2)
            {
                usedVehicleSales.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
            }
            if (usedVehicleSales.SaleAmountCurrency == 3)
            {
                usedVehicleSales.SaleAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
            }

            if (usedVehicleSales.PurchaseAmountCurrency == 0)
            {
                usedVehicleSales.PurchaseAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
            }
            if (usedVehicleSales.PurchaseAmountCurrency == 1)
            {
                usedVehicleSales.PurchaseAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
            }
            if (usedVehicleSales.PurchaseAmountCurrency == 2)
            {
                usedVehicleSales.PurchaseAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
            }
            if (usedVehicleSales.PurchaseAmountCurrency == 3)
            {
                usedVehicleSales.PurchaseAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
            }

            return usedVehicleSales;
        }

        public static List<ToDoLists> GetEnumName(List<ToDoLists> lists)
        {
            foreach (var list in lists)
            {
                if (list.AmountCurrency == 0)
                {
                    list.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(ToDoLists.AmountCurrencyEnum.TRY);
                }
                if (list.AmountCurrency == 1)
                {
                    list.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(ToDoLists.AmountCurrencyEnum.USD);
                }
                if (list.AmountCurrency == 2)
                {
                    list.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(ToDoLists.AmountCurrencyEnum.EUR);
                }
                if (list.AmountCurrency == 3)
                {
                    list.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(ToDoLists.AmountCurrencyEnum.GBP);
                }
            }
            return lists;
        }

        public static ToDoLists GetEnumName(ToDoLists lists)
        {
            if (lists.AmountCurrency == 0)
            {
                lists.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(ToDoLists.AmountCurrencyEnum.TRY);
            }
            if (lists.AmountCurrency == 1)
            {
                lists.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(ToDoLists.AmountCurrencyEnum.USD);
            }
            if (lists.AmountCurrency == 2)
            {
                lists.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(ToDoLists.AmountCurrencyEnum.EUR);
            }
            if (lists.AmountCurrency == 3)
            {
                lists.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(ToDoLists.AmountCurrencyEnum.GBP);
            }
            return lists;
        }

        public static List<BankVaults> GetEnumName(List<BankVaults> banks)
        {
            foreach (var bank in banks)
            {
                if (bank.AmountCurrency == 0)
                {
                    bank.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(BankVaults.AmountCurrencyEnum.TRY);
                }
                if (bank.AmountCurrency == 1)
                {
                    bank.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(BankVaults.AmountCurrencyEnum.USD);
                }
                if (bank.AmountCurrency == 2)
                {
                    bank.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(BankVaults.AmountCurrencyEnum.EUR);
                }
                if (bank.AmountCurrency == 3)
                {
                    bank.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(BankVaults.AmountCurrencyEnum.GBP);
                }
            }
            return banks;
        }

        public static BankVaults GetEnumName(BankVaults banks)
        {
            if (banks.AmountCurrency == 0)
            {
                banks.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(BankVaults.AmountCurrencyEnum.TRY);
            }
            if (banks.AmountCurrency == 1)
            {
                banks.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(BankVaults.AmountCurrencyEnum.USD);
            }
            if (banks.AmountCurrency == 2)
            {
                banks.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(BankVaults.AmountCurrencyEnum.EUR);
            }
            if (banks.AmountCurrency == 3)
            {
                banks.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(BankVaults.AmountCurrencyEnum.GBP);
            }
            return banks;
        }

        public static List<Expenses> GetEnumName(List<Expenses> expenses)
        {
            foreach (var expense in expenses)
            {
                switch (expense.AmountCurrency)
                {
                    case 0:
                        expense.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                        break;
                    case 1:
                        expense.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                        break;
                    case 2:
                        expense.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                        break;
                    case 3:
                        expense.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                        break;
                    default:
                        break;
                }

                switch (expense.DiscountCurrency)
                {
                    case 0:
                        expense.DiscountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                        break;
                    case 1:
                        expense.DiscountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                        break;
                    case 2:
                        expense.DiscountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                        break;
                    case 3:
                        expense.DiscountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                        break;
                    default:
                        break;
                }

                switch (expense.TAXCurrency)
                {
                    case 0:
                        expense.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                        break;
                    case 1:
                        expense.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                        break;
                    case 2:
                        expense.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                        break;
                    case 3:
                        expense.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                        break;
                    default:
                        break;
                }

                switch (expense.OtherTAXCurrency)
                {
                    case 0:
                        expense.OtherTAXCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                        break;
                    case 1:
                        expense.OtherTAXCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                        break;
                    case 2:
                        expense.OtherTAXCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                        break;
                    case 3:
                        expense.OtherTAXCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                        break;
                    default:
                        break;
                }

                switch (expense.ExpenseType)
                {
                    case 0:
                        expense.ExpenseTypeName = EnumExtensionsHelper.GetDisplayName(Expenses.ExpenseTypeEnum.PURCHASE);
                        break;
                    case 1:
                        expense.ExpenseTypeName = EnumExtensionsHelper.GetDisplayName(Expenses.ExpenseTypeEnum.EXPENSE);
                        break;
                    case 2:
                        expense.ExpenseTypeName = EnumExtensionsHelper.GetDisplayName(Expenses.ExpenseTypeEnum.SALARY);
                        break;
                    default:
                        break;
                }

                switch (expense.SalaryAmountCurrency)
                {
                    case 0:
                        expense.SalaryAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                        break;
                    case 1:
                        expense.SalaryAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                        break;
                    case 2:
                        expense.SalaryAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                        break;
                    case 3:
                        expense.SalaryAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                        break;
                    default:
                        break;
                }

                expense.MonthName = GetMonthNameHelper.GetMonth(expense.Month);
            }
            return expenses;
        }

        public static Expenses GetEnumName(Expenses expenses)
        {
            switch (expenses.AmountCurrency)
            {
                case 0:
                    expenses.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                    break;
                case 1:
                    expenses.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                    break;
                case 2:
                    expenses.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                    break;
                case 3:
                    expenses.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                    break;
                default:
                    break;
            }

            switch (expenses.DiscountCurrency)
            {
                case 0:
                    expenses.DiscountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                    break;
                case 1:
                    expenses.DiscountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                    break;
                case 2:
                    expenses.DiscountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                    break;
                case 3:
                    expenses.DiscountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                    break;
                default:
                    break;
            }

            switch (expenses.TAXCurrency)
            {
                case 0:
                    expenses.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                    break;
                case 1:
                    expenses.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                    break;
                case 2:
                    expenses.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                    break;
                case 3:
                    expenses.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                    break;
                default:
                    break;
            }

            switch (expenses.OtherTAXCurrency)
            {
                case 0:
                    expenses.OtherTAXCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                    break;
                case 1:
                    expenses.OtherTAXCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                    break;
                case 2:
                    expenses.OtherTAXCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                    break;
                case 3:
                    expenses.OtherTAXCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                    break;
                default:
                    break;
            }

            switch (expenses.ExpenseType)
            {
                case 0:
                    expenses.ExpenseTypeName = EnumExtensionsHelper.GetDisplayName(Expenses.ExpenseTypeEnum.PURCHASE);
                    break;
                case 1:
                    expenses.ExpenseTypeName = EnumExtensionsHelper.GetDisplayName(Expenses.ExpenseTypeEnum.EXPENSE);
                    break;
                case 2:
                    expenses.ExpenseTypeName = EnumExtensionsHelper.GetDisplayName(Expenses.ExpenseTypeEnum.SALARY);
                    break;
                default:
                    break;
            }

            switch (expenses.SalaryAmountCurrency)
            {
                case 0:
                    expenses.SalaryAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                    break;
                case 1:
                    expenses.SalaryAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                    break;
                case 2:
                    expenses.SalaryAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                    break;
                case 3:
                    expenses.SalaryAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                    break;
                default:
                    break;
            }

            expenses.MonthName = GetMonthNameHelper.GetMonth(expenses.Month);
            return expenses;
        }

        public static List<Incomes> GetEnumName(List<Incomes> incomes)
        {
            foreach (var income in incomes)
            {
                switch (income.AmountCurrency)
                {
                    case 0:
                        income.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Incomes.AmountCurrencyEnum.TRY);
                        break;
                    case 1:
                        income.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Incomes.AmountCurrencyEnum.USD);
                        break;
                    case 2:
                        income.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Incomes.AmountCurrencyEnum.EUR);
                        break;
                    case 3:
                        income.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Incomes.AmountCurrencyEnum.GBP);
                        break;
                    default:
                        break;
                }

                switch (income.TAXCurrency)
                {
                    case 0:
                        income.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(Incomes.TAXCurrencyEnum.TRY);
                        break;
                    case 1:
                        income.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(Incomes.TAXCurrencyEnum.USD);
                        break;
                    case 2:
                        income.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(Incomes.TAXCurrencyEnum.EUR);
                        break;
                    case 3:
                        income.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(Incomes.TAXCurrencyEnum.GBP);
                        break;
                    default:
                        break;
                }
            }
            return incomes;
        }

        public static Incomes GetEnumName(Incomes incomes)
        {
            switch (incomes.AmountCurrency)
            {
                case 0:
                    incomes.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Incomes.AmountCurrencyEnum.TRY);
                    break;
                case 1:
                    incomes.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Incomes.AmountCurrencyEnum.USD);
                    break;
                case 2:
                    incomes.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Incomes.AmountCurrencyEnum.EUR);
                    break;
                case 3:
                    incomes.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Incomes.AmountCurrencyEnum.GBP);
                    break;
                default:
                    break;
            }

            switch (incomes.TAXCurrency)
            {
                case 0:
                    incomes.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(Incomes.TAXCurrencyEnum.TRY);
                    break;
                case 1:
                    incomes.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(Incomes.TAXCurrencyEnum.USD);
                    break;
                case 2:
                    incomes.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(Incomes.TAXCurrencyEnum.EUR);
                    break;
                case 3:
                    incomes.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(Incomes.TAXCurrencyEnum.GBP);
                    break;
                default:
                    break;
            }
            return incomes;
        }

        public static List<GeneralResponse> GetEnumName(List<GeneralResponse> generalResponses)
        {
            foreach (var generalResponse in generalResponses)
            {
                generalResponse.MonthName = GetMonthNameHelper.GetMonth((byte)generalResponse.Month);

                switch (generalResponse.TotalAmountCurrency)
                {
                    case 0:
                        generalResponse.TotalAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.SalaryAmountCurrencyEnum.TRY);
                        break;
                    case 1:
                        generalResponse.TotalAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.SalaryAmountCurrencyEnum.USD);
                        break;
                    case 2:
                        generalResponse.TotalAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.SalaryAmountCurrencyEnum.EUR);
                        break;
                    case 3:
                        generalResponse.TotalAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.SalaryAmountCurrencyEnum.GBP);
                        break;
                    default:
                        break;
                }
            }
            return generalResponses;
        }

        public static List<ToDoListResponse> GetEnumName(List<ToDoListResponse> toDoListResponses)
        {
            foreach (var toDoListResponse in toDoListResponses)
            {
                switch (toDoListResponse.TotalAmountCurrency)
                {
                    case 0:
                        toDoListResponse.TotalAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.SalaryAmountCurrencyEnum.TRY);
                        break;
                    case 1:
                        toDoListResponse.TotalAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.SalaryAmountCurrencyEnum.USD);
                        break;
                    case 2:
                        toDoListResponse.TotalAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.SalaryAmountCurrencyEnum.EUR);
                        break;
                    case 3:
                        toDoListResponse.TotalAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.SalaryAmountCurrencyEnum.GBP);
                        break;
                    default:
                        break;
                }
            }
            return toDoListResponses;
        }

        public static List<EnergyDaily> GetEnumName(List<EnergyDaily> energyDailies)
        {
            foreach (var energyDaily in energyDailies)
            {
                energyDaily.MonthName = GetMonthNameHelper.GetMonth((byte)energyDaily.Date.Month);
            }
            return energyDailies;
        }

        public static List<EnergyMonthlies> GetEnumName(List<EnergyMonthlies> energyMonthlies)
        {
            foreach (var energyMonthly in energyMonthlies)
            {
                energyMonthly.MonthName = GetMonthNameHelper.GetMonth(energyMonthly.Month);
            }
            return energyMonthlies;
        }

        public static List<EnergyLuytobs> GetEnumName(List<EnergyLuytobs> energyLuytobs)
        {
            foreach (var energyLuytob in energyLuytobs)
            {
                energyLuytob.MonthName = GetMonthNameHelper.GetMonth(energyLuytob.Month);
            }
            return energyLuytobs;
        }

        public static List<ExpenseEndorsementProfitReport> GetEnumName(List<ExpenseEndorsementProfitReport> expenseReportViewModel)
        {
            foreach (var item in expenseReportViewModel)
            {
                if (item.TotalAmountCurrency == 0)
                {
                    item.TotalAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                }
                if (item.TotalAmountCurrency == 1)
                {
                    item.TotalAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                }
                if (item.TotalAmountCurrency == 2)
                {
                    item.TotalAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                }
                if (item.TotalAmountCurrency == 3)
                {
                    item.TotalAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                }

                if (item.TotalProfitCurrency == 0)
                {
                    item.TotalProfitCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                }
                if (item.TotalProfitCurrency == 1)
                {
                    item.TotalProfitCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                }
                if (item.TotalProfitCurrency == 2)
                {
                    item.TotalProfitCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                }
                if (item.TotalProfitCurrency == 3)
                {
                    item.TotalProfitCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                }
            }
            return expenseReportViewModel;
        }

        public static List<PointOfSale> GetEnumName(List<PointOfSale> pos)
        {
            foreach (var p in pos)
            {
                switch (p.AmountCurrency)
                {
                    case 0:
                        p.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                        break;
                    case 1:
                        p.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                        break;
                    case 2:
                        p.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                        break;
                    case 3:
                        p.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                        break;
                    default:
                        break;
                }
                p.MonthName = GetMonthNameHelper.GetMonth(p.Month);
            }
            return pos;
        }

        public static PointOfSale GetEnumName(PointOfSale pos)
        {
            switch (pos.AmountCurrency)
            {
                case 0:
                    pos.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                    break;
                case 1:
                    pos.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                    break;
                case 2:
                    pos.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                    break;
                case 3:
                    pos.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                    break;
                default:
                    break;
            }
            pos.MonthName = GetMonthNameHelper.GetMonth(pos.Month);
            return pos;
        }

        public static List<Bonuses> GetEnumName(List<Bonuses> bonuses)
        {
            foreach (var bonus in bonuses)
            {
                switch (bonus.BonusType)
                {
                    case 0:
                        bonus.BonusTypeName = EnumExtensionsHelper.GetDisplayName(Bonuses.BonusTypeEnum.SERVICE);
                        break;
                    case 1:
                        bonus.BonusTypeName = EnumExtensionsHelper.GetDisplayName(Bonuses.BonusTypeEnum.SALE);
                        break;
                    default:
                        break;
                }
                bonus.MonthName = GetMonthNameHelper.GetMonth(bonus.Month);
            }
            return bonuses;
        }

        public static Bonuses GetEnumName(Bonuses bonuses)
        {
            switch (bonuses.BonusType)
            {
                case 0:
                    bonuses.BonusTypeName = EnumExtensionsHelper.GetDisplayName(Bonuses.BonusTypeEnum.SERVICE);
                    break;
                case 1:
                    bonuses.BonusTypeName = EnumExtensionsHelper.GetDisplayName(Bonuses.BonusTypeEnum.SALE);
                    break;
                default:
                    break;
            }
            bonuses.MonthName = GetMonthNameHelper.GetMonth(bonuses.Month);
            return bonuses;
        }

        public static List<PetrolNetProfit> GetEnumName(List<PetrolNetProfit> profits)
        {
            foreach (var profit in profits)
            {
                profit.MonthName = GetMonthNameHelper.GetMonth(profit.Month);
            }
            return profits;
        }

        public static List<Tankers> GetEnumName(List<Tankers> tankers)
        {
            foreach (var tanker in tankers)
            {
                tanker.MonthName = GetMonthNameHelper.GetMonth(tanker.Month);
            }
            return tankers;
        }

        public static List<InterestIncomes> GetEnumName(List<InterestIncomes> interestIncomes)
        {
            foreach (var interestIncome in interestIncomes)
            {
                switch (interestIncome.InterestType)
                {
                    case 0:
                        interestIncome.InterestTypeName = EnumExtensionsHelper.GetDisplayName(InterestIncomes.InterestTypeEnum.OVERNIGHT_INTEREST);
                        break;
                    case 1:
                        interestIncome.InterestTypeName = EnumExtensionsHelper.GetDisplayName(InterestIncomes.InterestTypeEnum.LONG_TERM_INTEREST);
                        break;
                    default:
                        break;
                }
            }
            return interestIncomes;
        }

        public static InterestIncomes GetEnumName(InterestIncomes interestIncomes)
        {
            switch (interestIncomes.InterestType)
            {
                case 0:
                    interestIncomes.InterestTypeName = EnumExtensionsHelper.GetDisplayName(InterestIncomes.InterestTypeEnum.OVERNIGHT_INTEREST);
                    break;
                case 1:
                    interestIncomes.InterestTypeName = EnumExtensionsHelper.GetDisplayName(InterestIncomes.InterestTypeEnum.LONG_TERM_INTEREST);
                    break;
                default:
                    break;
            }
            return interestIncomes;
        }
    }
}
