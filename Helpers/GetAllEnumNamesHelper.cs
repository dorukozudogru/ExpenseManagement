using ExpenseManagement.Enums;
using ExpenseManagement.Models;
using System.Collections.Generic;
using static ExpenseManagement.Models.ViewModels.ReportViewModel;

namespace ExpenseManagement.Helpers
{
    public static class GetAllEnumNamesHelper
    {
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

        public static List<Endorsements> GetEnumName(List<Endorsements> endorsements)
        {
            foreach (var endorsement in endorsements)
            {
                if (endorsement.AmountCurrency == 0)
                {
                    endorsement.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Endorsements.AmountCurrencyEnum.TRY);
                }
                if (endorsement.AmountCurrency == 1)
                {
                    endorsement.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Endorsements.AmountCurrencyEnum.USD);
                }
                if (endorsement.AmountCurrency == 2)
                {
                    endorsement.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Endorsements.AmountCurrencyEnum.EUR);
                }
                if (endorsement.AmountCurrency == 3)
                {
                    endorsement.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Endorsements.AmountCurrencyEnum.GBP);
                }

                switch (endorsement.Month)
                {
                    case 1:
                        endorsement.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JANUARY);
                        break;
                    case 2:
                        endorsement.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.FEBRUARY);
                        break;
                    case 3:
                        endorsement.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.MARCH);
                        break;
                    case 4:
                        endorsement.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.APRIL);
                        break;
                    case 5:
                        endorsement.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.MAY);
                        break;
                    case 6:
                        endorsement.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JUNE);
                        break;
                    case 7:
                        endorsement.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JULY);
                        break;
                    case 8:
                        endorsement.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.AUGUST);
                        break;
                    case 9:
                        endorsement.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.SEPTEMBER);
                        break;
                    case 10:
                        endorsement.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.OCTOBER);
                        break;
                    case 11:
                        endorsement.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.NOVEMBER);
                        break;
                    case 12:
                        endorsement.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.DECEMBER);
                        break;
                    default:
                        break;
                }
            }
            return endorsements;
        }

        public static Endorsements GetEnumName(Endorsements endorsements)
        {
            if (endorsements.AmountCurrency == 0)
            {
                endorsements.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Endorsements.AmountCurrencyEnum.TRY);
            }
            if (endorsements.AmountCurrency == 1)
            {
                endorsements.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Endorsements.AmountCurrencyEnum.USD);
            }
            if (endorsements.AmountCurrency == 2)
            {
                endorsements.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Endorsements.AmountCurrencyEnum.EUR);
            }
            if (endorsements.AmountCurrency == 3)
            {
                endorsements.AmountCurrencyName = EnumExtensionsHelper.GetDisplayName(Endorsements.AmountCurrencyEnum.GBP);
            }

            switch (endorsements.Month)
            {
                case 1:
                    endorsements.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JANUARY);
                    break;
                case 2:
                    endorsements.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.FEBRUARY);
                    break;
                case 3:
                    endorsements.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.MARCH);
                    break;
                case 4:
                    endorsements.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.APRIL);
                    break;
                case 5:
                    endorsements.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.MAY);
                    break;
                case 6:
                    endorsements.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JUNE);
                    break;
                case 7:
                    endorsements.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.JULY);
                    break;
                case 8:
                    endorsements.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.AUGUST);
                    break;
                case 9:
                    endorsements.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.SEPTEMBER);
                    break;
                case 10:
                    endorsements.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.OCTOBER);
                    break;
                case 11:
                    endorsements.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.NOVEMBER);
                    break;
                case 12:
                    endorsements.MonthName = EnumExtensionsHelper.GetDisplayName(MonthEnum.DECEMBER);
                    break;
                default:
                    break;
            }

            return endorsements;
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
            }
            return generalResponses;
        }
    }
}
