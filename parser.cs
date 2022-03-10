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

        private void match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (check(type))
                {
                    current++;
                    return;
                }
            }
            Error e = new Error("PARSE ERROR: excepted token type " + types[0] + " but was " + tokens[current].type, tokens[current].line);
            Console.WriteLine(e);
            errors = true;
        }

        private Node matchAddNode(Node parent, params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (check(type))
                {
                    Node node = ast.add(tokens[current], parent);
                    current++;
                    return node;
                }
            }
            Error e = new Error("PARSE ERROR: excepted token type " + types[0] + " but was " + tokens[current].type, tokens[current].line);
            Console.WriteLine(e);
            errors = true;
            return null;
        }

        private Node matchAddNextNodeWithoutAdvancing(Node parent, params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (tokens[current + 1].type == type)
                {
                    Node node = ast.add(tokens[current + 1], parent);
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

        public Ast parse()
        {
            current = 0;
            errors = false;
            ast = new Ast();
            root = new Node(new Token(TokenType.EOF, "program", 0));
            ast.root = root;
            statements(root);
            return ast;
        }

        private void statements(Node parent)
        {
            while (!check(TokenType.EOF) & !check(TokenType.END) & !errors)
            {
                statement(parent);
                match(TokenType.SEMICOLON);
            }
        }

        private void statement(Node parent)
        {
            switch (tokens[current].type)
            {
                case TokenType.VAR:
                    variableDeclaration(parent);
                    return;
                case TokenType.IDENTIFIER:
                    Node n = matchAddNode(parent, TokenType.IDENTIFIER);
                    match(TokenType.STATEMENT);
                    expression(n);
                    return;
                case TokenType.READ:
                    Node n2 = matchAddNode(parent, TokenType.READ);
                    matchAddNode(n2, TokenType.IDENTIFIER);
                    return;
                case TokenType.PRINT:
                    Node n3 = matchAddNode(parent, TokenType.PRINT);
                    expression(n3);
                    return;
                case TokenType.ASSERT:
                    Node n4 = matchAddNode(parent, TokenType.ASSERT);
                    match(TokenType.LEFT_PAREN);
                    expression(n4);
                    match(TokenType.RIGHT_PAREN);
                    return;
                case TokenType.FOR:
                    Node n5 = matchAddNode(parent, TokenType.FOR);
                    Node n6 = matchAddNode(n5, TokenType.IDENTIFIER);
                    Node n7 = matchAddNode(n6, TokenType.IN);
                    Node n8 = matchAddNextNodeWithoutAdvancing(n7, TokenType.DOUBLEDOT);
                    expression(n8);
                    current++;
                    expression(n8);
                    Node n9 = matchAddNode(n7, TokenType.DO);
                    statements(n9);
                    match(TokenType.END);
                    match(TokenType.FOR);
                    return;
            }
        }

        private void variableDeclaration(Node parent)
        {
            Node n = matchAddNode(parent, TokenType.VAR);
            Node n2 = matchAddNode(n, TokenType.IDENTIFIER);
            match(TokenType.COLON);
            Node n3 = matchAddNode(n2, TokenType.INTTYPE, TokenType.STRINGTYPE, TokenType.BOOLTYPE);
            if (check(TokenType.STATEMENT))
            {
                match(TokenType.STATEMENT);
                expression(n3);
            }
        }

        private void expression(Node parent)
        {
            if (check(TokenType.NOT))
            {
                Node n = matchAddNode(parent, TokenType.NOT);
                operand(n);
            }
            else if (operators.Contains(peek()))
            {
                binaryExpression(parent);
            }
            else
            {
                matchAddNode(parent, TokenType.INT, TokenType.STRING, TokenType.IDENTIFIER);
            }
        }

        private void binaryExpression(Node parent)
        {
            Node n = matchAddNextNodeWithoutAdvancing(parent, operators.ToArray());
            operand(n);
            current++;
            operand(n);
        }

        private void operand(Node parent)
        {
            if (check(TokenType.LEFT_PAREN))
            {
                match(TokenType.LEFT_PAREN);
                expression(parent);
                match(TokenType.RIGHT_PAREN);
            }
            else
            {
                matchAddNode(parent, TokenType.INT, TokenType.STRING, TokenType.IDENTIFIER);
            }
        }




    }
}