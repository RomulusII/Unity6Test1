using GameCore.Services;
using Model;

namespace GameCore.Mechanics.Seeder
{
    public class GameSeeder
    {
        public static void SeedGame()
        {
            SeedUnits();
        }


        private static void SeedUnits()
        {
            var npc = new Player();
            GameServiceStatic.GameService.GameContext.TruncateTable("Units");
            GameServiceStatic.GameService.GameContext.TruncateTable("UnitJobs");
            GameServiceStatic.GameService.GameContext.TruncateTable("Chest");
            var rnd = new Random();
            for (int i = 0; i < 3000; i++)
            {
                var randomCell = GameServiceStatic.GameService.Harita.GetRandomLandCell(rnd);
                Unit u = new Unit(npc, randomCell.X, randomCell.Y);
                Services.GameServiceStatic.GameService.GameContext.Units.Add(u);
            }
            Services.GameServiceStatic.GameService.GameContext.SaveChanges();
        }
    }
}
