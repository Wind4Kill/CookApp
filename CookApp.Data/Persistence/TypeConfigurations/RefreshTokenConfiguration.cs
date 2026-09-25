using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Model.Entities.UserClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CookApp.Data.Persistence.TypeConfigurations
{
      public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
      {
            public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.RefreshTokenId);
            builder.HasOne(rt => rt.User).WithMany(u=>u.RefreshTokens).HasForeignKey(rt => rt.UserId);
            builder.Property(rt => rt.Token).HasMaxLength(200);
        }
      }
}