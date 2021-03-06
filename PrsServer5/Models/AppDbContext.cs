using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace PrsServer5.Models {

    public class AppDbContext : DbContext {

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<Requestline> Requestlines { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<User>(e => e.HasIndex(x => x.Username).IsUnique(true));
            builder.Entity<Vendor>(e => e.HasIndex(x => x.Code).IsUnique(true));
            builder.Entity<Product>(e => e.HasIndex(x => x.PartNbr).IsUnique(true));
        }

    }
}
