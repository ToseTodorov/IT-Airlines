using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT_Airlines.Models.UserRoles
{
    public static class Roles
    {
        public static string Administrator { get; } = "Administrator";

        public static string Moderator { get; } = "Moderator";
        public static string User { get; } = "User";
    }
}