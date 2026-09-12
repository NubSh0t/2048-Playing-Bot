namespace Board
{
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    class Board
    {
        public int[,] grid;

        public Board()
        {
            grid = new int[4, 4];
        }

        public int Score()
        {
            int score = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    score += grid[i, j];
                }
            }
            return score;
        }

        public void AddRandomTile()
        {
            if (!IsEmpty())
            {
                return; // No empty space to add a new tile
            }

            Random rand = new Random();
            int x, y;
            do
            {
                x = rand.Next(0, 4);
                y = rand.Next(0, 4);
            } while (grid[x, y] != 0);

            float roll = rand.NextSingle();
            grid[x, y] = roll < 0.9f ? 1 : 2; // Add either a 2 or a 4
        }

        private bool IsEmpty()
        {
            int emptyCount = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (grid[i, j] == 0)
                    {
                        emptyCount++;
                    }
                }
            }
            
            return emptyCount > 0;
        }

        private List<Direction> GenerateMoves()
        {
            List<Direction> moves = new List<Direction>();

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                int[,] oldGrid = DoMove(direction);
                if (!AreGridsEqual(oldGrid, grid))
                {
                    moves.Add(direction);
                }
                UndoMove(oldGrid);
            }

            return moves;
        }

        private bool AreGridsEqual(int[,] grid1, int[,] grid2)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (grid1[i, j] != grid2[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private int[,] DoMove(Direction direction)
        {
            int[,] oldGrid = new int[4, 4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    oldGrid[i, j] = grid[i, j];
                }

                if (direction == Direction.Up)
                {
                    
                }
                else if (direction == Direction.Down)
                {
                    // Implement logic for moving tiles down
                }
                else if (direction == Direction.Left)
                {
                    // Implement logic for moving tiles left
                }
                else if (direction == Direction.Right)
                {
                    // Implement logic for moving tiles right
                }
            }



            return oldGrid;
        }

        private void UndoMove(int[,] oldGrid)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    grid[i, j] = oldGrid[i, j];
                }
            }
        }


        public void Tick()
        {
            List<Direction> moves = GenerateMoves();

            if (moves.Count < 1)
            {
                return; // No moves available
            }
            else
            {
                DoMove(Direction.Up); 
            }
        }
    }
}

