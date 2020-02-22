using Partify.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Partify.Repositories
{
    public class PartifyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<Invite> Invites { get; set; }

        public PartifyDbContext() : base("PartifyDbContext")
        {
            Users = this.Set<User>();
            Parties = this.Set<Party>();
            Invites = this.Set<Invite>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invite>()
                        .HasRequired(c => c.Receiver)
                        .WithMany()
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Invite>()
                        .HasRequired(s => s.Sender)
                        .WithMany()
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Invite>()
                        .HasRequired(s => s.Party)
                        .WithMany()
                        .WillCascadeOnDelete(false);

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }
    }
}