using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

public class Graph
{
    private Dictionary<string, List<string>> _adjacencyList;

    public Graph(Dictionary<string, List<string>> adjacencyList)
    {
        _adjacencyList = adjacencyList;
    }

    public void DFS(string startVertex)
    {
        HashSet<string> visited = new HashSet<string>();
        DFSUtil(startVertex, visited);
    }

    private void DFSUtil(string vertex, HashSet<string> visited)
    {
        visited.Add(vertex);
        Console.Write(vertex + " ");

        if (_adjacencyList.ContainsKey(vertex))
        {
            foreach (string neighbor in _adjacencyList[vertex])
            {
                if (!visited.Contains(neighbor))
                {
                    DFSUtil(neighbor, visited);
                }
            }
        }
    }

    public List<string> BuildSpanningTree(string startVertex)
    {
        List<string> spanningTree = new List<string>();
        HashSet<string> visited = new HashSet<string>();

        void DFSUtil(string vertex)
        {
            visited.Add(vertex);
            spanningTree.Add(vertex);

            if (_adjacencyList.ContainsKey(vertex))
            {
                foreach (string neighbor in _adjacencyList[vertex])
                {
                    if (!visited.Contains(neighbor))
                    {
                        DFSUtil(neighbor);
                    }
                }
            }
        }

        DFSUtil(startVertex);
        return spanningTree;
    }

    public void PrintGraph()
    {
        foreach (var vertex in _adjacencyList)
        {
            Console.Write(vertex.Key + ": ");
            foreach (var neighbor in vertex.Value)
            {
                Console.Write(neighbor + " ");
            }
            Console.WriteLine();
        }
    }

    public static Graph GetSampleGraph1() => new Graph(new Dictionary<string, List<string>>
    {
        {"1", new List<string>() {"3", "5", "6", "9"}},
        {"2", new List<string>() {"4", "6", "8"}},
        {"3", new List<string>() {"1", "5", "6", "7", "8"}},
        {"4", new List<string>() {"2", "10"}},
        {"5", new List<string>() {"1", "3", "7"}},
        {"6", new List<string>() {"1", "2", "3"}},
        {"7", new List<string>() {"3", "5"}},
        {"8", new List<string>() {"2", "3"}},
        {"9", new List<string>() {"1", "11", "12"}},
        {"10", new List<string>() {"4"}},
        {"11", new List<string>() {"9", "12"}},
        {"12", new List<string>() {"9", "11"}},
    });

    public static Graph GetSampleGraph2() => new Graph(new Dictionary<string, List<string>>
    {
        {"Б", new List<string>() {"Д", "К", "М"}},
        {"Д", new List<string>() {"Б", "К"}},
        {"К", new List<string>() {"Б", "Д", "Р"}},
        {"М", new List<string>() {"Б"}},
        {"Р", new List<string>() {"К"}},
    });

    public static Graph GetSampleGraph3() => new Graph(new Dictionary<string, List<string>>
    {
        {"1", new List<string>() {"4"}},
        {"2", new List<string>() {"4"}},
        {"3", new List<string>() {"4"}},
        {"4", new List<string>() {"1", "2", "3", "5"}},
        {"5", new List<string>() {"4", "6"}},
        {"6", new List<string>() {"5"}},
    });
}


public class Sudoku
{
    private int[][] _field;

    public Sudoku(int[][] field)
    {
        _field = field;
    }

    public void Solve()
    {
        Backtracking();
    }

    private bool Backtracking()
    {
        if (IsSolutionFound())
        {
            return true; // Решение найдено
        }

        foreach (var cell in GetFreeCells())
        {
            for (int number = 1; number <= 9; number++)
            {
                if (CheckMoveForFairness(cell, number))
                {
                    _field[cell.Item1][cell.Item2] = number;
                    if (Backtracking())
                    {
                        return true; // Решение найдено
                    }
                    _field[cell.Item1][cell.Item2] = 0; // Стираем вариант с поля
                }
            }
        }

        return false; // Решение не найдено
    }

    bool IsSolutionFound() => _field.All(rows => rows.All(cell => cell != 0));
    IEnumerable<(int, int)> GetFreeCells() => _field.SelectMany((row, i) => row.Select((col, j) => (i, j))).Where(cell => _field[cell.Item1][cell.Item2] == 0);

    bool CheckMoveForFairness((int, int) cell, int number)
    {
        if (number < 1 || number > 9)
            return false;

        if (_field[cell.Item1][cell.Item2] != 0)
            return false;
        for (int k = 0; k < 9; k++)
        {
            if (_field[k][cell.Item2] == number || _field[cell.Item1][k] == number)
                return false;
        }
        var (cornerX, cornerY) = ((cell.Item1 / 3) * 3, (cell.Item2 / 3) * 3);
        for (int i = cornerX; i < cornerX + 3; i++)
        {
            for (int j = cornerY; j < cornerY + 3; j++)
            {
                if (_field[i][j] == number)
                    return false;
            }
        }

        return true;
    }

