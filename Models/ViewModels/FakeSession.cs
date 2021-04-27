using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManagement.Models.ViewModels
{
    public class FakeSession
    {
        private static FakeSession _instance;

        public static FakeSession Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                _instance = new FakeSession();
                return _instance;
            }
        }

        public string Obj { get; set; }
        public byte[] Buffer { get; set; }

    }
}
