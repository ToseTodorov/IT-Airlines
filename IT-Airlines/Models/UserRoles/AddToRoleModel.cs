using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT_Airlines.Models.UserRoles
{
    public class AddToRoleModel
    {
        public string Email { get; set; }
        public string selectedRole { get; set; }
        public List<String> roles { get; set; }

        public AddToRoleModel()
        {
            roles = new List<string>();
        }
    }
}