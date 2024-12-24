using Model.UnityOyun.Assets.Model;

namespace Model
{
    public class Player : PlayerBase
    {
        public ICollection<Unit> Units { get; set; } = new List<Unit>();
    }
}
