using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces {
	public interface INetworkDbContext {
		DbSet<Profile> Profiles { get; set; }
	}
}
