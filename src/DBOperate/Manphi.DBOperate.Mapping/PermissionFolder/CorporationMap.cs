using Manphi.DBOperate.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Manphi.DBOperate.Mapping
{
    public class CorporationMap: ManphiEntityTypeConfiguration<Corporation>
    {
        public override void Configure(EntityTypeBuilder<Corporation> builder)
        {
            builder.ToTable("tb_Corporation");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).HasMaxLength(400).IsRequired();
            builder.Property(t => t.Code).HasMaxLength(400).IsRequired();
            //// 语言-翻译 一对多
            //builder.HasOne(t => t.Language).WithMany(c => c.AutoTranses)
            //    .HasForeignKey(cl => cl.LanguageID).OnDelete(DeleteBehavior.Cascade);
            //// 词条-翻译一对多
            //builder.HasOne(cl => cl.StdString).WithMany(l => l.AutoTranses)
            //    .HasForeignKey(cl => cl.StdStringID).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
