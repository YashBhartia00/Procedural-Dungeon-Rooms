using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
namespace pathFinding
{
public class DijkstrasAlgorithm 
{  
  
    public  readonly int NO_PARENT = -1;  
    public  void dijkstra(int[,] adjacencyMatrix,  int startVertex)  
    {  
        int nVertices = adjacencyMatrix.GetLength(0);  
        int[] shortestDistances = new int[nVertices];  
        bool[] added = new bool[nVertices];  
        for (int vertexIndex = 0; vertexIndex < nVertices;  vertexIndex++)  
        {  
            shortestDistances[vertexIndex] = int.MaxValue;  
            added[vertexIndex] = false;  
        }  
        shortestDistances[startVertex] = 0;  
        int[] parents = new int[nVertices];  
        parents[startVertex] = NO_PARENT;  
        for (int i = 1; i < nVertices; i++)  
        {  
  
            int nearestVertex = -1;  
            int shortestDistance = int.MaxValue;  
            for (int vertexIndex = 0;  
                    vertexIndex < nVertices;  
                    vertexIndex++)  
            {  
                if (!added[vertexIndex] &&  
                    shortestDistances[vertexIndex] <  
                    shortestDistance)  
                {  
                    nearestVertex = vertexIndex;  
                    shortestDistance = shortestDistances[vertexIndex];  
                }  
            }  
  
            added[nearestVertex] = true;  
  
            for (int vertexIndex = 0;  
                    vertexIndex < nVertices;  
                    vertexIndex++)  
            {  
                int edgeDistance = adjacencyMatrix[nearestVertex,vertexIndex];  
                  
                if (edgeDistance > 0 
                    && ((shortestDistance + edgeDistance) <  
                        shortestDistances[vertexIndex]))  
                {  
                    parents[vertexIndex] = nearestVertex;  
                    shortestDistances[vertexIndex] = shortestDistance +  
                                                    edgeDistance;  
                }  
            }  
        }  
  
        printSolution(startVertex, shortestDistances, parents);  
        // return new int[10][];
    }  
  
    public  void printSolution(int startVertex,  int[] distances,  int[] parents)  
    {  
        int nVertices = distances.Length;  
        // Console.Write("Vertex\t Distance\tPath");  
          
        for (int vertexIndex = 0;  vertexIndex < nVertices;  vertexIndex++)  
        {  
            if (vertexIndex != startVertex)  
            {  
                Debug.Log("\n" + startVertex + " -> ");  
                Debug.Log(vertexIndex + " \t\t ");  
                Debug.Log(distances[vertexIndex] + "\t\t");  
                printPath(vertexIndex, parents);  
            }  
        }  
    }  
  
    public  void printPath(int currentVertex,  int[] parents)  
    {  
          
        if (currentVertex == NO_PARENT)  
        {  
            return;  
        }  
        printPath(parents[currentVertex], parents);  
        Debug.Log(currentVertex + " ");  
    }  
  
    // Driver Code  
    // public  void Main(String[] args)  
    // {  
    //     int[,] adjacencyMatrix = { { 0, 4, 0, 0, 0, 0, 0, 8, 0 },  
    //                                 { 4, 0, 8, 0, 0, 0, 0, 11, 0 },  
    //                                 { 0, 8, 0, 7, 0, 4, 0, 0, 2 },  
    //                                 { 0, 0, 7, 0, 9, 14, 0, 0, 0 },  
    //                                 { 0, 0, 0, 9, 0, 10, 0, 0, 0 },  
    //                                 { 0, 0, 4, 0, 10, 0, 2, 0, 0 },  
    //                                 { 0, 0, 0, 14, 0, 2, 0, 1, 6 },  
    //                                 { 8, 11, 0, 0, 0, 0, 1, 0, 7 },  
    //                                 { 0, 0, 2, 0, 0, 0, 6, 7, 0 } };  
    //     dijkstra(adjacencyMatrix, 0);  
    // }  
}  


  
public class Graph  
{  
    public List<string> numbersOnly = new List<string>();
    public int v;  
    public List<int>[] adjList;  
    public Graph(int vertices) 
    {  
        this.v = vertices;  
        initAdjList();  
    }  
    public void initAdjList()  
    {  
        adjList = new List<int>[v];  
        for(int i = 0; i < v; i++)  
        {  
            adjList[i] = new List<int>();  
        }  
    }  
    public void addEdge(int u, int v)  
    {  
        adjList[u].Add(v);  
    }  
    public void printAllPaths(int s, int d)  
    {  
        bool[] isVisited = new bool[v];  
        List<int> pathList = new List<int>();  
          
        pathList.Add(s);  
          
        printAllPathsUtil(s, d, isVisited, pathList);  
    }  
  
    public void printAllPathsUtil(int u, int d,  
                                    bool[] isVisited,  
                            List<int> localPathList)  
    {  
          
        isVisited[u] = true;  
          
        if (u.Equals(d))  
        {  
            isVisited[u] = false;  
            // Debug.Log(string.Join(" ", localPathList));
            numbersOnly.Add(string.Join(" ",localPathList).ToString());
            // return localPathList;
        }  
          
        foreach (int i in adjList[u])  
        {  
            if (!isVisited[i])  
            {  
                localPathList.Add(i);  
                printAllPathsUtil(i, d, isVisited,  
                                    localPathList);  
                  
                localPathList.Remove(i);  
            }  
        }  
          
        isVisited[u] = false;  
        // return new List<int>{2,9};
    }  
  
    // public static void Main(String[] args)  
    // {  
    //     Graph g = new Graph(4);  
    //     g.addEdge(0,1);  
    //     g.addEdge(0,2);  
    //     g.addEdge(0,3);  
    //     g.addEdge(2,0);  
    //     g.addEdge(2,1);  
    //     g.addEdge(1,3);  
      
    //     int s = 2;  
    //     int d = 3;  
      
    //     Console.WriteLine("Following are all different" +  
    //                         " paths from " + s + " to " + d);  
    //     g.printAllPaths(s, d);  
    // }  
}  



}
