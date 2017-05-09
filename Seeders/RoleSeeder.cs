using StarterPack.Core.Seeders;
using StarterPack.Models;

namespace StarterPack.Seeders
{
	public class RoleSeeder : ISeeder
	{

        public void EmptyData()
		{
			Role.DeleteAll();
		}

		public void InsertData()
		{
			Role roleAdmin = new Role() {Title = "Admin", Slug = "admin" };
            roleAdmin.Save();
		}
	}
}