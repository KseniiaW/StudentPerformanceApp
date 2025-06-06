﻿using Microsoft.EntityFrameworkCore;
using StudentPerformanceApp.Models;

namespace StudentPerformanceApp.Data
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
    }
}