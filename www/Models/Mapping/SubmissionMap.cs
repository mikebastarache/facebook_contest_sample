using System.Data.Entity.ModelConfiguration;

namespace www.Models.Mapping
{
    public class SubmissionMap : EntityTypeConfiguration<Submission>
    {
        public SubmissionMap()
        {
            // Primary Key
            HasKey(t => t.filename);

            // Properties
            Property(t => t.location)
                .HasMaxLength(255);

            Property(t => t.caption)
                .HasMaxLength(255);

            Property(t => t.description)
                .HasMaxLength(255);

            // Table & Column Mappings
            ToTable("Submission");
            Property(t => t.userID).HasColumnName("userID");
            Property(t => t.contestID).HasColumnName("contestID");
            Property(t => t.filename).HasColumnName("filename");
            Property(t => t.dateCreated).HasColumnName("dateCreated");
            Property(t => t.caption).HasColumnName("caption");
            Property(t => t.location).HasColumnName("location");
            Property(t => t.description).HasColumnName("description");


        }
    }
}
