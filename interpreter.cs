using System;
using System.Collections.Generic;

namespace MiniPl
{

    class Element
    {

        public string value { get; set; }
        public string type { get; set; }


        public Element(string value_, string type_)
        {

            value = value_;
            type = type_;
        }
    }
    class Interpreter
    {

        static private Ast ast;

        Dictionary<string, Element> variables;

        public Interpreter(Ast ast_)
        {
            ast = ast_;
        }

        public void start()
        {
            variables = new Dictionary<string, Element>();
            foreach (Node node in ast.root.childs)
            {
                statement(node);

            }
        }

        public void statement(Node node)
        {
            switch (node.token.type)
            {
                case TokenType.VAR:
                    defineVariable(node);
                    return;
                case TokenType.IDENTIFIER:
                    return;
                case TokenType.PRINT:
                    print(node);
                    return;
                case TokenType.READ:
                    return;
                case TokenType.ASSERT:
                    return;
                case TokenType.FOR:
                    return;
            }
        }

        public void defineVariable(Node node)
        {
            Node iden = node.childs[0];
            Node type = iden.childs[0];
            Node value = type.childs[0];
            if (!variables.ContainsKey(iden.token.value))
            {
                if (matchingVariableType(type.token.type, value.token.type))
                {
                    variables.Add(iden.token.value, new Element(value.token.value, type.token.value));
                }
                else
                {
                    Error e = new Error("SEMANTIC ERROR", 0);
                    Console.WriteLine(e);
                }
            }
            else
            {
                Error e = new Error("SEMANTIC ERROR", 0);
                Console.WriteLine(e);
            }
        }

        public bool matchingVariableType(TokenType type, TokenType value)
        {
            if (type == TokenType.STRINGTYPE & value == TokenType.STRING)
            {
                return true;
            }
            if (type == TokenType.INTTYPE & value == TokenType.INT)
            {
                return true;
            }
            return false;
        }

        public void editVariable(Node node)
        {

        }

        public void print(Node node)
        {
            Node printable = node.childs[0];
            if (printable.token.type == TokenType.IDENTIFIER)
            {
                if (variables.ContainsKey(printable.token.value))
                {
                    Console.WriteLine(variables[printable.token.value].value);
                }
                else
                {
                    Error e = new Error("SEMANTIC ERROR", 0);
                    Console.WriteLine(e);
                }
            }

            else
            {
                Console.WriteLine(printable.token.value);
            }
        }

        public void read(Node node)
        {

        }


        public void assert(Node node)
        {

        }

        public void forLoop(Node node)
        {

        }

    }
}