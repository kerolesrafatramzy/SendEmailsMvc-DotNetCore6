using System.ComponentModel.DataAnnotations;

namespace SendEmailsDotNetCore6.Dtos
{
    public class WelcomeRequestDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
