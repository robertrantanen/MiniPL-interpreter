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
                    editVariable(node);
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
                switch (type.token.type)
                {
                    case TokenType.STRINGTYPE:
                        if (value.token.type == TokenType.STRING)
                        {
                            variables.Add(iden.token.value, new Element(value.token.value, "string"));
                        }
                        else if (value.token.type == TokenType.PLUS)
                        {

                        }
                        else
                        {
                            Error e = new Error("SEMANTIC ERROR: excepted string", node.token.line);
                            Console.WriteLine(e);
                        }
                        return;
                    case TokenType.INTTYPE:
                        if (value.token.type == TokenType.INT)
                        {
                            variables.Add(iden.token.value, new Element(value.token.value, "int"));
                        }
                        else if (isArithmeticOperation(value.token.type))
                        {
                            variables.Add(iden.token.value, new Element(arithmetic(value).ToString(), "int"));
                        }
                        else
                        {
                            Error e = new Error("SEMANTIC ERROR: excepted int", node.token.line);
                            Console.WriteLine(e);
                        }
                        return;
                    case TokenType.BOOLTYPE:
                        return;
                }
            }
            else
            {
                Error e = new Error("SEMANTIC ERROR: variable already defined", node.token.line);
                Console.WriteLine(e);
            }
        }

        public void editVariable(Node node)
        {
            if (variables.ContainsKey(node.token.value))
            {
                Element element = variables[node.token.value];
                Node value = node.childs[0];
                switch (element.type)
                {
                    case "string":
                        if (value.token.type == TokenType.STRING)
                        {
                            variables[node.token.value] = new Element(value.token.value, "string");
                        }
                        else if (value.token.type == TokenType.PLUS)
                        {

                        }
                        else
                        {
                            Error e = new Error("SEMANTIC ERROR: excepted string", node.token.line);
                            Console.WriteLine(e);
                        }
                        return;
                    case "int":
                        if (value.token.type == TokenType.INT)
                        {
                            variables[node.token.value] = new Element(value.token.value, "int");
                        }
                        else if (isArithmeticOperation(value.token.type))
                        {
                            variables[node.token.value] = new Element(arithmetic(value).ToString(), "int");
                        }
                        else
                        {
                            Error e = new Error("SEMANTIC ERROR: excepted int", node.token.line);
                            Console.WriteLine(e);
                        }
                        return;
                    case "bool":
                        return;
                }
            }
            else
            {
                Error e = new Error("SEMANTIC ERROR: undeclared variable", node.token.line);
                Console.WriteLine(e);
            }
        }


        public bool isArithmeticOperation(TokenType type)
        {
            return (type == TokenType.PLUS | type == TokenType.MINUS | type == TokenType.STAR | type == TokenType.SLASH);
        }

        public int arithmetic(Node node)
        {
            TokenType operation = node.token.type;
            Node left = node.childs[0];
            Node right = node.childs[1];
            int leftint = 0;
            int rightint = 0;
            if (isArithmeticOperation(left.token.type))
            {
                leftint = arithmetic(left);
            }
            else if (left.token.type == TokenType.INT)
            {
                Int32.TryParse(left.token.value, out leftint);
            }
            else if (left.token.type == TokenType.IDENTIFIER)
            {
                if (variables.ContainsKey(left.token.value))
                {
                    Int32.TryParse(variables[left.token.value].value, out leftint);
                }
                else
                {
                    Error e = new Error("SEMANTIC ERROR: undeclared variable", left.token.line);
                    Console.WriteLine(e);
                }
            }
            else
            {
                Error e = new Error("SEMANTIC ERROR: excepted int", node.token.line);
                Console.WriteLine(e);
            }
            if (isArithmeticOperation(right.token.type))
            {
                rightint = arithmetic(right);
            }
            else if (right.token.type == TokenType.INT)
            {
                Int32.TryParse(right.token.value, out rightint);
            }
            else if (right.token.type == TokenType.IDENTIFIER)
            {
                if (variables.ContainsKey(right.token.value))
                {
                    Int32.TryParse(variables[right.token.value].value, out rightint);
                }
                else
                {
                    Error e = new Error("SEMANTIC ERROR: undeclared variable", right.token.line);
                    Console.WriteLine(e);
                }
            }
            else
            {
                Error e = new Error("SEMANTIC ERROR: excepted int", node.token.line);
                Console.WriteLine(e);
            }
            if (operation == TokenType.PLUS)
            {
                return leftint + rightint;
            }
            else if (operation == TokenType.MINUS)
            {
                return leftint - rightint;
            }
            else if (operation == TokenType.STAR)
            {
                return leftint * rightint;
            }
            else if (operation == TokenType.SLASH)
            {
                return leftint / rightint;
            }
            return 0;
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
                    Error e = new Error("SEMANTIC ERROR: undeclared variable", node.token.line);
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