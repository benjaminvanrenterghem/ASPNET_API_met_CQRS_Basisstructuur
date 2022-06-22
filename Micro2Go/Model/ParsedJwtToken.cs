namespace Micro2Go.Model {
	public record ParsedJwtToken {
		public string Email { get; set; } = "";
		public string Name { get; set; } = "";
		public List<string> Groups { get; set; } = new();
	}
}
