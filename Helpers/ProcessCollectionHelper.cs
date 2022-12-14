using ExpenseManagement.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static ExpenseManagement.Models.ViewModels.ReportViewModel;

namespace ExpenseManagement.Helpers
{
    public static class ProcessCollectionHelper
    {
        public static List<Incomes> ProcessCollection(List<Incomes> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = GetIncomesProperty(columnName);
                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                        {
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<Expenses> ProcessCollection(List<Expenses> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = GetExpensesProperty(columnName);
                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                        {
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<Audit> ProcessCollection(List<Audit> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    if (pageSize > 0)
                    {
                        return lstElements.Skip(skip).Take(pageSize).ToList();
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<AppIdentityUser> ProcessCollection(List<AppIdentityUser> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();
                    string searchValue = requestFormData["search[value]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = GetUsersProperty(columnName);

                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                        {
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }

                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<VehiclePurchases> ProcessCollection(List<VehiclePurchases> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = GetVehicleProperty(columnName);
                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                        {
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<VehiclePurchases> ProcessCollectionT(List<VehiclePurchases> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = GetVehicleProperty("purchaseDate");
                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                        {
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<NewVehicleSales> ProcessCollection(List<NewVehicleSales> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();
                    string searchValue = requestFormData["search[value]"].ToString().ToUpper();

                    if (pageSize > 0)
                    {
                        var prop = GetNewVehicleSalesProperty(columnName);
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            if (sortDirection == "asc")
                            {
                                return lstElements.Where(l => l.VehiclePurchase.Chassis.ToUpper().Contains(searchValue)
                                || l.VehiclePurchase.CarModel.CarBrand.Name.ToUpper().Contains(searchValue)
                                || l.VehiclePurchase.CarModel.Name.ToUpper().Contains(searchValue)
                                || l.Salesman.Name.ToUpper().Contains(searchValue)
                                || l.Id.ToString().Contains(searchValue)).OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                            else
                            {
                                return lstElements.Where(l => l.VehiclePurchase.Chassis.ToUpper().ToString().Contains(searchValue)
                                || l.VehiclePurchase.CarModel.CarBrand.Name.ToUpper().ToString().Contains(searchValue)
                                || l.VehiclePurchase.CarModel.Name.ToUpper().ToString().Contains(searchValue)
                                || l.Salesman.Name.ToUpper().Contains(searchValue)
                                || l.Id.ToString().Contains(searchValue)).OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                        }
                        else
                        {
                            if (sortDirection == "asc")
                            {
                                return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                            else
                            {
                                return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<UsedVehicleSales> ProcessCollection(List<UsedVehicleSales> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();
                    string searchValue = requestFormData["search[value]"].ToString().ToUpper();

                    if (pageSize > 0)
                    {
                        var prop = GetUsedVehicleSalesProperty(columnName);
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            if (sortDirection == "asc")
                            {
                                return lstElements.Where(l => l.UsedVehiclePurchases.CarModel.CarBrand.Name.ToUpper().Contains(searchValue)
                                || l.UsedVehiclePurchases.CarModel.Name.ToUpper().Contains(searchValue)
                                || l.PurchasedSalesman.Name.ToUpper().Contains(searchValue)
                                || l.SoldSalesman.Name.ToUpper().Contains(searchValue)
                                || l.Id.ToString().Contains(searchValue)).OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                            else
                            {
                                return lstElements.Where(l => l.UsedVehiclePurchases.CarModel.CarBrand.Name.ToUpper().Contains(searchValue)
                                || l.UsedVehiclePurchases.CarModel.Name.ToUpper().Contains(searchValue)
                                || l.PurchasedSalesman.Name.ToUpper().Contains(searchValue)
                                || l.SoldSalesman.Name.ToUpper().Contains(searchValue)
                                || l.Id.ToString().Contains(searchValue)).OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                        }
                        else
                        {
                            if (sortDirection == "asc")
                            {
                                return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                            else
                            {
                                return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<BankVaults> ProcessCollection(List<BankVaults> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();
                    string searchValue = requestFormData["search[value]"].ToString().ToUpper();

                    if (pageSize > 0)
                    {
                        var prop = GetBankVaultsProperty(columnName);
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            if (sortDirection == "asc")
                            {
                                return lstElements.Where(l => l.Amount.ToString().ToUpper().Contains(searchValue)
                                || l.AccountType.Name.ToUpper().Contains(searchValue)
                                || l.BankBranch.Name.ToUpper().Contains(searchValue)).OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                            else
                            {
                                return lstElements.Where(l => l.Amount.ToString().ToUpper().Contains(searchValue)
                                || l.AccountType.Name.ToUpper().Contains(searchValue)
                                || l.BankBranch.Name.ToUpper().Contains(searchValue)).OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                        }
                        else
                        {
                            if (sortDirection == "asc")
                            {
                                return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                            else
                            {
                                return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<DepositAccounts> ProcessCollection(List<DepositAccounts> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();
                    string searchValue = requestFormData["search[value]"].ToString().ToUpper();

                    if (pageSize > 0)
                    {
                        var prop = GetDepositAccountsProperty(columnName);
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            if (sortDirection == "asc")
                            {
                                return lstElements.Where(l => l.Amount.ToString().ToUpper().Contains(searchValue)
                                || l.BankBranch.Name.ToUpper().Contains(searchValue)).OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                            else
                            {
                                return lstElements.Where(l => l.Amount.ToString().ToUpper().Contains(searchValue)
                                || l.BankBranch.Name.ToUpper().Contains(searchValue)).OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                        }
                        else
                        {
                            if (sortDirection == "asc")
                            {
                                return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                            else
                            {
                                return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<EnergyDaily> ProcessCollection(List<EnergyDaily> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();
                    string searchValue = requestFormData["search[value]"].ToString().ToUpper();

                    if (pageSize > 0)
                    {
                        var prop = GetEnergyDailiesProperty(columnName);
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            if (sortDirection == "asc")
                            {
                                return lstElements.Where(l => l.Kw.ToString().Contains(searchValue)).OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                            else
                            {
                                return lstElements.Where(l => l.Kw.ToString().Contains(searchValue)).OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                        }
                        else
                        {
                            if (sortDirection == "asc")
                            {
                                return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                            else
                            {
                                return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<EnergyMonthlies> ProcessCollection(List<EnergyMonthlies> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();
                    string searchValue = requestFormData["search[value]"].ToString().ToUpper();

                    if (pageSize > 0)
                    {
                        var prop = GetEnergyMonthliesProperty(columnName);
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            if (sortDirection == "asc")
                            {
                                return lstElements.Where(l => l.MonthName.ToUpper().Contains(searchValue)).OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                            else
                            {
                                return lstElements.Where(l => l.MonthName.ToUpper().Contains(searchValue)).OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                        }
                        else
                        {
                            if (sortDirection == "asc")
                            {
                                return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                            else
                            {
                                return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<EnergyLuytobs> ProcessCollection(List<EnergyLuytobs> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();
                    string searchValue = requestFormData["search[value]"].ToString().ToUpper();

                    if (pageSize > 0)
                    {
                        var prop = GetEnergyLuytobsProperty(columnName);
                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                        {
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<PointOfSale> ProcessCollection(List<PointOfSale> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = GetPointOfSaleProperty(columnName);
                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                        {
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<FuelSales> ProcessCollection(List<FuelSales> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = GetFuelSaleProperty(columnName);
                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                        {
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<RaiseDiscountTracking> ProcessCollection(List<RaiseDiscountTracking> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = GetRaiseDiscountTrackingProperty(columnName);
                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                        {
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<Bonuses> ProcessCollection(List<Bonuses> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = GetBonusesProperty(columnName);
                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                        {
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<GeneralResponse> ProcessCollection(List<GeneralResponse> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());

            if (pageSize > 0)
            {
                return lstElements.Skip(skip).Take(pageSize).ToList();
            }
            else
            {
                return lstElements;
            }
        }

        public static List<ToDoListResponse> ProcessCollection(List<ToDoListResponse> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());

            if (pageSize > 0)
            {
                return lstElements.Skip(skip).Take(pageSize).ToList();
            }
            else
            {
                return lstElements;
            }
        }

        public static List<UsedVehiclePurchases> ProcessCollection(List<UsedVehiclePurchases> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = GetUsedVehicleProperty(columnName);
                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                        {
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<PetrolNetProfit> ProcessCollection(List<PetrolNetProfit> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = GetProfitProperty(columnName);
                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                        {
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<InterestIncomes> ProcessCollection(List<InterestIncomes> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = GetInterestIncomeProperty(columnName);
                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                        {
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<PaylistResponse> ProcessCollection(List<PaylistResponse> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());

            if (pageSize > 0)
            {
                return lstElements.Skip(skip).Take(pageSize).ToList();
            }
            else
            {
                return lstElements;
            }
        }

        public static List<WashingIncome> ProcessCollection(List<WashingIncome> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = GetWIProperty(columnName);
                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                        {
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<Tankers> ProcessCollection(List<Tankers> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = GetTankerProperty(columnName);
                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                        {
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<ImportantDocuments> ProcessCollection(List<ImportantDocuments> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();
                    string searchValue = requestFormData["search[value]"].ToString().ToUpper();

                    if (pageSize > 0)
                    {
                        var prop = GetImportantDocumentsProperty(columnName);
                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                        {
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<MyVehicles> ProcessCollection(List<MyVehicles> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = GetMyVehiclesProperty(columnName);
                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                        {
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<LetterOfGuarantees> ProcessCollection(List<LetterOfGuarantees> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = GetLetterOfGuaranteesProperty(columnName);
                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                        {
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        public static List<BankLoans> ProcessCollection(List<BankLoans> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = GetBankLoansProperty(columnName);
                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                        {
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                    }
                    else
                    {
                        return lstElements;
                    }
                }
            }
            return null;
        }

        private static PropertyInfo GetIncomesProperty(string name)
        {
            var properties = typeof(Incomes).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetExpensesProperty(string name)
        {
            var properties = typeof(Expenses).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetAuditsProperty(string name)
        {
            var properties = typeof(Audit).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetUsersProperty(string name)
        {
            var properties = typeof(AppIdentityUser).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetToDoListsProperty(string name)
        {
            var properties = typeof(ToDoLists).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetVehicleProperty(string name)
        {
            var properties = typeof(VehiclePurchases).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetNewVehicleSalesProperty(string name)
        {
            var properties = typeof(NewVehicleSales).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetUsedVehicleSalesProperty(string name)
        {
            var properties = typeof(UsedVehicleSales).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetBankVaultsProperty(string name)
        {
            var properties = typeof(BankVaults).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetDepositAccountsProperty(string name)
        {
            var properties = typeof(DepositAccounts).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetEnergyDailiesProperty(string name)
        {
            var properties = typeof(EnergyDaily).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetEnergyMonthliesProperty(string name)
        {
            var properties = typeof(EnergyMonthlies).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetEnergyLuytobsProperty(string name)
        {
            var properties = typeof(EnergyLuytobs).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetPointOfSaleProperty(string name)
        {
            var properties = typeof(PointOfSale).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetFuelSaleProperty(string name)
        {
            var properties = typeof(FuelSales).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetRaiseDiscountTrackingProperty(string name)
        {
            var properties = typeof(RaiseDiscountTracking).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetBonusesProperty(string name)
        {
            var properties = typeof(Bonuses).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetUsedVehicleProperty(string name)
        {
            var properties = typeof(UsedVehiclePurchases).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetProfitProperty(string name)
        {
            var properties = typeof(PetrolNetProfit).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetInterestIncomeProperty(string name)
        {
            var properties = typeof(InterestIncomes).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetWIProperty(string name)
        {
            var properties = typeof(WashingIncome).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetTankerProperty(string name)
        {
            var properties = typeof(Tankers).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetImportantDocumentsProperty(string name)
        {
            var properties = typeof(ImportantDocuments).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetMyVehiclesProperty(string name)
        {
            var properties = typeof(MyVehicles).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetLetterOfGuaranteesProperty(string name)
        {
            var properties = typeof(LetterOfGuarantees).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private static PropertyInfo GetBankLoansProperty(string name)
        {
            var properties = typeof(BankLoans).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        public class PaginatedResponse<T>
        {
            public List<T> Data { get; set; }

            public int Draw { get; set; }

            public int RecordsFiltered { get; set; }

            public long RecordsTotal { get; set; }
        }
    }
}
