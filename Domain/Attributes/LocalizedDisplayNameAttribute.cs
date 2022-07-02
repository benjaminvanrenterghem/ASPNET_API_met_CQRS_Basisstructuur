using Domain.Exceptions;

namespace Domain.Attributes {
	// Voorziet het response object van een extra mapping object PropertyName <-> LocalizedDisplayName
	// Aan te brengen in de gewenste Response DTO
	// Kan gebruikt worden in de frontend om properties middels een alternatieve naamgeving weer te geven
	// Zie ook Model -> Messaging -> Response -> LocalizedProperties, waar middels reflectie de waarden vergaard worden.
	public class LocalizedDisplayNameAttribute : Attribute {
		public string LocalizedDisplayName { get; private set; }

		public LocalizedDisplayNameAttribute(string displayName) {
			LocalizedDisplayName = !String.IsNullOrWhiteSpace(displayName) ? displayName : throw new ModelException("Display name is required to use this attribute.");
		}
	}
}