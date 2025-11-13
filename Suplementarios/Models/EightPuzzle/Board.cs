using System;
using System.Linq;

namespace EightQueens.Web.Models.EightPuzzle
{
    public enum Direction { Up, Down, Left, Right }

    public class Board
    {
        private readonly int[] tiles;
        public Board(int[] tiles)
        {
            if (tiles == null || tiles.Length != 9) throw new ArgumentException("tiles must be length 9");
            this.tiles = (int[])tiles.Clone();
        }

        public int[] Tiles => (int[])tiles.Clone();

        public static Board FromArray(int[] arr) => new Board(arr);

        public static Board Goal() => new Board(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 0 });

        public Board Copy() => new Board(tiles);

        public string ToKey() => string.Concat(tiles.Select(i => i.ToString()));

        public int BlankIndex => Array.IndexOf(tiles, 0);

        public bool TryMove(Direction d, out Board result)
        {
            int bi = BlankIndex;
            int br = bi / 3, bc = bi % 3;
            int tr = (d == Direction.Up) ? br - 1 : (d == Direction.Down) ? br + 1 : br;
            int tc = (d == Direction.Left) ? bc - 1 : (d == Direction.Right) ? bc + 1 : bc;
            if (tr < 0 || tr > 2 || tc < 0 || tc > 2)
            {
                result = null!;
                return false;
            }
            var newTiles = (int[])tiles.Clone();
            int ti = tr * 3 + tc;
            newTiles[bi] = newTiles[ti];
            newTiles[ti] = 0;
            result = new Board(newTiles);
            return true;
        }

        public bool IsGoal() => ToKey() == Goal().ToKey();

        public bool IsSolvable()
        {
            var list = tiles.Where(x => x != 0).ToArray();
            int inv = 0;
            for (int i = 0; i < list.Length; i++)
                for (int j = i + 1; j < list.Length; j++)
                    if (list[i] > list[j]) inv++;
            return inv % 2 == 0;
        }
    }
}
