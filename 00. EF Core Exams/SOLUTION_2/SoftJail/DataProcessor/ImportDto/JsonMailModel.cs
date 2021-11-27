using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class JsonMailModel
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        [RegularExpression("[0-9]{1,} [A-Z][a-z]{1,} [A-Z][a-z]{1,} str.")]
        public string Address { get; set; }
    }
}
