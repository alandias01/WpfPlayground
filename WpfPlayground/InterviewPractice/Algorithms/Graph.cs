using System;
using System.Collections.Generic;
using System.Text;

namespace WpfPlayground.InterviewPractice.Algorithms
{
    public class Graph
    {
        public Graph()
        {

        }

        private List<Vertex> Vertices = new List<Vertex>();
        private int[,] AdjMatrix;
        private int INFINITY = 1000000;

        public Graph(int width, int height)
        {
            AdjMatrix = new int[width, height];

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    AdjMatrix[i, j] = INFINITY; //replaces 0

        }

        public void AddVertex(string label)
        {
            Vertices.Add(new Vertex(label));
        }

        public void AddEdge(int start, int end, int weight)
        {
            AdjMatrix[start, end] = weight;
        }
    }
    public class Vertex
    {
        public bool IsInTree { get; set; }
        public string Label { get; set; }

        public Vertex(string label)
        {
            this.Label = label;
            IsInTree = false;
        }
    }

    public class DistPar
    {
        public int Distance { get; set; }  //Distance from start to this vertex
        public int ParentVert { get; set; } //current parent of this vertex

        public DistPar(int distance, int parentVert)
        {
            Distance = distance;
            ParentVert = parentVert;
        }
    }


}
