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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var requestFormData = Request.Form;

            List<Audit> audits = await _context.Audits
                .AsNoTracking()
                .ToListAsync();

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

        public class KeyValue
        {
            public string Name { get; set; }
            public int Id { get; set; }
        }
    }
}