using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class JsonDepartmentModel
    {
        public JsonDepartmentModel()
        {
            this.Cells = new HashSet<JsonCellModel>();
        }

        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }
        public ICollection<JsonCellModel> Cells { get; set; }
    }
}
