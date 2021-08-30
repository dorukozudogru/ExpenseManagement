using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ExpenseManagement.Models;
using static ExpenseManagement.Helpers.ProcessCollectionHelper;
using ExpenseManagement.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using static ExpenseManagement.Models.ViewModels.ReportViewModel;

namespace ExpenseManagement.Controllers
{
    [Authorize(Roles = ("Admin, Banaz, Muhasebe"))]
    public class AuditController : Controller
    {
        private readonly ExpenseContext _context;

        public AuditController(ExpenseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tableNames = _context.Audits
                .GroupBy(i => i.TableName)
                .Select(i => new Audit
                {
                    TableName = i.Key
                })
                .ToList();

            tableNames.Add(new Audit
            {
                TableName = ""
            });

            ViewData["TableName"] = new SelectList(tableNames.OrderBy(s => s.TableName).ToList(), "TableName", "TableName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post(bool isFiltered, string tableName, DateTime startDate, DateTime finishDate, string act, string changedUser)
        {
            var requestFormData = Request.Form;

            List<Audit> audits = await _context.Audits
                .AsNoTracking()
                .ToListAsync();

            if (isFiltered != false)
            {
                if (tableName != null)
                {
                    audits = audits.Where(e => e.TableName == tableName).ToList();
                }
                if (startDate != DateTime.MinValue && finishDate != DateTime.MinValue)
                {
                    audits = audits.Where(e => e.DateTime >= startDate && e.DateTime <= finishDate).ToList();
                }
                if (act != null)
                {
                    audits = audits.Where(e => e.Action == act).ToList();
                }
                if (changedUser != null)
                {
                    audits = audits.Where(e => e.Username != null && e.Username.ToUpper().Contains(changedUser.ToUpper())).ToList();
                }
            }

            List<Audit> lastAuditList = new List<Audit>();

            foreach (var audit in audits)
            {
                try
                {
                    var keyValue = !string.IsNullOrEmpty(audit.KeyValues) ? JsonConvert.DeserializeObject<Dictionary<string, string>>(audit.KeyValues) : null;
                    var oldValue = !string.IsNullOrEmpty(audit.OldValues) ? JsonConvert.DeserializeObject<Dictionary<string, string>>(audit.OldValues) : null;
                    var newValue = !string.IsNullOrEmpty(audit.NewValues) ? JsonConvert.DeserializeObject<Dictionary<string, string>>(audit.NewValues) : null;
                    var username = !string.IsNullOrEmpty(audit.Username) ? audit.Username : "-";

                    if (audit.Action == "Modified")
                    {
                        foreach (var value in oldValue)
                        {
                            if (value.Key != "InvoiceImage")
                            {
                                if (value.Key != "Id")
                                {
                                    var _tempNew = newValue.Where(nv => nv.Key == value.Key && nv.Value?.ToString() != value.Value?.ToString()).ToList().FirstOrDefault();
                                    string _tempOld = value.Value;

                                    if (_tempNew.Value != null && _tempNew.Key != null)
                                    {
                                        lastAuditList.Add(new Audit
                                        {
                                            Action = audit.Action,
                                            DateTime = audit.DateTime,
                                            EntityName = _tempNew.Key.ToString(),
                                            Id = audit.Id,
                                            KeyValues = keyValue.FirstOrDefault().Value.ToString(),
                                            NewValues = _tempNew.Value.ToString(),
                                            OldValues = _tempOld.ToString(),
                                            TableName = audit.TableName,
                                            Username = username
                                        });
                                    }
                                }
                            }

                        }
                    }
                    else if (audit.Action == "Added")
                    {
                        foreach (var value in newValue)
                        {
                            if (value.Key != "InvoiceImage")
                            {
                                if (value.Value != null && value.Key != null)
                                {
                                    lastAuditList.Add(new Audit
                                    {
                                        Action = audit.Action,
                                        DateTime = audit.DateTime,
                                        EntityName = value.Key.ToString(),
                                        Id = audit.Id,
                                        KeyValues = keyValue.FirstOrDefault().Value.ToString(),
                                        NewValues = value.Value.ToString(),
                                        OldValues = "-",
                                        TableName = audit.TableName,
                                        Username = username
                                    });
                                }
                            }
                        }
                    }
                    else if (audit.Action == "Deleted")
                    {
                        foreach (var value in oldValue)
                        {
                            if (value.Key != "InvoiceImage")
                            {
                                if (value.Value != null && value.Key != null)
                                {
                                    lastAuditList.Add(new Audit
                                    {
                                        Action = audit.Action,
                                        DateTime = audit.DateTime,
                                        EntityName = value.Key.ToString(),
                                        Id = audit.Id,
                                        KeyValues = keyValue.FirstOrDefault().Value.ToString(),
                                        NewValues = "-",
                                        OldValues = value.Value.ToString(),
                                        TableName = audit.TableName,
                                        Username = username
                                    });
                                }
                            }
                        }
                    }
                    else if (audit.Action == "Exported")
                    {
                        foreach (var value in newValue)
                        {
                            if (value.Value != null && value.Key != null)
                            {
                                lastAuditList.Add(new Audit
                                {
                                    Action = audit.Action,
                                    DateTime = audit.DateTime,
                                    EntityName = "-",
                                    Id = audit.Id,
                                    KeyValues = keyValue.FirstOrDefault().Value.ToString(),
                                    NewValues = "-",
                                    OldValues = "-",
                                    TableName = audit.TableName,
                                    Username = username
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            List<Audit> listItems = ProcessCollection(lastAuditList, requestFormData);

            var response = new PaginatedResponse<Audit>
            {
                Data = listItems,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = lastAuditList.Count,
                RecordsTotal = lastAuditList.Count
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UserAuditPost(string username, DateTime startDate, DateTime finishDate)
        {
            var requestFormData = Request.Form;

            var audits = await _context.Audits
                .Where(e => e.Username == username)
                .Where(e => e.DateTime >= startDate && e.DateTime <= finishDate)
                .GroupBy(e => new
                {
                    e.Action,
                    e.TableName
                })
                .Select(e => new AuditResponse
                {
                    ActionName = e.Key.Action,
                    TableName = e.Key.TableName,
                    ActionCount = e.Count()
                })
                .OrderBy(e => e.ActionName)
                .AsNoTracking()
                .ToListAsync();

            var response = new PaginatedResponse<AuditResponse>
            {
                Data = audits,
                Draw = int.Parse(requestFormData["draw"]),
                RecordsFiltered = audits.Count,
                RecordsTotal = audits.Count
            };

            return Ok(response);
        }

        public class KeyValue
        {
            public string Name { get; set; }
            public int Id { get; set; }
        }
    }
}