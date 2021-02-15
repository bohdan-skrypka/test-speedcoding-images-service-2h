using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Images.Database.Models.Business
{
    [Table("Image")]
    public class ImageEntity
    {
        [Key]
        public long Id { get; set; }
    }
}
