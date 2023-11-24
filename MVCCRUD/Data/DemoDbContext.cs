﻿using Microsoft.EntityFrameworkCore;
using MVCCRUD.Models.Domain;

namespace MVCCRUD.Data
{
    public class DemoDbContext : DbContext
	{
		public DemoDbContext(DbContextOptions options) : base(options)
		{
		}

        public DbSet<Employee> Employees { get; set; }
    }
}
