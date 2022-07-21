namespace Micro2Go.Model {
	public record ParsedJwtToken {
		public int UserId { get; set; }
		public string Email { get; set; } = "";
		public string DisplayName { get; set; } = "";
		public string LoginName { get; set; } = "";
		public List<ClearanceLevel> ClearanceLevels { get; set; } = new();
	}

	// Ten behoeve van unit tests & validatie qua inhoud
	public record VerboseParsedJwtToken : ParsedJwtToken {
		public string Issuer { get; set; }
		public string Audience { get; set; }
		public string Subject { get; set; }
		public DateTime Expiration { get; set; }
	}
}
