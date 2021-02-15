using System.ComponentModel.DataAnnotations;

namespace Images.API.DataContracts
{
    public class Image
    {
        [DataType(DataType.Text)]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Firstname { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Lastname { get; set; }
    }
}
