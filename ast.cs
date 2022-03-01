using System;
using System.Collections.Generic;

namespace MiniPl
{

    class Node
    {
        public List<Node> childs { get; set; }
        public TokenType type { get; set; }
        public string data { get; set; }

        public Node(TokenType type_, string data_)
        {
            childs = new List<Node>();
            type = type_;
            data = data_;
        }

        public Node()
        {
            childs = new List<Node>();
        }
    }

    class Ast
    {
        public Node root { get; set; }

        public Node add(TokenType t, string s, Node parent)
        {
            Node node = new Node(t, s);
            parent.childs.Add(node);
            return node;
        }

        public void printChilds(Node node)
        {
            Console.WriteLine("node: " + node.data);
            foreach (Node n in node.childs)
            {
                Console.WriteLine(n.data);
            }
            foreach (Node n in node.childs)
            {
                printChilds(n);
            }
        }


        public void traverse(Node parent)
        {
            if (parent != null)
            {
                foreach (Node node in parent.childs)
                {
                    traverse(node);
                }
                Console.WriteLine(parent.data);
            }
        }
    }
}