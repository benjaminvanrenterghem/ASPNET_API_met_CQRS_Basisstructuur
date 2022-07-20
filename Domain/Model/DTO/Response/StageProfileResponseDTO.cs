using Domain.Attributes;

namespace Domain.Model.DTO.Response {
	public class StageProfileResponseDTO : ResponseDTO {

		[LocalizedDisplayName("Naam")]
		public string FullName { get; set; }

		public UserResponseDTO Owner { get; set; }

		[LocalizedDisplayName("Over mij")]
		public string About { get; set; }

		[LocalizedDisplayName("Website")]
		public string WebsiteURL { get; set; }
	}
}
