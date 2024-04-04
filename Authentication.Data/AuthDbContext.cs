using Authentication.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Data;

public class AuthDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options);