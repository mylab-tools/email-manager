using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyLab.EmailManager.Domain.Entities;

namespace MyLab.EmailManager.Infrastructure;

public class SendingEntityTypeConfiguration : IEntityTypeConfiguration<Sending>
{
    public void Configure(EntityTypeBuilder<Sending> builder)
    {
        throw new NotImplementedException();
    }
}