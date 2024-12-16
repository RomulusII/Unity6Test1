using Data;
using GameCore.Creator;
using GameCore.Map;
using GameCore.Mechanics;
using Model;

namespace GameCore.Services
{

    public static class GameService
    {
        private static Game game;
        public static Game Game
        {
            get
            {
                if(game == null) { game= new Game(); }
                return game;
            }
        }
        static GameService()
        {
            
        }
    }

    public class Game
    {
        //public Dictionary<int, Player> AllPlayers { get; } = new Dictionary<int, Player>();
        //public Dictionary<int, Unit> AllUnits { get; } = new Dictionary<int, Unit>();

        public HaritaCreator HaritaCreator;

        public GameContext GameContext;
        public Harita Harita { get; }
        public GameEngine GameEngine { get; } = new();

        public Game()
        {
            GameContext = new GameContext();
            Harita = new();
            HaritaCreator = new(Harita);

            HaritaCreator.InitHucrelerAsync();
        }
    }

    public class PlayerService
    {
        public Player? GetPlayer(int userId)
        {
            return GameService.Game.GameContext.Players.Where(p=>p.Id == userId).FirstOrDefault();
        }

    }
}
