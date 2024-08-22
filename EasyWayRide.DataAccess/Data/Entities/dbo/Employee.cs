using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EasyWayRide.DataAccess.Data.Entities.dbo;

// Add profile data for application users by adding properties to the Employee class
public class Employee : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

