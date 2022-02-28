using System;
using System.Collections.Generic;

namespace MiniPl
{
    class Parser
    {
        static private List<Token> tokens;
        static private int current = 0;

        static private bool errors = false;

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
            statements();
        }

        private void statements()
        {
            //statement();
            //match(TokenType.SEMICOLON);
            while (!check(TokenType.EOF) & !check(TokenType.END) & !errors)
            {
                Console.WriteLine("statements start");
                statement();
                match(TokenType.SEMICOLON);
            }
        }

        private void statement()
        {
            Console.WriteLine("statement start");
            switch (tokens[current].type)
            {
                case TokenType.VAR:
                    variableDeclaration();
                    return;
                case TokenType.IDENTIFIER:
                    match(TokenType.IDENTIFIER);
                    match(TokenType.STATEMENT);
                    expression();
                    return;
                case TokenType.READ:
                    match(TokenType.READ);
                    match(TokenType.IDENTIFIER);
                    return;
                case TokenType.PRINT:
                    match(TokenType.PRINT);
                    expression();
                    return;
                case TokenType.ASSERT:
                    match(TokenType.ASSERT);
                    match(TokenType.LEFT_PAREN);
                    expression();
                    match(TokenType.RIGHT_PAREN);
                    return;
                case TokenType.FOR:
                    match(TokenType.FOR);
                    match(TokenType.IDENTIFIER);
                    match(TokenType.IN);
                    expression();
                    match(TokenType.DOUBLEDOT);
                    expression();
                    match(TokenType.DO);
                    statements();
                    match(TokenType.END);
                    match(TokenType.FOR);
                    return;
            }
        }

        private void variableDeclaration()
        {
            Console.WriteLine("variable start");
            match(TokenType.VAR);
            match(TokenType.IDENTIFIER);
            match(TokenType.COLON);
            match(TokenType.INTTYPE, TokenType.STRINGTYPE, TokenType.BOOLTYPE);
            if (check(TokenType.STATEMENT))
            {
                match(TokenType.STATEMENT);
                expression();
            }
        }

        private void expression()
        {
            Console.WriteLine("expression start");
            if (check(TokenType.NOT))
            {
                match(TokenType.NOT);
                operand();
            }
            else if (operators.Contains(peek()))
            {
                binaryExpression();
            }
            else
            {
                match(TokenType.INT, TokenType.STRING, TokenType.IDENTIFIER);
            }
        }

        private void binaryExpression()
        {
            operand();
            match(operators.ToArray());
            operand();
        }

        private void operand()
        {
            if (check(TokenType.LEFT_PAREN))
            {
                match(TokenType.LEFT_PAREN);
                expression();
                match(TokenType.RIGHT_PAREN);
            }
            else
            {
                match(TokenType.INT, TokenType.STRING, TokenType.IDENTIFIER);
            }
        }




    }
}