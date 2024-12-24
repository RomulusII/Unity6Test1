using Data;
using GameCore.Creator;
using GameCore.Map;
using GameCore.Mechanics;
using Microsoft.Extensions.Logging;
using Model;
using Model.UnityOyun.Assets.Model;

namespace GameCore.Services
{

    public static class GameServiceStatic
    {
        private static readonly ILogger<GameService> _logger;
        
        public static GameService GameService { get; } = new(_logger);

        public static PlayerService PlayerService { get; } = new(_logger);

    }

    public class GameService
    {
        private readonly ILogger _logger;

        public HaritaCreator HaritaCreator;

        public GameContext GameContext;
        public Harita Harita { get; }

        public MapService MapService { get; }
        public GameEngine GameEngine { get; } = new();

        public GameService(ILogger logger)
        {
            _logger = logger;
            GameContext = new GameContext();
            Harita = new();
            HaritaCreator = new(Harita);
            MapService = new(logger);

            HaritaCreator.InitHucrelerAsync();
        }
    }

    public class PlayerService
    {
        public Dictionary<int, PlayerSocket> LoggedInPlayers { get; } = new Dictionary<int, PlayerSocket>();

        private readonly ILogger _logger;

        public PlayerService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task Login(PlayerSocket playerSocket)
        {
            if (playerSocket.Player == null) {
                _logger.Log(LogLevel.Error, "Player is null");
                return;
            }

            if (LoggedInPlayers.TryGetValue(playerSocket.Player.Id, out PlayerSocket? loggedInPlayer))
            {
                // Same client wants login, do nothing...
                if (loggedInPlayer == playerSocket)
                    return;

                try
                {
                    await loggedInPlayer.Logout(System.Net.WebSockets.WebSocketCloseStatus.PolicyViolation,
                                                "Another login detected");
                }
                catch (Exception ex)
                {
                    _logger?.LogError($"Cant logout old login. Error: {ex.Message}");
                    // ignore exception
                }

                LoggedInPlayers.Remove(playerSocket.Player.Id);
            }

            LoggedInPlayers.Add(playerSocket.Player.Id, playerSocket);
        }

        public async Task Logout(PlayerSocket ps)
        {
            if (ps?.Player != null && LoggedInPlayers.TryGetValue(ps.Player.Id, 
                                                                  out PlayerSocket? loggedInPlayer))
            {
                await loggedInPlayer.Logout(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, 
                    "User Logout");

                LoggedInPlayers.Remove(ps.Player.Id);
            }
        }

        public static async Task<Player?> GetPlayer(int userId)
        {
            return await Task.Run(() =>
            {
                return GameServiceStatic.GameService.GameContext.Players?.Where(p => p.Id == userId).FirstOrDefault();
            });
        }

        public static async Task<Player?> GetPlayer(string? email, string? password)
        {
            return await Task.Run(() =>
            {
                return GameServiceStatic.GameService.GameContext.Players?
                            .Where(p => p.Email != null 
                                && p.Email.Equals(email) 
                                && p.Password == password)
                            .FirstOrDefault();
            });
        }
    }
}
