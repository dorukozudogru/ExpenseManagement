using System.ComponentModel;

namespace ExpenseManagement.Models.ViewModels
{
    public class RoleAssignViewModel
    {
        [DisplayName("Id")]
        public string RoleId { get; set; }
        [DisplayName("Rol Adı")]
        public string RoleName { get; set; }
        [DisplayName("Atanmış Mı?")]
        public bool HasAssign { get; set; }
    }

}
