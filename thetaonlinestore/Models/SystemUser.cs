using System;
using System.Collections.Generic;

namespace thetaonlinestore.Models
{
    public partial class SystemUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string CV { get; set; }
        public string Image { get; set; }

    }
}
