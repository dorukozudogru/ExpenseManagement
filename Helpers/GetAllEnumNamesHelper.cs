﻿using ExpenseManagement.Enums;
using ExpenseManagement.Models;
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

                if (newVehicleSale.SalesmanBonusCurrency == 0)
                {
                    newVehicleSale.SalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                }
                if (newVehicleSale.SalesmanBonusCurrency == 1)
                {
                    newVehicleSale.SalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                }
                if (newVehicleSale.SalesmanBonusCurrency == 2)
                {
                    newVehicleSale.SalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                }
                if (newVehicleSale.SalesmanBonusCurrency == 3)
                {
                    newVehicleSale.SalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
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

            if (newVehicleSales.SalesmanBonusCurrency == 0)
            {
                newVehicleSales.SalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
            }
            if (newVehicleSales.SalesmanBonusCurrency == 1)
            {
                newVehicleSales.SalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
            }
            if (newVehicleSales.SalesmanBonusCurrency == 2)
            {
                newVehicleSales.SalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
            }
            if (newVehicleSales.SalesmanBonusCurrency == 3)
            {
                newVehicleSales.SalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
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

                if (usedVehicleSale.PurchasedSalesmanBonusCurrency == 0)
                {
                    usedVehicleSale.PurchasedSalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                }
                if (usedVehicleSale.PurchasedSalesmanBonusCurrency == 1)
                {
                    usedVehicleSale.PurchasedSalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                }
                if (usedVehicleSale.PurchasedSalesmanBonusCurrency == 2)
                {
                    usedVehicleSale.PurchasedSalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                }
                if (usedVehicleSale.PurchasedSalesmanBonusCurrency == 3)
                {
                    usedVehicleSale.PurchasedSalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
                }

                if (usedVehicleSale.SoldSalesmanBonusCurrency == 0)
                {
                    usedVehicleSale.SoldSalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
                }
                if (usedVehicleSale.SoldSalesmanBonusCurrency == 1)
                {
                    usedVehicleSale.SoldSalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
                }
                if (usedVehicleSale.SoldSalesmanBonusCurrency == 2)
                {
                    usedVehicleSale.SoldSalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
                }
                if (usedVehicleSale.SoldSalesmanBonusCurrency == 3)
                {
                    usedVehicleSale.SoldSalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
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

            if (usedVehicleSales.PurchasedSalesmanBonusCurrency == 0)
            {
                usedVehicleSales.PurchasedSalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
            }
            if (usedVehicleSales.PurchasedSalesmanBonusCurrency == 1)
            {
                usedVehicleSales.PurchasedSalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
            }
            if (usedVehicleSales.PurchasedSalesmanBonusCurrency == 2)
            {
                usedVehicleSales.PurchasedSalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
            }
            if (usedVehicleSales.PurchasedSalesmanBonusCurrency == 3)
            {
                usedVehicleSales.PurchasedSalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
            }

            if (usedVehicleSales.SoldSalesmanBonusCurrency == 0)
            {
                usedVehicleSales.SoldSalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.TRY);
            }
            if (usedVehicleSales.SoldSalesmanBonusCurrency == 1)
            {
                usedVehicleSales.SoldSalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.USD);
            }
            if (usedVehicleSales.SoldSalesmanBonusCurrency == 2)
            {
                usedVehicleSales.SoldSalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.EUR);
            }
            if (usedVehicleSales.SoldSalesmanBonusCurrency == 3)
            {
                usedVehicleSales.SoldSalesmanBonusCurrencyName = EnumExtensionsHelper.GetDisplayName(CurrencyEnum.GBP);
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
                        expense.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.AmountCurrencyEnum.TRY);
                        break;
                    case 1:
                        expense.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.AmountCurrencyEnum.USD);
                        break;
                    case 2:
                        expense.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.AmountCurrencyEnum.EUR);
                        break;
                    case 3:
                        expense.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.AmountCurrencyEnum.GBP);
                        break;
                    default:
                        break;
                }

                switch (expense.TAXCurrency)
                {
                    case 0:
                        expense.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.TAXCurrencyEnum.TRY);
                        break;
                    case 1:
                        expense.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.TAXCurrencyEnum.USD);
                        break;
                    case 2:
                        expense.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.TAXCurrencyEnum.EUR);
                        break;
                    case 3:
                        expense.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.TAXCurrencyEnum.GBP);
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
                        expense.SalaryAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.SalaryAmountCurrencyEnum.TRY);
                        break;
                    case 1:
                        expense.SalaryAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.SalaryAmountCurrencyEnum.USD);
                        break;
                    case 2:
                        expense.SalaryAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.SalaryAmountCurrencyEnum.EUR);
                        break;
                    case 3:
                        expense.SalaryAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.SalaryAmountCurrencyEnum.GBP);
                        break;
                    default:
                        break;
                }

                switch (expense.Month)
                {
                    case 1:
                        expense.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JANUARY);
                        break;
                    case 2:
                        expense.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.FEBRUARY);
                        break;
                    case 3:
                        expense.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.MARCH);
                        break;
                    case 4:
                        expense.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.APRIL);
                        break;
                    case 5:
                        expense.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.MAY);
                        break;
                    case 6:
                        expense.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JUNE);
                        break;
                    case 7:
                        expense.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JULY);
                        break;
                    case 8:
                        expense.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.AUGUST);
                        break;
                    case 9:
                        expense.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.SEPTEMBER);
                        break;
                    case 10:
                        expense.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.OCTOBER);
                        break;
                    case 11:
                        expense.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.NOVEMBER);
                        break;
                    case 12:
                        expense.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.DECEMBER);
                        break;
                    default:
                        break;
                }
            }
            return expenses;
        }

        public static Expenses GetEnumName(Expenses expenses)
        {
            switch (expenses.AmountCurrency)
            {
                case 0:
                    expenses.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.AmountCurrencyEnum.TRY);
                    break;
                case 1:
                    expenses.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.AmountCurrencyEnum.USD);
                    break;
                case 2:
                    expenses.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.AmountCurrencyEnum.EUR);
                    break;
                case 3:
                    expenses.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.AmountCurrencyEnum.GBP);
                    break;
                default:
                    break;
            }

            switch (expenses.TAXCurrency)
            {
                case 0:
                    expenses.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.TAXCurrencyEnum.TRY);
                    break;
                case 1:
                    expenses.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.TAXCurrencyEnum.USD);
                    break;
                case 2:
                    expenses.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.TAXCurrencyEnum.EUR);
                    break;
                case 3:
                    expenses.TAXCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.TAXCurrencyEnum.GBP);
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
                    expenses.SalaryAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.SalaryAmountCurrencyEnum.TRY);
                    break;
                case 1:
                    expenses.SalaryAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.SalaryAmountCurrencyEnum.USD);
                    break;
                case 2:
                    expenses.SalaryAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.SalaryAmountCurrencyEnum.EUR);
                    break;
                case 3:
                    expenses.SalaryAmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Expenses.SalaryAmountCurrencyEnum.GBP);
                    break;
                default:
                    break;
            }

            switch (expenses.Month)
            {
                case 1:
                    expenses.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JANUARY);
                    break;
                case 2:
                    expenses.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.FEBRUARY);
                    break;
                case 3:
                    expenses.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.MARCH);
                    break;
                case 4:
                    expenses.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.APRIL);
                    break;
                case 5:
                    expenses.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.MAY);
                    break;
                case 6:
                    expenses.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JUNE);
                    break;
                case 7:
                    expenses.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JULY);
                    break;
                case 8:
                    expenses.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.AUGUST);
                    break;
                case 9:
                    expenses.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.SEPTEMBER);
                    break;
                case 10:
                    expenses.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.OCTOBER);
                    break;
                case 11:
                    expenses.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.NOVEMBER);
                    break;
                case 12:
                    expenses.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.DECEMBER);
                    break;
                default:
                    break;
            }
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

                switch (income.Month)
                {
                    case 1:
                        income.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JANUARY);
                        break;
                    case 2:
                        income.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.FEBRUARY);
                        break;
                    case 3:
                        income.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.MARCH);
                        break;
                    case 4:
                        income.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.APRIL);
                        break;
                    case 5:
                        income.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.MAY);
                        break;
                    case 6:
                        income.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JUNE);
                        break;
                    case 7:
                        income.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JULY);
                        break;
                    case 8:
                        income.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.AUGUST);
                        break;
                    case 9:
                        income.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.SEPTEMBER);
                        break;
                    case 10:
                        income.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.OCTOBER);
                        break;
                    case 11:
                        income.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.NOVEMBER);
                        break;
                    case 12:
                        income.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.DECEMBER);
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

            switch (incomes.Month)
            {
                case 1:
                    incomes.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JANUARY);
                    break;
                case 2:
                    incomes.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.FEBRUARY);
                    break;
                case 3:
                    incomes.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.MARCH);
                    break;
                case 4:
                    incomes.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.APRIL);
                    break;
                case 5:
                    incomes.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.MAY);
                    break;
                case 6:
                    incomes.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JUNE);
                    break;
                case 7:
                    incomes.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JULY);
                    break;
                case 8:
                    incomes.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.AUGUST);
                    break;
                case 9:
                    incomes.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.SEPTEMBER);
                    break;
                case 10:
                    incomes.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.OCTOBER);
                    break;
                case 11:
                    incomes.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.NOVEMBER);
                    break;
                case 12:
                    incomes.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.DECEMBER);
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
                switch (generalResponse.Month)
                {
                    case 1:
                        generalResponse.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JANUARY);
                        break;
                    case 2:
                        generalResponse.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.FEBRUARY);
                        break;
                    case 3:
                        generalResponse.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.MARCH);
                        break;
                    case 4:
                        generalResponse.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.APRIL);
                        break;
                    case 5:
                        generalResponse.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.MAY);
                        break;
                    case 6:
                        generalResponse.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JUNE);
                        break;
                    case 7:
                        generalResponse.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JULY);
                        break;
                    case 8:
                        generalResponse.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.AUGUST);
                        break;
                    case 9:
                        generalResponse.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.SEPTEMBER);
                        break;
                    case 10:
                        generalResponse.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.OCTOBER);
                        break;
                    case 11:
                        generalResponse.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.NOVEMBER);
                        break;
                    case 12:
                        generalResponse.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.DECEMBER);
                        break;
                    default:
                        break;
                }

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
    }
}
