﻿
using helpingout.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace ServidorExemplo.Data
{

    public class ApiContext : DbContext
    {
        public DbSet<Usuario> usuarios { get; set; }
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {

        }
    }
}