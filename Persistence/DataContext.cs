using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace Persistence
{
    public class DataContext : DbContext
    {
        private Encryptor encryptor = new Encryptor();
        public DataContext(DbContextOptions options) : base(options)
        {

        }
      
        public DbSet<UserModel> user { get; set; }
        public DbSet<RouteModel> route { get; set; }
        public DbSet<RouteObstacleModel> routeObstacle { get; set; }
        public DbSet<RouteStationModel> routeStation { get; set; }
        public DbSet<StationModel> station { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            addArrayMembers(modelBuilder);
            encryptMembers(modelBuilder);

            base.OnModelCreating(modelBuilder);

        }


        public void encryptMembers(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    var encrypt = property.PropertyInfo.GetCustomAttributes(typeof(Encrypted), false);
                    if (encrypt.Any())
                    {
                        property.SetValueConverter(encryptor);
                    }
                }
            }
        }
        public void addArrayMembers(ModelBuilder modelBuilder)
        {
            var modelTypes = typeof(DataContext).GetProperties()
                         .Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                         .Select(x => x.PropertyType.GetGenericArguments().First())
                         .ToList();
            foreach (Type modelType in modelTypes)
            {
                var properties = modelType.GetProperties();
                foreach (var property in properties)
                {
                    if (property.PropertyType.IsArray)
                    {
                        if (property.PropertyType.Name == "Int64[]")
                        {
                            var converter = new ValueConverter<long[], string>(
                                               v => JsonConvert.SerializeObject(v),
                                               v => JsonConvert.DeserializeObject<List<long>>(v).ToArray());
                            modelBuilder.Entity(modelType).Property(property.Name).HasConversion(converter);
                        }
                        else if (property.PropertyType.Name == "String[]")
                        {
                            var converter = new ValueConverter<string[], string>(
                                         v => JsonConvert.SerializeObject(v),
                                         v => JsonConvert.DeserializeObject<List<string>>(v).ToArray());
                            modelBuilder.Entity(modelType).Property(property.Name).HasConversion(converter);
                        }
                        else if (property.PropertyType.Name == "Guid[]")
                        {
                            var converter = new ValueConverter<Guid[], string>(
                                           v => JsonConvert.SerializeObject(v),
                                           v => JsonConvert.DeserializeObject<List<Guid>>(v).ToArray());
                            modelBuilder.Entity(modelType).Property(property.Name).HasConversion(converter);
                        }
                    }
                }
            }
        }
    }
}