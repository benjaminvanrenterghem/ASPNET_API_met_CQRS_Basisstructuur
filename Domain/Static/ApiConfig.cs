using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Static {
	public static class ApiConfig {
		#region Private
		private static string Connection_DataSource = @".\SQLEXPRESS";
		private static string Connection_Database = "BVRNET";
		#endregion

		#region Publics
		#region Publics >> Bare values
		// Om parsen in FE te vergemakkelijken
		public static string ExcSeparator = "::||::";

		public static int DefaultPage = 1;
		public static int DefaultPageSize = 100;
		#endregion

		#region Publics >> Constructions
		public static string ConnectionString => @$"Data Source=${Connection_DataSource};Initial Catalog=${Connection_Database};Integrated Security=True";
		#endregion
		#endregion
	}
}
