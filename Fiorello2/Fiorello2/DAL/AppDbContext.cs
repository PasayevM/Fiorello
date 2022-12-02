﻿using Fiorello2.Models;
using Microsoft.EntityFrameworkCore;

namespace Fiorello2.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
    }
}