    public void PrintField(bool shouldClearConsole = false, int delayInMillis = 0)
    {
        Console.OutputEncoding = Encoding.UTF8;
        if (delayInMillis > 0)
            Thread.Sleep(delayInMillis);
        if (shouldClearConsole)
            Console.Clear();

        Console.WriteLine("┏━━━┯━━━┯━━━┳━━━┯━━━┯━━━┳━━━┯━━━┯━━━┓");
        for (int i = 0; i < 9; i++)
        {
            Console.Write("┃");
            for (int j = 0; j < 9; j++)
            {
                Console.Write((_field[i][j] == 0 ? $"   " : $" {_field[i][j]} ") + ((j - 2) % 3 == 0 ? "┃" : "│"));
            }

            if (i < 8)
            {
                Console.WriteLine((i - 2) % 3 == 0
                    ? "\n┣━━━┿━━━┿━━━╋━━━┿━━━┿━━━╋━━━┿━━━┿━━━┫"
                    : "\n┠───┼───┼───╂───┼───┼───╂───┼───┼───┨");
            }
        }

        Console.WriteLine("\n┗━━━┷━━━┷━━━┻━━━┷━━━┷━━━┻━━━┷━━━┷━━━┛");
    }

    public static Sudoku GetSampleSudoku3() => new Sudoku(new int[][]
    {
        new int[] { 0, 1, 6, 0, 0, 8, 0, 9, 0 },
        new int[] { 5, 2, 0, 0, 3, 4, 7, 6, 8 },
        new int[] { 4, 8, 0, 6, 2, 0, 5, 0, 1 },
        new int[] { 0, 6, 3, 0, 1, 5, 0, 8, 0 },
        new int[] { 0, 0, 4, 8, 6, 0, 1, 2, 5 },
        new int[] { 8, 0, 1, 7, 9, 2, 6, 4, 3 },
        new int[] { 1, 3, 8, 0, 4, 7, 2, 0, 6 },
        new int[] { 0, 9, 0, 3, 5, 1, 0, 7, 4 },
        new int[] { 0, 4, 5, 0, 0, 6, 3, 1, 0 }
    });
    public static Sudoku GetSampleSudoku1() => new Sudoku(new int[][]
    {
        new int[] { 0, 1, 6, 5, 7, 8, 0, 9, 2 },
        new int[] { 5, 2, 9, 0, 3, 4, 7, 6, 8 },
        new int[] { 4, 8, 7, 6, 2, 0, 5, 0, 1 },
        new int[] { 2, 6, 3, 0, 1, 5, 9, 8, 7 },
        new int[] { 0, 7, 4, 8, 6, 0, 1, 2, 5 },
        new int[] { 8, 5, 1, 7, 9, 2, 6, 4, 3 },
        new int[] { 1, 3, 8, 0, 4, 7, 2, 0, 6 },
        new int[] { 6, 9, 0, 3, 5, 1, 8, 7, 4 },
        new int[] { 7, 4, 5, 0, 0, 6, 3, 1, 0 }
    });
    public static Sudoku GetSampleSudoku2() => new Sudoku(new int[][]
    {
        new int[] { 0, 1, 6, 0, 0, 8, 0, 9, 0 },
        new int[] { 5, 2, 0, 0, 3, 4, 7, 6, 8 },
        new int[] { 4, 8, 0, 6, 2, 0, 5, 0, 1 },
        new int[] { 0, 6, 3, 0, 1, 5, 9, 8, 0 },
        new int[] { 0, 0, 4, 8, 6, 0, 1, 2, 5 },
        new int[] { 8, 0, 1, 7, 9, 2, 6, 4, 3 },
        new int[] { 1, 3, 8, 0, 4, 7, 2, 0, 6 },
        new int[] { 6, 9, 0, 3, 5, 1, 0, 7, 4 },
        new int[] { 0, 4, 5, 0, 0, 6, 3, 1, 0 }
    });
}

 
public class Program
{
    public static void Main()
    {
        Graph graph = Graph.GetSampleGraph1();
        graph.PrintGraph();

        Console.WriteLine("Depth First Traversal starting from vertex 2:");
        graph.DFS("2");

        List<string> spanningTree = graph.BuildSpanningTree("2");
        Console.WriteLine("Spanning Tree starting from vertex 2: " + string.Join(" ", spanningTree));

       Thread.Sleep(5000);

        Sudoku sudoku = Sudoku.GetSampleSudoku1();
        sudoku.Solve();
        sudoku.PrintField(true, 1000); // Печать решенного судоку с задержкой 1 секунда между ходами
    }

}