using Domain.Abstract;
using Micro2Go.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model {
	[Table("User")]
    public class User : Entity
    {
        [Required]
        [MaxLength(250)]
        public string DisplayName { get; set; }

        [Required]
        [MaxLength(250)]
        public string LoginName { get; set; }

        [Required]
        [MaxLength(250)]
        public string Email { get; set; }

        [Required]
        [MaxLength(250)]
        public string Password { get; set; }

        [Required]
        public virtual List<ClearanceLevel> ClearanceLevels { get; set; }

		[Required]
        public virtual List<StageProfile> Profiles { get; set; }

        public User () { /*EF*/ }

        public User(string displayName, string loginName, string email, string password, List<ClearanceLevel> clearanceLevels)
        {
            DisplayName = displayName;
            LoginName = loginName;
            Email = email;
            Password = password;
            ClearanceLevels = clearanceLevels ?? new();
        }
    }
}
