using Framework.ExternalMicroservices.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Framework.ExternalMicroservices.Domain.Models
{
    public class AppointmentDBContext : DbContext
    {
        public AppointmentDBContext(DbContextOptions<AppointmentDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    } 
}
