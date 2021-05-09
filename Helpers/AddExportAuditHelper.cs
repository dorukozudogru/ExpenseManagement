using ExpenseManagement.Data;
using ExpenseManagement.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManagement.Helpers
{
    public class AddExportAuditHelper
    {
        [Authorize(Roles = "Admin, Banaz, Muhasebe")]
        public static void AddExportAudit(string pageName, string username, ExpenseContext _context)
        {
            Audit audit = new Audit()
            {
                Action = "Exported",
                DateTime = DateTime.Now.ToUniversalTime(),
                KeyValues = "{\"Id\":\"-\"}",
                NewValues = "{\"PageName\":\"" + pageName + "\"}",
                TableName = pageName,
                Username = username
            };
            _context.Add(audit);
            _context.SaveChanges();
        }
    }
}
