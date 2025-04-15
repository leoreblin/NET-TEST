﻿using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.ORM;

public class DefaultContext : DbContext
{
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultContext"/> class.
    /// </summary>
    /// <remarks>Used by EF Core Tools.</remarks>
    public DefaultContext()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultContext"/> class with the specified options.
    /// </summary>
    /// <param name="options">The specified options.</param>
    public DefaultContext(DbContextOptions<DefaultContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}