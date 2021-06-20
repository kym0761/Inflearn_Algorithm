using System;
using System.Collections.Generic;

namespace Ex
{
    class Graph
    {
        int[,] adj = new int[6, 6]
        {
           {-1,15,-1,35,-1,-1 },
           {15,-1,5,10,-1,-1 },
           {-1,5,-1,-1,-1,-1 },
           {35,10,-1,-1,5,-1 },
           {-1,-1,-1,5,-1,5 },
           {-1,-1,-1,-1,5,-1 }
        };

        List<int>[] adj2 = new List<int>[]
        {
            new List<int>(){1,3 },
            new List<int>(){0,2,3 },
            new List<int>(){1 },
            new List<int>(){0,1,4},
            new List<int>(){3,5 },
            new List<int>(){4 }
        };

        bool[] visited = new bool[6];

        public void DFS(int now)
        {
            Console.WriteLine(now);
            visited[now] = true;

            for (int next = 0; next < 6; next++)
            {
                if (adj[now, next] == 0 || visited[next] == true)
                {
                    continue;
                }

                //재귀
                DFS(next);
            }
        }

        public void DFS2(int now)
        {
            Console.WriteLine(now);
            visited[now] = true;

            foreach (int next in adj2[now])
            {
                if (visited[next] == true)
                {
                    continue;
                }
                DFS2(next);
            }
        }

        public void SearchAll()
        {
           // visited = new bool[6];
            for (int now = 0; now < 6; now++)
            {
                if (visited[now] == false)
                {
                    DFS(now);
                }
            }
        
        }


        public void BFS(int now)
        {
            bool[] found = new bool[6];

            int[] parent = new int[6];
            int[] distance = new int[6];

            Queue<int> q = new Queue<int>();

            q.Enqueue(now);
            found[now] = true;

            while (q.Count > 0)
            {
                int current = q.Dequeue();
                Console.WriteLine(current);

                for (int next = 0; next < 6; next++)
                {
                    if (adj[current, next] == 0 || found[next] == true)
                    {
                        continue;
                    }

                    q.Enqueue(next);
                    found[next] = true;

                    parent[next] = current;
                    distance[next] = distance[current] + 1;

                }
            }
        
        }

        public void Dijikstra(int start)
        {
            bool[] visited = new bool[6];
            int[] distance = new int[6];
            int[] parent = new int[6];
            Array.Fill(distance, Int32.MaxValue);

            //※첫 루프라면, 시작점을 선택한다.
            distance[start] = 0;
            parent[start] = start;            

            while (true)
            {
                //가까이 있는 제일 좋은 후보를 찾는다.

                //가장 유력한 후보의 거리와 번호를 저장
                int closest = Int32.MaxValue;
                int now = -1;

                for (int i = 0; i < 6; i++)
                {
                    //방문했던 정점은 스킵함.
                    if (visited[i] == true)
                    {
                        continue;
                    }
                    // 아직 발견(예약)된 적이 없거나, 기존 후보보다 멀다면 스킵.
                    if (distance[i] == Int32.MaxValue || distance[i] >= closest)
                    {
                        continue;
                    }

                    //가장 좋은 후보로 정보 갱신함.
                    closest = distance[i];
                    now = i;

                }
            
                //-1이면 못찾음. 프로그램 끝.
                if(now  == -1)
                { 
                    break;
                }

                //방문했다고 알림.
                visited[now] = true;


                //인접 정점들을 조사하여 발견한 최단거리를 갱신함.
                for (int next = 0; next < 6; next++)
                {
                    if (adj[now, next] == -1)
                    {
                        continue;
                    }

                    if (visited[next] == true)
                    {
                        continue;
                    }

                    //자신이 여태까지 갔던 거리 + 인접 노드에 대한 거리
                    int nextDist = distance[now] + adj[now, next];
                    if (nextDist < distance[next])
                    {
                        distance[next] = nextDist;
                        parent[next] = now;
                    }
                }

            
            }


        
        }

    }

