using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RabbitPublisher.Models
{
    public class Element
    {
        [Required]
        [DisplayName("Key")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Value")]
        public string Value { get; set; }

        public string asJson()
        {
            return $"{{{Name}:{Value}}}";
        }
    }
}
