using Data;
using GameCore.Creator;
using GameCore.Map;
using GameCore.Mechanics;
using Model;
using Model.UnityOyun.Assets.Model;

namespace GameCore.Services
{

    public static class GameServiceStatic
    {
        //private static GameService game;
        public static GameService GameService { get; } = new(); 

        public static PlayerService PlayerService { get; } = new();
    }

    public class GameService
    {
        //public Dictionary<int, Player> AllPlayers { get; } = new Dictionary<int, Player>();
        //public Dictionary<int, Unit> AllUnits { get; } = new Dictionary<int, Unit>();

        public HaritaCreator HaritaCreator;

        public GameContext GameContext;
        public Harita Harita { get; }
        public GameEngine GameEngine { get; } = new();

        public GameService()
        {
            GameContext = new GameContext();
            Harita = new();
            HaritaCreator = new(Harita);

            HaritaCreator.InitHucrelerAsync();
        }
    }

    public class PlayerService
    {
        public Dictionary<int, Player> LoggedInPlayers { get; } = new Dictionary<int, Player>();

        public async Task<Player?> Login(string email, string password)
        {
            var player = await GetPlayer(email, password);
            if (player == null) {
                return null;
            }

            LoggedInPlayers.Add(player.Id, player);
            return player;
        }
     
        public static async Task<Player?> GetPlayer(int userId)
        {
            return await Task.Run(() =>
            {
                return GameServiceStatic.GameService.GameContext.Players.Where(p => p.Id == userId).FirstOrDefault();
            });
        }

        public static async Task<Player?> GetPlayer(string email, string password)
        {
            return await Task.Run(() =>
            {
                return GameServiceStatic.GameService.GameContext.Players.Where(p => p.Email.Equals(email) && p.Password == password).FirstOrDefault();
            });
        }
    }
}
