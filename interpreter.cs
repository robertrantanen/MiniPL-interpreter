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
                    read(node);
                    return;
                case TokenType.ASSERT:
                    return;
                case TokenType.FOR:
                    return;
            }
        }

        public void defineVariable(Node node)
        {
            bool hasValue = false;
            Node iden = node.childs[0];
            Node type = iden.childs[0];
            Node value = null;
            if (type.childs.Count > 0)
            {
                value = type.childs[0];
                hasValue = true;
            }
            if (!variables.ContainsKey(iden.token.value))
            {
                switch (type.token.type)
                {
                    case TokenType.STRINGTYPE:
                        if (!hasValue)
                        {
                            variables.Add(iden.token.value, new Element(null, "string"));
                        }
                        else if (value.token.type == TokenType.STRING)
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
                        if (!hasValue)
                        {
                            variables.Add(iden.token.value, new Element(null, "int"));
                        }
                        else
                        {
                            variables.Add(iden.token.value, new Element(arithmetic(value).ToString(), "int"));
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
                        variables[node.token.value] = new Element(arithmetic(value).ToString(), "int");
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
            if (node.token.type == TokenType.INT)
            {
                return Convert.ToInt32(node.token.value);
            }
            else if (node.token.type == TokenType.IDENTIFIER)
            {
                if (variables.ContainsKey(node.token.value))
                {
                    return Convert.ToInt32(variables[node.token.value].value);
                }
                else
                {
                    Error e = new Error("SEMANTIC ERROR: undeclared variable", node.token.line);
                    Console.WriteLine(e);
                }
            }
            else if (isArithmeticOperation(node.token.type))
            {
                TokenType operation = node.token.type;
                Node left = node.childs[0];
                Node right = node.childs[1];
                int leftint = arithmetic(left);
                int rightint = arithmetic(right);
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
            }
            else
            {
                Error e = new Error("SEMANTIC ERROR: invalid type, excepted int", node.token.line);
                Console.WriteLine(e);
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
                    if (variables[printable.token.value].value == null)
                    {
                        Error e = new Error("SEMANTIC ERROR: null variable", node.token.line);
                        Console.WriteLine(e);
                    }
                    else
                    {
                        Console.WriteLine(variables[printable.token.value].value);
                    }
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
            Node readable = node.childs[0];
            if (readable.token.type == TokenType.IDENTIFIER)
            {
                if (variables.ContainsKey(readable.token.value))
                {
                    Element element = variables[readable.token.value];
                    if (element.type.Equals("string"))
                    {

                        string x = Console.ReadLine();
                        variables[readable.token.value] = new Element(x, "string");
                    }
                    else if (element.type.Equals("int"))
                    {
                        try
                        {
                            int x = Convert.ToInt32(Console.ReadLine());
                            variables[readable.token.value] = new Element(x.ToString(), "int");
                        }
                        catch
                        {
                            Error e = new Error("SEMANTIC ERROR: excepted int", node.token.line);
                            Console.WriteLine(e);
                        }

                    }
                }
                else
                {
                    Error e = new Error("SEMANTIC ERROR: undeclared variable", node.token.line);
                    Console.WriteLine(e);
                }
            }
            else
            {
                Error e = new Error("SEMANTIC ERROR: excepted variable", node.token.line);
                Console.WriteLine(e);
            }
        }


        public void assert(Node node)
        {

        }

        public void forLoop(Node node)
        {

        }

    }
}