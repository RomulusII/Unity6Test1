using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Player
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public ICollection<Unit> Units { get; set; }
    }
}
