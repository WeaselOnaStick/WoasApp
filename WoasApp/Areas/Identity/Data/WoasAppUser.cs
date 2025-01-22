using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WoasApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the WoasAppUser class
public class WoasAppUser : IdentityUser
{
    public ICollection<UserLoginTime> LoginTimes { get; set; } = new List<UserLoginTime>();

    public bool Blocked { get; set; } = false;
}


