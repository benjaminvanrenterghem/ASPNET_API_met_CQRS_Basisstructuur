namespace Domain.Model.DTO.Request {
	public class StageProfileRequestDTO {
		public int? Id { get; set; }
		public string FullName { get; set; }
		public int OwnerUserId { get; set; }
		public string About { get; set; }
		public string WebsiteURL { get; set; }
		public bool? Deleted { get; set; }
	}
}
