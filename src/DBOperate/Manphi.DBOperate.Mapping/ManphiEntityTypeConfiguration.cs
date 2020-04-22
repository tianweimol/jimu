using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mol.DBOperate.Model;

namespace Manphi.DBOperate.Mapping
{
    /// <summary>
    /// 实体配置类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class ManphiEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {

        }
    }
}
