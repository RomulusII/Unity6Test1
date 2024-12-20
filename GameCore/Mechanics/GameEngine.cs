using Model;

namespace GameCore.Mechanics
{
    public class GameEngine
    {

        //public List<Unit> Units { get; set; }
        public GameEngine()
        {
        }

        public void MoveNpcs()
        {
            foreach(Unit u in Services.GameServiceStatic.GameService.GameContext.Units)
            {
                
            }
        }
    }
}
