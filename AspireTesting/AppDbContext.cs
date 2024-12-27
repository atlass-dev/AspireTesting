﻿using AspireTesting.Models;
using Microsoft.EntityFrameworkCore;

namespace AspireTesting;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options)
        : base(options)
    {
        
    }

    public DbSet<Note> Notes { get; set; }
}
