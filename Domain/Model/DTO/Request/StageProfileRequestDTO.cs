namespace Domain.Model.DTO.Request {
	public class StageProfileRequestDTO : RequestDTO {
		public string FullName { get; set; }
		public int OwnerUserId { get; set; }
		public string About { get; set; }
		public string WebsiteURL { get; set; }
		public bool? Deleted { get; set; }
	}
}
