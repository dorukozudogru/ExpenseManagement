using System;

namespace ExpenseManagement.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        
        public string RequestMessage { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
