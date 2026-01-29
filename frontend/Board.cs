namespace Board
{
    enum MoveType{
        Left,
        Right,
        Up,
        Down
    }

    struct Move{
        public ulong key;
        public MoveType dir;
    }

    class Board
    {
        public int[,] grid;

        List<Move> moveList;

        public Board()
        {
            grid=new int[4, 4];
            moveList=new List<Move>();
        }
            

        private List<(int, int)> emptyCells()
        {
            List<(int, int)> empty = new List<(int, int)>();

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (grid[i, j] == 0)
                        empty.Add((i, j));

            return empty;
        }

        public void addRandomTile()
        {
            List<(int, int)> empty = emptyCells();
            if (empty.Count == 0)
                return;

            Random rand = new Random();
            var (i, j) = empty[rand.Next(empty.Count)];
            grid[i, j] = (rand.NextDouble() < 0.9) ? 1 : 2;
        }
        public bool doMove(MoveType dir)
        {
            bool moved = false;
            Move m = new Move();
            m.key=toKey();
            m.dir=dir;

            if (dir== MoveType.Up)
            {
                for (int j=0; j<4; j++)
                {
                    int i=1;
                    while (i<4){
                        if(grid[i,j]==grid[i-1,j] && grid[i,j]!=0){
                            grid[i-1,j]+=1;
                            grid[i,j]=0;
                            i=1;
                            moved=true;
                        }            
                        else if(grid[i-1,j]==0 && grid[i,j]!=0){
                            grid[i-1,j]=grid[i,j];
                            grid[i,j]=0;
                            i=1;
                            moved=true;
                        }
                        else{
                            i+=1;
                        }

                    }
                }
            }
            else if (dir==MoveType.Down)
            {
                for (int j=0; j<4; j++)
                {
                    int i=2;
                    while (i>=0){
                        if(grid[i,j]==grid[i+1,j] && grid[i,j]!=0){
                            grid[i+1,j]+=1;
                            grid[i,j]=0;
                            i=2;
                            moved=true;
                        }            
                        else if(grid[i+1,j]==0 && grid[i,j]!=0){
                            grid[i+1,j]=grid[i,j];
                            grid[i,j]=0;
                            i=2;
                            moved=true;
                        }
                        else{
                            i-=1;
                        }

                    }
                }
            }
            else if (dir==MoveType.Left)
            {
                for (int i=0; i<4; i++)
                {
                    int j=1;
                    while (j<4){
                        if(grid[i,j]==grid[i,j-1] && grid[i,j]!=0){
                            grid[i,j-1]+=1;
                            grid[i,j]=0;
                            j=1;
                            moved=true;
                        }            
                        else if(grid[i,j-1]==0 && grid[i,j]!=0){
                            grid[i,j-1]=grid[i,j];
                            grid[i,j]=0;
                            j=1;
                            moved=true;
                        }
                        else{
                            j+=1;
                        }

                    }
                }
            }
            else if (dir==MoveType.Right)
            {
                for (int i=0; i<4; i++)
                {
                    int j=2;
                    while (j>=0){
                        if(grid[i,j]==grid[i,j+1] && grid[i,j]!=0){
                            grid[i,j+1]+=1;
                            grid[i,j]=0;
                            j=2;
                            moved=true;
                        }            
                        else if(grid[i,j+1]==0 && grid[i,j]!=0){
                            grid[i,j+1]=grid[i,j];
                            grid[i,j]=0;
                            j=2;
                            moved=true;
                        }
                        else{
                            j-=1;
                        }

                    }
                }
            }

            if (moved){
                moveList.Add(m);
            }

            return moved;
        }

        public bool undoMove(){
            int index=moveList.Count-1;

            if (index==-1){
                return false;
            }
            Move m=moveList[index];
            toBoard(m.key);
            moveList.Remove(m);
            return true;
        }


        private ulong toKey(){
            ulong key = 0;
            int shift = 0;

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    key |= ((ulong)grid[i, j] & 0xF) << shift;
                    shift += 4;
                }

            return key;
        }

        private void toBoard(ulong key){
            int shift = 0;

            for (int i = 0; i < 4; i++){
                for (int j = 0; j < 4; j++)
                {
                    grid[i, j] = (int)((key >> shift) & 0xF);
                    shift += 4;
                }
            }
        }


        public int evaluateBoard(){
            int eval=0;
            int highest=0;

            int[,] gradient =new int[4,4]{
                {0,1,2,3},
                {7,6,5,4},
                {8,9,10,11},
                {15,14,13,12}
            };

            for (int i=0;i<4;i++){
                for (int j=0;j<4;j++){
                    highest=Math.Max(grid[i,j],highest);
                }
            }
            
            for (int i=0;i<4;i++){
                for (int j=0;j<4;j++){
                    if (grid[i,j]==highest-gradient[i,j]){
                        eval+=1<<grid[i,j];
                    }
                }
            }

            return eval;
        }

        public int scoreBoard(){
            int score=0;

            for (int i=0;i<4;i++){
                for (int j=0;j<4;j++){

                    if (grid[i,j]>0){
                        score+=1 << grid[i, j];
                    }

                }
            }

            return score;
        }

        private MoveType bestMove(){

            MoveType[] moves = {
                MoveType.Left,
                MoveType.Up,
                MoveType.Right,
                MoveType.Down
            };

            int besteval=-1;
            MoveType bestmove = MoveType.Left;

            foreach (MoveType move in moves)
            {
                if(doMove(move)){
                    int eval = expectiMax(false,5,0,999999,1);

                    if (eval > besteval){
                        bestmove=move;
                        besteval=eval;
                    }
                    undoMove();
                }

            }

            return bestmove;
        }

        private int expectiMax(bool ismaximizing,int depth,int alpha,int beta,double prob){
            if (depth==0 || prob <0.0001){
                return evaluateBoard();
            }

            if (ismaximizing){

                List<MoveType> moves = new List<MoveType>(){
                MoveType.Left,
                MoveType.Right,
                MoveType.Up,
                MoveType.Down
                };

                int eval = 0;

                foreach (MoveType move in moves)
                {
                    if(doMove(move)){
                        eval = Math.Max(expectiMax(!ismaximizing,depth-1,alpha,beta,prob),eval);
                        undoMove();
                    }

                    alpha = Math.Max(alpha, eval);
                    if(beta <= alpha)
                        break;
                }

                return eval;

            }
            else
            {
                List<(int,int)> tiles = emptyCells();

                int eval = 999999;

                foreach ((int,int) tile in tiles)
                {
                    grid[tile.Item1,tile.Item2] = 1;
                    eval = Math.Min(expectiMax(!ismaximizing,depth-1,alpha,beta,prob*0.9),eval);
                    grid[tile.Item1,tile.Item2] = 2;
                    eval = Math.Min(expectiMax(!ismaximizing,depth-1,alpha,beta,prob*0.1),eval);
                    grid[tile.Item1,tile.Item2] = 0;

                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                        break;
                    
                }

                return eval;
            }

        }

        public void tick(){
            MoveType b = bestMove();
            
            if (doMove(b)){
                addRandomTile();
            }
        }   
          
    }
}

