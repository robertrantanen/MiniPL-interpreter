using System;
using System.Collections.Generic;

namespace MiniPl
{
    class Parser
    {
        static private List<Token> tokens;
        static private int current = 0;

        static private bool errors = false;

        static private Ast ast;

        static private Node root;

        static private List<TokenType> operators = new List<TokenType>() { TokenType.PLUS, TokenType.MINUS, TokenType.STAR, TokenType.SLASH, TokenType.AND, TokenType.EQUAL, TokenType.LESS };

        public Parser(List<Token> tokens_)
        {
            tokens = tokens_;
        }

        private Node match(Node parent, params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (check(type))
                {
                    Node node = ast.add(tokens[current].type, tokens[current].value, parent);
                    current++;
                    return node;
                }
            }
            Error e = new Error("PARSE ERROR: excepted token type " + types[0] + " but was " + tokens[current].type, tokens[current].line);
            Console.WriteLine(e);
            errors = true;
            return null;
        }


        private bool check(TokenType type)
        {
            return tokens[current].type == type;
        }

        private TokenType peek()
        {
            return tokens[current + 1].type;
        }

        public void parse()
        {
            current = 0;
            errors = false;
            ast = new Ast();
            root = new Node(TokenType.EOF, "program");
            ast.root = root;
            statements(root);
            Console.WriteLine();
            ast.traverse(root);
            ast.printChilds(root);
        }

        private void statements(Node parent)
        {
            //statement();
            //match(TokenType.SEMICOLON);
            while (!check(TokenType.EOF) & !check(TokenType.END) & !errors)
            {
                Console.WriteLine("statements start");
                statement(parent);
                match(parent, TokenType.SEMICOLON);
            }
        }

        private void statement(Node parent)
        {
            Console.WriteLine("statement start");
            switch (tokens[current].type)
            {
                case TokenType.VAR:
                    variableDeclaration(parent);
                    return;
                case TokenType.IDENTIFIER:
                    Node n = match(parent, TokenType.IDENTIFIER);
                    Node n2 = match(n, TokenType.STATEMENT);
                    expression(n2);
                    return;
                case TokenType.READ:
                    Node n3 = match(parent, TokenType.READ);
                    match(n3, TokenType.IDENTIFIER);
                    return;
                case TokenType.PRINT:
                    Node n4 = match(parent, TokenType.PRINT);
                    expression(n4);
                    return;
                case TokenType.ASSERT:
                    Node n5 = match(parent, TokenType.ASSERT);
                    match(n5, TokenType.LEFT_PAREN);
                    expression(n5);
                    match(n5, TokenType.RIGHT_PAREN);
                    return;
                case TokenType.FOR:
                    Node n6 = match(parent, TokenType.FOR);
                    Node n7 = match(n6, TokenType.IDENTIFIER);
                    Node n8 = match(n7, TokenType.IN);
                    expression(n8);
                    match(n8, TokenType.DOUBLEDOT);
                    expression(n8);
                    Node n9 = match(n8, TokenType.DO);
                    statements(n9);
                    match(n9, TokenType.END);
                    match(n9, TokenType.FOR);
                    return;
            }
        }

        private void variableDeclaration(Node parent)
        {
            Console.WriteLine("variable start");
            Node n = match(parent, TokenType.VAR);
            Node n2 = match(n, TokenType.IDENTIFIER);
            Node n3 = match(n2, TokenType.COLON);
            Node n4 = match(n3, TokenType.INTTYPE, TokenType.STRINGTYPE, TokenType.BOOLTYPE);
            if (check(TokenType.STATEMENT))
            {
                Node n5 = match(n4, TokenType.STATEMENT);
                expression(n5);
            }
        }

        private void expression(Node parent)
        {
            Console.WriteLine("expression start");
            if (check(TokenType.NOT))
            {
                Node n = match(parent, TokenType.NOT);
                operand(n);
            }
            else if (operators.Contains(peek()))
            {
                binaryExpression(parent);
            }
            else
            {
                match(parent, TokenType.INT, TokenType.STRING, TokenType.IDENTIFIER);
            }
        }

        private void binaryExpression(Node parent)
        {
            Node node = ast.add(tokens[current+1].type, tokens[current+1].value, parent);
            operand(node);
            //match(parent, operators.ToArray());
            current++;
            operand(node);
        }

        private void operand(Node parent)
        {
            if (check(TokenType.LEFT_PAREN))
            {
                match(parent, TokenType.LEFT_PAREN);
                expression(parent);
                match(parent, TokenType.RIGHT_PAREN);
            }
            else
            {
                match(parent, TokenType.INT, TokenType.STRING, TokenType.IDENTIFIER);
            }
        }




    }
}