using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;

namespace Game
{
    public static class AStarPathfinding
    {
        public static NativeList<int2> FindPath(int2 startPos, int2 endPos, in NativeArray<int> walkableMap)
        {
            var openList = new NativeList<PathNode>(Allocator.Temp);
            var closeList = new NativeList<PathNode>(Allocator.Temp);
            var path = new NativeList<int2>(Allocator.Temp);
            NativeArray<int2> offset = new NativeArray<int2>(4, Allocator.Temp)
            {
                [0] = new int2(x: 0, y: 1),
                [1] = new int2(x: 1, y: 0),
                [2] = new int2(x: 0, y: -1),
                [3] = new int2(x: -1, y: 0)
            };


            var heuristicCost = HeuristicCost(startPos, endPos);
            openList.Add(new PathNode {pos = startPos, G = 0, H = heuristicCost, F = heuristicCost});


            bool found = false;
            while (openList.Length > 0)
            {
                int lowsestH = int.MaxValue;
                int currentNodeIndex = 0;

                for (int i = 0; i < openList.Length; i++)
                {
                    if (openList[i].H >= lowsestH) continue;
                    currentNodeIndex = i;
                    lowsestH = openList[i].H;
                }
                
                var currentNode = openList[currentNodeIndex];
                for (int i = 0; i < 4; i++)
                {
                    var neighbour = currentNode.pos + offset[i];
                    
                    var newNode = new PathNode
                    {
                        F = HeuristicCost(neighbour, endPos),
                        G = currentNode.G + 1,
                        priviousIndex = closeList.Length,
                        pos = neighbour
                    };
                    newNode.CalculateH();
                    

                    if (LegalCoord(neighbour) && (walkableMap[neighbour.x*100 + neighbour.y] == 1) && (!closeList.Contains(newNode)))
                    {

                        
                        openList.Add(newNode);
                        if (neighbour.Equals(endPos))
                        {
                            found = true;
                        }
                    }
                }
                closeList.Add(currentNode);
                openList.RemoveAtSwapBack(currentNodeIndex);

                if (found)
                {
                    var tmp = currentNode;
                    break;
                }
            }

            if (found)
            {
                var privousIndex = closeList.Length - 1;
                while (privousIndex != 0)
                {
                    path.Add(closeList[privousIndex].pos);
                    privousIndex = closeList[privousIndex].priviousIndex;
                }
            }

            openList.Dispose();
            closeList.Dispose();
            offset.Dispose();
            return path;
        }

        private static bool LegalCoord(int2 coord)
        {
            if ((coord.y >= 100) || (coord.y < 0))
            {
                return false;
            }
            
            if (coord.x >= 100 || (coord.x < 0))
            {
                return false;
            }

            return true;
        }
        
        private struct PathNode : IEqualityComparer<PathNode>, IEquatable<PathNode>
        {
            public int2 pos;
            
            public int G;
            public int H;
            public int F;
            
            public int priviousIndex;

            public void CalculateH()
            {
                H = F + G;
            }
            
            public bool Equals(PathNode x, PathNode y)
            {
                return x.pos.Equals(y.pos);
            }

            public int GetHashCode(PathNode obj)
            {
                return pos.GetHashCode();
            }

            public bool Equals(PathNode other)
            {
                return pos.Equals(other.pos);
            }

            public override bool Equals(object obj)
            {
                return obj is PathNode other && Equals(other);
            }

            public override int GetHashCode()
            {
                return pos.GetHashCode();
            }
        }
        
        private static int HeuristicCost(int2 current, int2 end)
        {
            var vector = end - current;
            return math.abs(vector.x) + math.abs(vector.y);
        }
    }
}