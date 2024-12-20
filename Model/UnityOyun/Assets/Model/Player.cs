using System.ComponentModel.DataAnnotations;

namespace Model.UnityOyun.Assets.Model
{
    public class PlayerBase
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public DateTime LastCommunicationTime { get; set; }
    }
}
