using System.Data.Entity;
using www.Models.Mapping;

namespace www.Models
{
	public class SubCrmContext : DbContext
	{
		static SubCrmContext()
		{
			Database.SetInitializer<SubCrmContext>(null);
		}

		public SubCrmContext()
			: base("Name=CrmContext")
		{
		}

        public SubCrmContext(string connectionString)
            : base(connectionString)
        {
        }

		public DbSet<Submission> Votes { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations.Add(new SubmissionMap());
		}
	}
}
