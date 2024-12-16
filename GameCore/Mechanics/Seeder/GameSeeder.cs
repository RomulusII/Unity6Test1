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
            GameService.Game.GameContext.TruncateTable("Units");
            GameService.Game.GameContext.TruncateTable("UnitJobs");
            GameService.Game.GameContext.TruncateTable("Chest");
            var rnd = new Random();
            for (int i = 0; i < 3000; i++)
            {
                var randomCell = GameService.Game.Harita.GetRandomLandCell(rnd);
                Unit u = new Unit(npc, randomCell.X, randomCell.Y);
                Services.GameService.Game.GameContext.Units.Add(u);
            }
            Services.GameService.Game.GameContext.SaveChanges();
        }
    }
}
