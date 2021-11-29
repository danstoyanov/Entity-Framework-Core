using System.ComponentModel.DataAnnotations;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class ImportEmployeesDto
    {

        [Required]
        [Range(3, 40)]
        public string Username { get; set; }
        
        [Required]
        public string Email { get; set; }

        [Required]
        [RegularExpression("[0-9]{3}-[0-9]{3}-[0-9]{4}")]
        public string Phone { get; set; }
        public int[] Tasks { get; set; }
    }
}
