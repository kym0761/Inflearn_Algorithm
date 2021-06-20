using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Pos
    {
        public Pos(int y, int x) 
        {
            Y = y;
            X = x;
        }
        public int Y;
        public int X;
    }

    class Player
    {
        public int PosX { get; private set; }
        public int PosY { get; private set; }

        Random Rand = new Random();

        Board board;

        enum Direction
        {
            Up = 0,
            Left = 1,
            Down = 2,
            Right = 3
        }

        Direction Dir = Direction.Down;

        List<Pos> Points = new List<Pos>();

        int[] FrontX = new int[] { 0, -1, 0, 1 };
        int[] FrontY = new int[] { -1, 0, 1, 0 };

        int[] RightX = new int[] { 1, 0, -1, 0 };
        int[] RightY = new int[] { 0, -1, 0, 1 };

        public void Initialize(int x, int y, Board inBoard)
        {
            PosX = x;
            PosY = y;

            board = inBoard;

            //RightHandSearch();
            //BFS();

            AStar();
        }

        const int Move_Tick =75;
        int Sum_tick = 0;
        int LastIndex = 0;

        public void Update(int DeltaTime)
        {
            if (Points.Count <= LastIndex)
            {
                //return;
                LastIndex = 0;
                Points.Clear();
                board.Initialize(board.Size,this);
                Initialize(1, 1, board);
            }

            Sum_tick += DeltaTime;

            if (Sum_tick >= Move_Tick)
            {
                Sum_tick = 0;

                PosX = Points[LastIndex].X;
                PosY = Points[LastIndex].Y;

                LastIndex++;
            }


        }

        public bool IsNotDest()
        {
            return PosX != board.DestX || PosY != board.DestY;
        }

        public void Move()
        {
            PosX += FrontX[(int)Dir];
            PosY += FrontY[(int)Dir];
        }

        public void RightHandSearch()
        {
            Points.Add(new Pos(PosY, PosX));

            while (IsNotDest())
            {
                //오른쪽으로 돌고 움직이기
                if (board.Tile[PosY + RightY[(int)Dir], PosX + RightX[(int)Dir]] == Board.TileType.Empty)
                {
                    int temp = ((int)Dir - 1 + 4) % 4;
                    Dir = (Direction)temp;
                    Move();
                    Points.Add(new Pos(PosY, PosX));
                }
                // 직진이 가능하면 직진하기
                else if (board.Tile[PosY + FrontY[(int)Dir], PosX + FrontX[(int)Dir]] == Board.TileType.Empty)
                {
                    Move();
                    Points.Add(new Pos(PosY, PosX));
                }
                //왼쪽으로 돌고 움직이기
                else
                {
                    int temp = ((int)Dir + 1 + 4) % 4;
                    Dir = (Direction)temp;
                }

            }
        }

        public void BFS()
        {
            int[] dx = new int[] { 0, -1, 0, 1 };
            int[] dy = new int[] { -1, 0, 1, 0 };

            bool[,] found = new bool[board.Size, board.Size];
            Pos[,] parent = new Pos[board.Size, board.Size];


            Queue<Pos> queue = new Queue<Pos>();

            queue.Enqueue(new Pos(PosY, PosX));

            found[PosY, PosX] = true;
            parent[PosY, PosX] = new Pos(PosY, PosX);
            
            while (queue.Count>0)
            {
                Pos pos = queue.Dequeue();
                
                int nowX = pos.X;
                int nowY = pos.Y;

                for (int i = 0; i < 4; i++)
                {
                    int nextX = nowX + dx[i];
                    int nextY = nowY + dy[i];

                    if (nextX < 0 || nextY < 0 || nextX >= board.Size || nextY >= board.Size)
                    {
                        continue;
                    }

                    if (board.Tile[nextY, nextX] == Board.TileType.Wall)
                    {
                        continue;
                    }

                    if( found[nextY, nextX] == true)
                    {
                        continue;
                    }


                    queue.Enqueue(new Pos(nextY,nextX));
                    found[nextY, nextX] = true;

                    parent[nextY, nextX] = new Pos(nowY, nowX);
                }

            }

            CalculatePathFromParent(parent);
        }

        class PQNode : IComparable<PQNode>
        {
            public int F;
            public int G;
            public int Y;
            public int X;

            public int CompareTo(PQNode other)
            {
                if (F == other.F)
                {
                    return 0;
                }

                return F < other.F ? 1 : -1;
            }
        }

        public void AStar()
        {

            int[] dx = new int[] { 0, -1, 0, 1/*, -1, -1, 1, 1*/ };
            int[] dy = new int[] { -1, 0, 1, 0/*, -1, 1, 1, -1*/ };
            int[] cost = new int[] { 10, 10, 10, 10/*, 14, 14, 14, 14*/ };
            //F = G+H Minimal is Best
            //G = 시작점에서 해당 좌표까지의 이동하는데 드는 비용
            //H = 휴리스틱, 목적지에서 얼마나 가까운지 알려주는 가산점.

            bool[,] closed = new bool[board.Size, board.Size]; // close list

            int[,] open = new int[board.Size, board.Size]; //open list
            for (int y = 0; y < board.Size; y++)
            {
                for (int x = 0; x < board.Size; x++)
                {
                    open[y, x] = Int32.MaxValue;
                }
            }

            Pos[,] parent = new Pos[board.Size, board.Size];

            //가장 좋은 후보를 찾는다.
            PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();

            //시작점 G = 0 H = X거리 + Y거리
            open[PosY, PosX] = 0 + 10 * (Math.Abs(board.DestY - PosY) + Math.Abs(board.DestX - PosX));
            pq.Push(new PQNode() { F = 10 * (Math.Abs(board.DestY - PosY) + Math.Abs(board.DestX - PosX)), G = 0, Y = PosY, X = PosX });
            parent[PosY, PosX] = new Pos(PosY, PosX);


            while (pq.Count>0)
            {
                PQNode node = pq.Pop();
                //동일한 좌표를 여러 경로로 찾아서, 더 빠른 경로로 인해서 이미 방문(closed)된 경우는 스킵.
                if (closed[node.Y, node.X] == true)
                {
                    continue;
                }

                //방문(closed)
                closed[node.Y, node.X] = true;

                //목적지 도착시 끝.
                if (node.Y == board.DestY && node.X == board.DestX)
                {
                    break;
                }

                //상하좌우 이동 가능한 좌표인지 확인하고 예약(open)함.
                for (int i = 0; i < dy.Length; i++)
                {
                    int nextY = node.Y + dy[i];
                    int nextX = node.X + dx[i];

                    //유효범위 체크
                    if (nextX < 0 || nextY < 0 || nextX >= board.Size || nextY >= board.Size)
                    {
                        continue;
                    }

                    //벽에 막히면 스킵
                    if (board.Tile[nextY, nextX] == Board.TileType.Wall)
                    {
                        continue;
                    }

                    //이미 방문했다면 스킵.
                    if (closed[nextY, nextX] == true)
                    {
                        continue;
                    }

                    //비용 계산
                    int g = node.G + cost[i];
                    int h = 10*(Math.Abs(board.DestY - nextY) + Math.Abs(board.DestX - nextX));

                    //다른 경로에서 더 빠른 길을 찾았다면 스킵함.
                    if (open[nextY, nextX] < g + h)
                    {
                        continue;
                    }
                    //통과하면 값 대입
                    open[nextY, nextX] = g + h;
                    pq.Push(new PQNode() { F = g + h, G = g, Y = nextY, X = nextX });
                    parent[nextY, nextX] = new Pos(node.Y, node.X);

                }
            }

            CalculatePathFromParent(parent);
        }



        public void CalculatePathFromParent(Pos[,] parent)
        {
            int y = board.DestY;
            int x = board.DestX;

            while (parent[y, x].Y != y || parent[y, x].X != x)
            {
                Points.Add(new Pos(y, x));
                Pos pos = parent[y, x]; // current's parent
                y = pos.Y;
                x = pos.X;
            }

            //start point.
            Points.Add(new Pos(y, x));

            Points.Reverse();
        }

    }
}
