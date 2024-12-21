using System;

#if !UNITY
using System.ComponentModel.DataAnnotations;
#endif

namespace Model.UnityOyun.Assets.Model
{
    public class PlayerBase
    {
#if !UNITY
        [Key]
#endif
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public DateTime LastCommunicationTime { get; set; }
    }
}
