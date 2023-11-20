using Bombazo.Model;
using Bombazo.Presistance.FileManager;

namespace Bombazo.Presistance
{
    public static class LoadGame
    {
        public static async Task<GameMap> LoadAsync( ITxtFileManager fileManager)
        {
            GameMap game = new GameMap();
            string[] lines;
            try
            {
                lines = await fileManager.LoadAsync();             
            }
            catch (FileManagerException exc)
            {
                throw new FileValidationException($"Hibás fájl! \n{exc.Message}", exc);
            }

            int.TryParse(lines[0], out game.Size);
            string actualLine;
            string lineWithoutBrackets;
            
            //SetWalls
            actualLine = lines[1];
            lineWithoutBrackets = actualLine.Substring(actualLine.IndexOf("[", StringComparison.Ordinal) + 1,
                actualLine.IndexOf("]", StringComparison.Ordinal) - actualLine.IndexOf("[", StringComparison.Ordinal) - 1);

            string[] walls = lineWithoutBrackets.Split(';');
            int x;
            int y;

            foreach (var item in walls)
            {
                if (
                    !int.TryParse(item.Split(",")[0], out x)
                    || !int.TryParse(item.Split(",")[1], out y)
                    || x < 0
                    || y < 0
                    || (x == 1 && y == 1)
                    || (x == 0 && y == 0)
                    || (x == 0 && y == 1)
                    || (x == 1 && y == 0)
                    || x >= game.Size
                    || y >= game.Size)
                {
                    throw new FileValidationException("Hibásak bemeneti fájlban lévő adatok.");
                }

                game.Walls.Add(new Position(x, y));
            }
            actualLine = lines[2];
            lineWithoutBrackets = actualLine.Substring(actualLine.IndexOf("[", StringComparison.Ordinal) + 1,
                actualLine.IndexOf("]", StringComparison.Ordinal) - actualLine.IndexOf("[", StringComparison.Ordinal) - 1);
            string[] enemies = lineWithoutBrackets.Split(';');
            if (enemies.Length == 0) throw new FileValidationException("Nincs ellenség.");
            foreach (var item in enemies)
            {
                if (
                    !int.TryParse(item.Split(",")[0], out x)
                    || !int.TryParse(item.Split(",")[1], out y)
                    || x < 0
                    || y < 0
                    || (x == 0 && y == 0)
                    || x >= game.Size
                    || y >= game.Size)
                {
                    throw new FileValidationException("Hibás bemeneti fájl.");
                }

                game.Enemies.Add(new Position(x, y));
            }
            return game;
        }
    }
}