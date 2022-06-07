using System;
using System.Collections.Generic;

namespace BinaryStrom
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Binarni strom!");
            BinStrom<int> muj_strom = new BinStrom<int>();
            muj_strom.Add(25);
            muj_strom.Add(3);
            muj_strom.Add(10);
            muj_strom.Add(7);
            muj_strom.Add(4);
            muj_strom.Add(5);
            muj_strom.Add(1);
            muj_strom.VypisObsah();
            Console.WriteLine("Vyska: " + muj_strom.Vyska());
            muj_strom.Print();
            muj_strom.SmazatUzel(3);
            muj_strom.VypisObsah();
            Console.WriteLine("Vyska: " + muj_strom.Vyska());
            muj_strom.Print();
        }
    }

    class BinStrom<T> where T : IComparable
    {
        Node koren;
        class Node
        {
            public T hodn;
            public Node levy, pravy;
            public Node(T vstup)
            {
                hodn = vstup;
            }
        }

        class NodeInfo
        {
            public Node Node;
            public string Text;
            public int StartPos;
            public int Size { get { return Text.Length; } }
            public int EndPos { get { return StartPos + Size; } set { StartPos = value - Size; } }
            public NodeInfo Parent, Left, Right;
        }

        public void Print()
        {
            Print(koren);
        }

        public int Vyska()
        {
            if (koren == null)
            {
                return 0;
            }
            return VyskaRekur(koren) - 1;
        }

        private int VyskaRekur(Node koren)
        {
            if (koren == null)
            {
                return 0;
            }
            int levy = VyskaRekur(koren.levy) + 1;
            int pravy = VyskaRekur(koren.pravy) + 1;
            return levy > pravy ? levy : pravy;
        }

        public void Add(T vstup)
        {
            if (koren == null)
            {
                koren = new Node(vstup);
                return;
            }
            AddRekur(koren, vstup);
        }

        private void AddRekur(Node koren, T vstup)
        {
            if (vstup.CompareTo(koren.hodn) <= 0)
            {
                if (koren.levy != null)
                {
                    AddRekur(koren.levy, vstup);
                }
                else
                {
                    koren.levy = new Node(vstup);
                }
            }
            else
            {
                if (koren.pravy != null)
                {
                    AddRekur(koren.pravy, vstup);
                }
                else
                {
                    koren.pravy = new Node(vstup);
                }
            }
        }
        public void VypisObsah()
        {
            if (koren != null)
            {
                VypisObsahRekur2(koren);
            }
        }

        private void VypisObsahRekur2(Node koren)
        {
            Console.WriteLine(koren.hodn);
            if (koren.levy != null)
            {
                VypisObsahRekur(koren.levy);
            }
            if (koren.pravy != null)
            {
                VypisObsahRekur(koren.pravy);
            }
        }

        private void VypisObsahRekur(Node koren)
        {
            if (koren.levy != null)
            {
                VypisObsahRekur(koren.levy);
            }
            Console.WriteLine(koren.hodn);
            if (koren.pravy != null)
            {
                VypisObsahRekur(koren.pravy);
            }
        }

        public void SmazatUzel(T vstup)
        {
            Node node = new Node(vstup);
            SmazatUzelRekur(koren, node);
        }

        private Node SmazatUzelRekur(Node koren, Node node)
        {
            if (koren == null)
            {
                return koren;
            }

            if (node.hodn.CompareTo(koren.hodn) < 0)
            {
                koren.levy = SmazatUzelRekur(koren.levy, node);
            }
            else if (node.hodn.CompareTo(koren.hodn) > 0)
            {
                koren.pravy = SmazatUzelRekur(koren.pravy, node);
            }

            else
            {
                if (koren.levy == null)
                {
                    return koren.pravy;
                }
                else if (koren.pravy == null)
                {
                    return koren.levy;
                }
                koren.hodn = NejmensiHodn(koren.pravy);

                Node temp = new Node(koren.hodn);
                koren.pravy = SmazatUzelRekur(koren.pravy, temp);
            }
            return koren;
        }

        private T NejmensiHodn(Node koren)
        {
            T min = koren.hodn;
            while (koren.levy != null)
            {
                min = koren.levy.hodn;
                koren = koren.levy;
            }
            return min;
        }

        //Vykresleni
        private void Print(Node root, int topMargin = 2, int leftMargin = 2)
        {
            if (root == null) return;
            int rootTop = Console.CursorTop + topMargin;
            var last = new List<NodeInfo>();
            var next = root;
            for (int level = 0; next != null; level++)
            {
                var item = new NodeInfo { Node = next, Text = next.hodn.ToString() };
                if (level < last.Count)
                {
                    item.StartPos = last[level].EndPos + 1;
                    last[level] = item;
                }
                else
                {
                    item.StartPos = leftMargin;
                    last.Add(item);
                }
                if (level > 0)
                {
                    item.Parent = last[level - 1];
                    if (next == item.Parent.Node.levy)
                    {
                        item.Parent.Left = item;
                        item.EndPos = Math.Max(item.EndPos, item.Parent.StartPos);
                    }
                    else
                    {
                        item.Parent.Right = item;
                        item.StartPos = Math.Max(item.StartPos, item.Parent.EndPos);
                    }
                }
                next = next.levy ?? next.pravy;
                for (; next == null; item = item.Parent)
                {
                    Print(item, rootTop + 2 * level);
                    if (--level < 0) break;
                    if (item == item.Parent.Left)
                    {
                        item.Parent.StartPos = item.EndPos;
                        next = item.Parent.Node.pravy;
                    }
                    else
                    {
                        if (item.Parent.Left == null)
                            item.Parent.EndPos = item.StartPos;
                        else
                            item.Parent.StartPos += (item.StartPos - item.Parent.EndPos) / 2;
                    }
                }
            }
            Console.SetCursorPosition(0, rootTop + 2 * last.Count - 1);
        }

        private static void Print(NodeInfo item, int top)
        {
            SwapColors();
            Print(item.Text, top, item.StartPos);
            SwapColors();
            if (item.Left != null)
                PrintLink(top + 1, "┌", "┘", item.Left.StartPos + item.Left.Size / 2, item.StartPos);
            if (item.Right != null)
                PrintLink(top + 1, "└", "┐", item.EndPos - 1, item.Right.StartPos + item.Right.Size / 2);
        }

        private static void PrintLink(int top, string start, string end, int startPos, int endPos)
        {
            Print(start, top, startPos);
            Print("─", top, startPos + 1, endPos);
            Print(end, top, endPos);
        }

        private static void Print(string s, int top, int left, int right = -1)
        {
            Console.SetCursorPosition(left, top);
            if (right < 0) right = left + s.Length;
            while (Console.CursorLeft < right) Console.Write(s);
        }

        private static void SwapColors()
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = Console.BackgroundColor;
            Console.BackgroundColor = color;
        }
    }
}
