namespace Domain.Model.DTO.Response {
	public class UserResponseDTO {
		public int? Id { get; set; }
		public string DisplayName { get; set; }
		public string LoginName { get; set; }
		public string Email { get; set; }
		
		// Ignore clause in NetworkDbContext zorgt er voor dat de wachtwoord SHA256 hash niet meekomt bij opvragingen van users uit de databank
		// Belangrijk is dus dat om een User te returnen vanuit een handler deze opnieuw opgevraagd dient te worden om te verzekeren dat de waarde niet aanwezig is.
		public string Password { get; set; }
		public DateTime RegistrationDate { get; set; }
		public List<string> ClearanceLevels { get; set; }

		public List<StageProfileResponseDTO> Profiles { get; set; }
		public bool? Deleted { get; set; }
	}
}
