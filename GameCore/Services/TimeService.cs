using System.Diagnostics;

namespace GameCore.Services
{
    public static class TimeService
    {
        public static bool IsGamePaused { get; private set; } = true;

        private static long lastContinueTick;
        private static long lastPauseTick;
        private static long gameTimeElapsed; 
        
        public static long ActualTick => IsGamePaused ? gameTimeElapsed : DateTime.Now.Ticks - lastContinueTick + gameTimeElapsed;

        public static void PauseGame()
        {
            if (IsGamePaused) return;
            if (lastContinueTick == 0) lastContinueTick = DateTime.Now.Ticks;
            gameTimeElapsed = ActualTick;
            IsGamePaused = true;
            lastPauseTick = DateTime.Now.Ticks;
        }

        public static void ContinueGame()
        {
            if (!IsGamePaused) return;
            IsGamePaused = false;

            if (lastPauseTick == 0) lastPauseTick = DateTime.Now.Ticks;

            lastContinueTick = DateTime.Now.Ticks;
        }

        public static string ToTimeString()
        {
            double ms = ActualTick % 10000000;

            const long dayTick = 24 * 60 * 60 * (long)10000000;
            long days = ActualTick / dayTick;

            DateTime dt = new DateTime(ActualTick);


            if (IsGamePaused) return $"Tick:{days} days, {dt:HH:mm:ss}.{ms} (Game paused) ";
            return $"Tick:{days} days, {dt:HH:mm:ss}.{ms}";
        }
    }
}