    class TreeNode<T> 
    {
        public T Data { get; set; }

        public List<TreeNode<T>> Children { get; set; } = new List<TreeNode<T>>();

    }


    class PriorityQueue<T> where T : IComparable<T>
    {
        List<T> Heap = new List<T>();

        public void Push(T data)
        {
            Heap.Add(data);


            int now = Heap.Count - 1;

            while (now > 0)
            {
                int parent = (now - 1) / 2;

                if (Heap[now].CompareTo(Heap[parent]) < 0)
                {
                    break;
                }

                T temp = Heap[now];
                Heap[now] = Heap[parent];
                Heap[parent] = temp;

                now = parent;
            }


        }


        public T Pop()
        {

            T ret = Heap[0];

            int lastIndex = Heap.Count - 1;

            Heap[0] = Heap[lastIndex];

            Heap.RemoveAt(lastIndex);

            lastIndex--;

            int now = 0;

            while (true)
            {
                int left = 2 * now + 1;
                int right = 2 * now + 2;

                int next = now;

                if (left <= lastIndex && Heap[next].CompareTo(Heap[left]) < 0)
                {
                    next = left;
                }
                if (right <= lastIndex && Heap[next].CompareTo(Heap[right]) < 0)
                {
                    next = right;
                }

                if (next == now)
                {
                    break;
                }

                T temp = Heap[now];
                Heap[now] = Heap[next];
                Heap[next] = temp;

                now = next;
            }


            return ret;        
        }


        public int Count()
        {
            return Heap.Count;
        }
    }

    class Knight :IComparable<Knight>
    {
    public int ID { get; set; }

        public int CompareTo(Knight other)
        {
            if (other.ID == this.ID)
            {
                return 0;
            }

            return this.ID > other.ID ? 1 : -1;
        }
    }

    class Program
    {
        static TreeNode<string> MakeTree()
        {
            TreeNode<string> root = new TreeNode<string>() { Data = "R1 개발실" };
            {
                TreeNode<string> node = new TreeNode<string> { Data = "디자인팀" };

                node.Children.Add(new TreeNode<string>() { Data = "전투" });
                node.Children.Add(new TreeNode<string>() { Data = "경제" });
                node.Children.Add(new TreeNode<string>() { Data = "스토리" });

                root.Children.Add(node);
            }
            {
                TreeNode<string> node = new TreeNode<string> { Data = "프로그래밍팀" };

                node.Children.Add(new TreeNode<string>() { Data = "서버" });
                node.Children.Add(new TreeNode<string>() { Data = "클라" });
                node.Children.Add(new TreeNode<string>() { Data = "엔진" });

                root.Children.Add(node);
            }
            {
                TreeNode<string> node = new TreeNode<string> { Data = "아트팀" };

                node.Children.Add(new TreeNode<string>() { Data = "배경" });
                node.Children.Add(new TreeNode<string>() { Data = "캐릭터" });

                root.Children.Add(node);
            }

            return root;
        }

        static void PrintTree(TreeNode<string> root)
        {
            Console.WriteLine(root.Data);
            foreach (TreeNode<string> child in root.Children)
            {
                PrintTree(child);
            }
        }

        static int GetHeight(TreeNode<string> root)
        {
            int height = 0;

            foreach (var child in root.Children)
            {
                int newHeight = GetHeight(child) + 1;
                height = Math.Max(newHeight, height);
            }

            return height;
        }





        static void Main(string[] args)
        {
            //Graph gra = new Graph();
            //gra.Dijikstra(0);

            //TreeNode<string> root = MakeTree();
            //PrintTree(root);
            //Console.WriteLine(GetHeight(root));

            PriorityQueue<int> q = new PriorityQueue<int>();
            q.Push(20);
            q.Push(10);
            q.Push(30);
            q.Push(90);
            q.Push(40);

            while(q.Count() > 0)
            {
                Console.WriteLine(q.Pop());

            }

        }
    }
}
