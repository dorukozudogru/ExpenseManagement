using ExpenseManagement.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExpenseManagement.Helpers
{
    public static class ProcessCollectionHelper
    {
        //public static List<Insurances> ProcessCollection(List<Insurances> lstElements, IFormCollection requestFormData)
        //{
        //    var skip = Convert.ToInt32(requestFormData["start"].ToString());
        //    var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
        //    Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

        //    if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
        //    {
        //        var columnIndex = requestFormData["order[0][column]"].ToString();
        //        var sortDirection = requestFormData["order[0][dir]"].ToString();
        //        tempOrder = new[] { "" };
        //        if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
        //        {
        //            var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();
        //            string searchValue = requestFormData["search[value]"].ToString().ToUpper();

        //            if (pageSize > 0)
        //            {
        //                var prop = GetInsurancesProperty(columnName);
        //                if (!string.IsNullOrEmpty(searchValue))
        //                {
        //                    if (sortDirection == "asc")
        //                    {
        //                        return lstElements.Where(l => l.InsurancePolicyNumber.Contains(searchValue)
        //                        || l.LicencePlate.Contains(searchValue)
        //                        || l.InsuranceCompany.Name.Contains(searchValue)
        //                        || l.Customer.FullName.Contains(searchValue)).OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
        //                    }
        //                    else
        //                    {
        //                        return lstElements.Where(l => l.InsurancePolicyNumber.Contains(searchValue)
        //                        || l.LicencePlate.Contains(searchValue)
        //                        || l.InsuranceCompany.Name.Contains(searchValue)
        //                        || l.Customer.FullName.Contains(searchValue)).OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
        //                    }
        //                }
        //                else
        //                {
        //                    if (sortDirection == "asc")
        //                    {
        //                        return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
        //                    }
        //                    else
        //                    {
        //                        return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                return lstElements;
        //            }
        //        }
        //    }
        //    return null;
        //}

        //public static List<Customers> ProcessCollection(List<Customers> lstElements, IFormCollection requestFormData)
        //{
        //    var skip = Convert.ToInt32(requestFormData["start"].ToString());
        //    var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
        //    Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

        //    if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
        //    {
        //        var columnIndex = requestFormData["order[0][column]"].ToString();
        //        var sortDirection = requestFormData["order[0][dir]"].ToString();
        //        tempOrder = new[] { "" };
        //        if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
        //        {
        //            var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();
        //            string searchValue = requestFormData["search[value]"].ToString().ToUpper();

        //            if (pageSize > 0)
        //            {
        //                var prop = GetCustomersProperty(columnName);
        //                if (!string.IsNullOrEmpty(searchValue))
        //                {
        //                    if (sortDirection == "asc")
        //                    {
        //                        return lstElements.Where(l => l.CitizenshipNo.Contains(searchValue)
        //                        || l.FullName.Contains(searchValue)
        //                        || l.Phone.Contains(searchValue)).OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
        //                    }
        //                    else
        //                    {
        //                        return lstElements.Where(l => l.CitizenshipNo.Contains(searchValue)
        //                        || l.FullName.Contains(searchValue)
        //                        || l.Phone.Contains(searchValue)).OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
        //                    }
        //                }
        //                else
        //                {
        //                    if (sortDirection == "asc")
        //                    {
        //                        return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
        //                    }
        //                    else
        //                    {
        //                        return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                return lstElements;
        //            }
        //        }
        //    }
        //    return null;
        //}

        public static List<Audit> ProcessCollection(List<Audit> lstElements, IFormCollection requestFormData)
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
                        var prop = GetAuditsProperty(columnName);
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            if (sortDirection == "asc")
                            {
                                return lstElements.Where(l => l.Id.ToString().Contains(searchValue)
                                || l.TableName.Contains(searchValue)
                                || l.Action.Contains(searchValue)
                                || l.EntityName.Contains(searchValue)
                                || l.KeyValues.Contains(searchValue)
                                || l.NewValues.Contains(searchValue)
                                || l.OldValues.Contains(searchValue)
                                || l.Username.Contains(searchValue)).OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                            }
                            else
                            {
                                return lstElements.Where(l => l.Id.ToString().Contains(searchValue)
                                || l.TableName.Contains(searchValue)
                                || l.Action.Contains(searchValue)
                                || l.EntityName.Contains(searchValue)
                                || l.KeyValues.Contains(searchValue)
                                || l.NewValues.Contains(searchValue)
                                || l.OldValues.Contains(searchValue)
                                || l.Username.Contains(searchValue)).OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
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

        //private static PropertyInfo GetInsurancesProperty(string name)
        //{
        //    var properties = typeof(Insurances).GetProperties();
        //    PropertyInfo prop = null;
        //    foreach (var item in properties)
        //    {
        //        if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
        //        {
        //            prop = item;
        //            break;
        //        }
        //    }
        //    return prop;
        //}

        //private static PropertyInfo GetCustomersProperty(string name)
        //{
        //    var properties = typeof(Customers).GetProperties();
        //    PropertyInfo prop = null;
        //    foreach (var item in properties)
        //    {
        //        if (item.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
        //        {
        //            prop = item;
        //            break;
        //        }
        //    }
        //    return prop;
        //}

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

        public class PaginatedResponse<T>
        {
            public List<T> Data { get; set; }

            public int Draw { get; set; }

            public int RecordsFiltered { get; set; }

            public long RecordsTotal { get; set; }
        }
    }
}
