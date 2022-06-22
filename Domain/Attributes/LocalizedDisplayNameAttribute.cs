using Domain.Exceptions;

namespace Domain.Attributes {
	public class LocalizedDisplayNameAttribute : Attribute {
		public string LocalizedDisplayName { get; private set; }

		public LocalizedDisplayNameAttribute(string displayName) {
			LocalizedDisplayName = !String.IsNullOrWhiteSpace(displayName) ? displayName : throw new ModelException("Display name is required.");
		}
	}
}