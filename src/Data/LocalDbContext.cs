using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Dot.Net.WebApi.Domain;

namespace Dot.Net.WebApi.Data
{
    public class LocalDbContext : DbContext
    {
        public DbSet<User> Users { get; set;}
    }
}