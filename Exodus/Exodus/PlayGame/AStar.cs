using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Exodus.PlayGame
{

    public static class AStar
    {
        class CellInfos : IComparable<CellInfos>
        {
            public Point point;
            public int currentCost;
            public int heuristic;
            public CellInfos parent = null;
            public CellInfos(int x, int y, int currentCost, int heuristic)
            {
                this.point = new Point(x, y);
                this.currentCost = currentCost;
                this.heuristic = heuristic;
            }
            public int CompareTo(CellInfos c)
            {
                // On trie on trie en fonction des coordonnées x.
                // Si x équivalents en fonction des coordonnées y.
                int i = this.point.X - c.point.X;
                if (i == 0)
                    return this.point.Y - c.point.Y;
                return i;
            }
        }
        public static LinkedList<Point> Pathfind(Point Start, Point Arrival)
        {
            return Pathfind(Start, Arrival, ValidCase);
        }
        public static LinkedList<Point> Pathfind(Point Start, Point Arrival, Func<int,int, bool> IsValidCase)
        {
            // Si la case n'est pas valable ou est un obstacle
            Arrival = closestFreePoint(x => IsValidCase(x.X, x.Y), Arrival, Start);
            if (Arrival.X == -1 && Arrival.Y == -1 || !IsValidCase(Arrival.X, Arrival.Y))
                return null;
            BinaryHeap<CellInfos> openSet = new BinaryHeap<CellInfos>(
                (x, y) => x.currentCost + x.heuristic < y.currentCost + y.heuristic,
                (x, y) => x.Equals(y));
            Dictionary<Point, CellInfos> openSet2 = new Dictionary<Point, CellInfos>();
            Dictionary<Point, CellInfos> closeSet = new Dictionary<Point, CellInfos>();
            openSet.Add(new CellInfos(Start.X, Start.Y, 0, Heuristic(Start, Arrival)));
            openSet2[openSet.First().point] = openSet.First();
            CellInfos current, temp, temp2;
            List<Point> neighbors;
            while (openSet2.Count > 0 && openSet2.Count < 300)
            {
                current = openSet.First();
                if (current.point.Equals(Arrival))
                    return ReconstructPath(current);
                openSet2.Remove(current.point);
                openSet.Remove(current);
                closeSet[current.point] = current;
                neighbors = GetFreeNeighbors(current.point);
                foreach (Point n in neighbors)
                {
                    temp = new CellInfos(n.X, n.Y, 0, 0);
                    if (!closeSet.ContainsKey(temp.point))
                    {
                        temp2 = null;
                        temp.currentCost = current.currentCost + Heuristic(temp.point, current.point);
                        temp.heuristic = Heuristic(temp.point, Arrival);
                        if (!openSet2.TryGetValue(temp.point, out temp2) || temp.currentCost + temp.heuristic < temp2.currentCost + temp2.heuristic)
                        {
                            temp.parent = current;
                            if (temp2 == null)
                            {
                                openSet.Add(temp);
                                openSet2[temp.point] = temp;
                            }
                        }
                    }
                }
            }
            return null;
        }
        static LinkedList<Point> ReconstructPath(CellInfos current)
        {
            LinkedList<Point> l = new LinkedList<Point>();
            while (current != null)
            {
                l.AddFirst(current.point);
                current = current.parent;
            }
            return l;
        }
        public static int Heuristic(Point p1, Point p2)
        {
            int dX = Math.Abs(p2.X - p1.X);
            int dY = Math.Abs(p2.Y - p1.Y);
            int diagonal = Math.Min(dX, dY);
            int straight = dX + dY - 2 * diagonal;
            return 14 * diagonal + 10 * straight;
        }
        public static List<Point> GetAllNeighbors(Point p)
        {
            List<Point> l = new List<Point>();
            if (p.X > 0)
            {
                if (p.Y > 0)
                    l.Add(new Point(p.X - 1, p.Y - 1));
                if (p.Y < Map.Height - 1)
                    l.Add(new Point(p.X - 1, p.Y + 1));
            }
            if (p.X < Map.Width - 1)
            {
                if (p.Y > 0)
                    l.Add(new Point(p.X + 1, p.Y - 1));
                if (p.Y < Map.Width - 1)
                    l.Add(new Point(p.X + 1, p.Y + 1));
            }
            return l;
        }
        public static List<Point> GetFreeNeighbors(Point p)
        {
            List<Point> result = new List<Point>();
            if (ValidCase(p.X, p.Y + 1))
            {
                result.Add(new Point(p.X, p.Y + 1));
                if (ValidCase(p.X + 1, p.Y) && ValidCase(p.X + 1, p.Y + 1))
                    result.Add(new Point(p.X + 1, p.Y + 1));
                if (ValidCase(p.X - 1, p.Y) && ValidCase(p.X - 1, p.Y + 1))
                    result.Add(new Point(p.X - 1, p.Y + 1));
            }
            if (ValidCase(p.X, p.Y - 1))
            {
                result.Add(new Point(p.X, p.Y - 1));
                if (ValidCase(p.X + 1, p.Y) && ValidCase(p.X + 1, p.Y - 1))
                    result.Add(new Point(p.X + 1, p.Y - 1));
                if (ValidCase(p.X - 1, p.Y) && ValidCase(p.X - 1, p.Y - 1))
                    result.Add(new Point(p.X - 1, p.Y - 1));
            }
            if (ValidCase(p.X + 1, p.Y))
                result.Add(new Point(p.X + 1, p.Y));
            if (ValidCase(p.X - 1, p.Y))
                result.Add(new Point(p.X - 1, p.Y));
            return result;
        }
        public static bool ValidCase(int x, int y)
        {
            return (x >= 0 && x < Map.Width && y >= 0 && y < Map.Height && Map.MapCells[x, y].ListItems.Count == 0);
        }

        /// <summary>
        /// Retourne la case libre la plus proche de p
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Point closestFreePoint(Func<Point, bool> ValidPoint, Point p, Point start)
        {
            Point min = new Point(-1, -1);
            if (ValidPoint(p))
                return p;
            else
            {
                BinaryHeap<Point> openSet = new BinaryHeap<Point>((p1, p2) => Heuristic(p1, p) < Heuristic(p2, p), (p1, p2) => p1.Equals(p2));
                SortedSet<CellInfos> openSet2 = new SortedSet<CellInfos>();
                SortedSet<CellInfos> closedSet = new SortedSet<CellInfos>();
                openSet.Add(p);
                openSet2.Add(new CellInfos(p.X, p.Y, 0, 0));
                Point current;
                List<Point> neighbors;
                while (openSet.Any())
                {
                    current = openSet.First();
                    if (ValidPoint(current))
                    {
                        if (min.X == -1 && min.Y == -1)
                            min = current;
                        else
                        {
                            int d = Heuristic(start, current) - Heuristic(start, min);
                            if (d < 0 && Heuristic(p, current) <= Heuristic(min, current))
                                min = current;
                            else if (d > 0)
                                return min;
                        }
                    }
                    closedSet.Add(new CellInfos(current.X, current.Y, 0, 0));
                    openSet.Remove(current);
                    openSet2.Remove(new CellInfos(current.X, current.Y, 0, 0));
                    neighbors = new List<Point>();
                    foreach (Point tmp in new List<Point>() {
                        new Point(current.X, current.Y - 1),
                        new Point(current.X, current.Y + 1),
                        new Point(current.X - 1, current.Y),
                        new Point(current.X + 1, current.Y)
                    })
                    {
                        if (tmp.X >= 0 && tmp.X < Map.Width && p.Y >= 0 && p.Y < Map.Height)
                            neighbors.Add(tmp);
                    }
                    foreach (Point n in neighbors)
                    {
                        if (!closedSet.Contains(new CellInfos(n.X, n.Y, 0, 0)) && !openSet2.Contains(new CellInfos(n.X, n.Y, 0, 0)))
                        {
                            openSet.Add(n);
                            openSet2.Add(new CellInfos(n.X, n.Y, 0, 0));
                        }
                    }
                }
                return new Point(-1, -1);
            }
        }
    }
}
