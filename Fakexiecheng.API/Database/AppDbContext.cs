﻿using Fakexiecheng.API.Models;
using Fakexiecheng.API.Moldes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Fakexiecheng.API.Database
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {


        }

        public DbSet<TouristRoute> TouristRoutes { get; set; }
        public DbSet<TouristRoutePicture> TouristRoutePictures { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<LineItem> LineItems { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Form_TD> Form_TD { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
                        modelBuilder.Entity<TouristRoute>().HasData(new TouristRoute()
                        {
                            Id = Guid.NewGuid(),
                            Title = "ceshititle",
                            Description = "shuoming",
                            OriginalPrice = 0,
                            CreatTime = DateTime.UtcNow



                        }) ;*/

            var touristRouteJsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Database/touristRoutesMockData.json");
            IList<TouristRoute> touristRoutes = JsonConvert.DeserializeObject<IList<TouristRoute>>(touristRouteJsonData);
            modelBuilder.Entity<TouristRoute>().HasData(touristRoutes);

            var touristRoutePictureJsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Database/touristRoutePicturesMockData.json");
            IList<TouristRoutePicture> touristRoutePictures = JsonConvert.DeserializeObject<IList<TouristRoutePicture>>(touristRoutePictureJsonData);
            modelBuilder.Entity<TouristRoutePicture>().HasData(touristRoutePictures);

            //初始化用户与角色的种子数据
            //1.更新用户与角色的外键
            modelBuilder.Entity<ApplicationUser>(u =>
            u.HasMany(x => x.UserRoles)
            .WithOne().HasForeignKey(ur => ur.UserId).IsRequired());
            //2.添加管理员角色
            var adminRoleId = "308660dc-ae51-480f-824d-7dca6714c3e2"; // guid 
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                }
            );

            // 3. 添加用户
            var adminUserId = "90184155-dee0-40c9-bb1e-b5ed07afc04e";
            ApplicationUser adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin@fakexiecheng.com",
                NormalizedUserName = "admin@fakexiecheng.com".ToUpper(),
                Email = "admin@fakexiecheng.com",
                NormalizedEmail = "admin@fakexiecheng.com".ToUpper(),
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                PhoneNumber = "123456789",
                PhoneNumberConfirmed = false
            };
            //密码不用明文存
            var ph = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = ph.HashPassword(adminUser, "Fake123$");
            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            // 4. 给用户加入管理员权限
            // 通过使用 linking table：IdentityUserRole
            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasData(new IdentityUserRole<string>()
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                });

            base.OnModelCreating(modelBuilder);
        }


    }
}
