using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.UnityOyun.Assets.Model
{
    public class PlayerBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public DateTime LastCommunicationTime { get; set; }
    }
}
