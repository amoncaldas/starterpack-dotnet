using System.Collections.Generic;
using StarterPack.Core.Extensions;
using StarterPack.Core.Seeders;
using StarterPack.Models;

namespace StarterPack.Seeders
{
	public class UserSeeder : ISeeder
	{
		public void EmptyData()
		{
			User.Delete();
		}

		public void InsertData()
		{
			User user  = new User(){Name = "Admin", Email = "admin-base@prodeb.com"};
            user.DefinePassword("Prodeb01");
            user.Roles =  Role.Where(r => r.Slug == "admin").Fetch();
			user.mapFromRoles();
            user.Save();
		}
	}
}
