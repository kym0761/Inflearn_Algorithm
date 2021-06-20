using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    #region CustomList
    class MyList<T>
    {
        const int DEFAULT_SIZE = 1;

        public int Count = 0;
        public int Capacity { get { return Data.Length; } }

        T[] Data = new T[DEFAULT_SIZE];

        public void Add(T Item)
        {
            if (Count >= Capacity)
            {
                T[] newArray = new T[Count * 2];
                for (int i = 0; i < Count; i++)
                {
                    newArray[i] = Data[i];
                }
                Data = newArray;
            }

            Data[Count] = Item;
            Count++;

        }

        public T this[int Index]
        {
            get { return Data[Index]; }
            set { Data[Index] = value; }
        }


        public void RemoveAt(int Index)
        {
            for (int i = Index; i < Count - 1; i++)
            {
                Data[i] = Data[i + 1];
            }
            Data[Count - 1] = default(T);
            Count--;
        }

    }

    class MyLinkedListNode<T>
    {
        public T Data;
        public MyLinkedListNode<T> Prev;
        public MyLinkedListNode<T> Next;


    }

    class MyLinkedList<T>
    {
        public MyLinkedListNode<T> Head = null;
        public MyLinkedListNode<T> Tail = null;
        public int Count = 0;

        public MyLinkedListNode<T> AddLast(T Data)
        {
            MyLinkedListNode<T> newRoom = new MyLinkedListNode<T>();
            newRoom.Data = Data;

            if (Head == null)
            {
                Head = newRoom;
            }

            if (Tail != null)
            {
                Tail.Next = newRoom;
                newRoom.Prev = Tail;
            }

            Tail = newRoom;
            Count++;

            return newRoom;

        }

        public void Remove(MyLinkedListNode<T> RoomValue)
        {
            if (Head == RoomValue)
            {
                Head = Head.Next;
            }

            if (Tail == RoomValue)
            {
                Tail = Tail.Prev;
            }

            if (RoomValue.Prev != null)
            {
                RoomValue.Prev.Next = RoomValue.Next;
            }

            if (RoomValue.Next != null)
            {
                RoomValue.Next.Prev = RoomValue.Prev;
            }

            Count--;
        }
    }

    #endregion

    class Board
    {

        public TileType[,] Tile { get; private set; }
        public int Size { get; private set; }

        const char CIRCLE = '\u25cf';


        Player player;

        public int DestX { get; private set; }
        public int DestY { get; private set; }

        public enum TileType
        {
            Empty,
            Wall
        }

        public void Initialize(int inSize, Player inPlayer)
        {

            if (inSize % 2 == 0)
            {
                return;
            }

            player = inPlayer;

            Tile = new TileType[inSize, inSize];
            Size = inSize;

            DestX = Size - 2;
            DestY = Size - 2;
            //GenerateByBinaryTree();
            GenerateBySideWinder();
        }

        void GenerateByBinaryTree()
        {
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                    {
                        Tile[y, x] = TileType.Wall;
                    }
                    else
                    {
                        Tile[y, x] = TileType.Empty;
                    }

                }
            }

            Random rand = new Random();
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                    {
                        continue;
                    }

                    if (y == Size - 2 && x == Size - 2)
                    {
                        continue;
                    }



                    if (y == Size - 2)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        continue;
                    }

                    if (x == Size - 2)
                    {
                        Tile[y + 1, x] = TileType.Empty;
                        continue;
                    }


                    if (rand.Next(0, 2) == 0)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                    }
                    else
                    {
                        Tile[y + 1, x] = TileType.Empty;
                    }

                }
            }
        }

        void GenerateBySideWinder()
        {
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                    {
                        Tile[y, x] = TileType.Wall;
                    }
                    else
                    {
                        Tile[y, x] = TileType.Empty;
                    }

                }
            }

            Random rand = new Random();
            for (int y = 0; y < Size; y++)
            {

                int count = 1;

                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                    {
                        continue;
                    }



                    if (y == Size - 2 && x == Size - 2)
                    {
                        continue;
                    }



                    if (y == Size - 2)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        continue;
                    }

                    if (x == Size - 2)
                    {
                        Tile[y + 1, x] = TileType.Empty;
                        continue;
                    }


                    if (rand.Next(0, 2) == 0)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        count++;
                    }
                    else
                    {
                        int randomIndex = rand.Next(0, count);
                      
                        Tile[y + 1, x - randomIndex * 2] = TileType.Empty;

                        count = 1;
                    }

                }
            }
        }



        public void Render()
        {
            ConsoleColor prevColor = Console.ForegroundColor;

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {

                    //player
                    if (y == player.PosY && x == player.PosX)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    else if(y == DestY && x == DestX)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else
                    {
                        Console.ForegroundColor = GetTileColor(Tile[y, x]);
                    }

                    Console.Write(CIRCLE);
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = prevColor;

        }

        ConsoleColor GetTileColor(TileType Type)
        {
            switch (Type)
            {

                case TileType.Empty:
                    return ConsoleColor.Green;
                case TileType.Wall:
                    return ConsoleColor.DarkBlue;
                default:
                    return ConsoleColor.Green;
            }
        }


    }
}
