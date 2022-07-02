namespace Micro2Go.Model {
	public record ParsedJwtToken {
		public int UserId { get; set; }
		public string Email { get; set; } = "";
		public string DisplayName { get; set; } = "";
		public string LoginName { get; set; } = "";
		public List<ClearanceLevel> ClearanceLevels { get; set; } = new();
	}
}
