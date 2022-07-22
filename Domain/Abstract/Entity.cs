using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract {
	public abstract class Entity : IEntity {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int? Id { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public DateTime CreatedDate { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime UpdatedDate { get; set; }

		[Required]
		public bool Deleted { get; set; } = false;

		// Een ConcurrencyToken, aangezien Mediatr.Send() mogelijk 2 identieke requests, welke dezelfde data behandelen, kan binnenkrijgen en parallel uitvoert.
		// https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/concurrency?view=aspnetcore-6.0#add-a-tracking-property
		// Zie tevens artikel voor uitgebreidere uitleg.

		// todo deze comment opnemen in artikel:
		// Mediatr.Send() functionaliteit zorgt er mogelijk ongewild voor dat meerdere handlers in parallel uitvoeren
		// Er komen bv gelijktijdig, binnen n milliseconden tijd, dezelfde requests binnen welke dezelfde data behandelen
		// Dit zorgt er voor dat er bij concurrency conflicts (Update, Delete) een exception geworpen wordt door EF ipv de reeds aangepaste data door een andere handler te overschrijven
		[Timestamp]
		public byte[]? RowVersion { get; set; } = BitConverter.GetBytes(DateTime.Now.ToBinary());
	}
}
