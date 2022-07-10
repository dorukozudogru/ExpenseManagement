using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManagement.Models.ViewModels
{
    public class FakeSessionSec
    {
        private static FakeSessionSec _instance;

        public static FakeSessionSec Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                _instance = new FakeSessionSec();
                return _instance;
            }
        }

        public string Obj { get; set; }
        public byte[] Buffer { get; set; }

    }
}
