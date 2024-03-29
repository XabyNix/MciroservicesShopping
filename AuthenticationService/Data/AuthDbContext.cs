using AuthenticationService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Data;

public class AuthDbContext(DbContextOptions options) : IdentityDbContext<User>(options);