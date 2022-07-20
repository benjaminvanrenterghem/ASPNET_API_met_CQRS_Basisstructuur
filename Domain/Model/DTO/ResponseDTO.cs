namespace Domain.Model.DTO {
	public abstract class ResponseDTO {
		public int? Id { get; set; }
		public bool? Deleted { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime UpdatedDate { get; set; }
		public byte[] RowVersion { get; set; }
	}
}
