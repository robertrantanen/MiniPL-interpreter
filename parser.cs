using System;
using System.Collections.Generic;

namespace MiniPl
{
    class Parser
    {
        static private List<Token> tokens;
        static private int current = 0;

        static private List<TokenType> operators = new List<TokenType>() {TokenType.PLUS, TokenType.MINUS, TokenType.STAR, TokenType.SLASH, TokenType.AND, TokenType.EQUAL, TokenType.LESS};

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
        }


        private bool check(TokenType type)
        {
            return tokens[current].type == type;
        }

        private TokenType peek() {
            return tokens[current + 1].type;
        }

        public void parse()
        {
            //statement();
            //match(TokenType.SEMICOLON);
            while (!check(TokenType.EOF))
            {
            statement();
            match(TokenType.SEMICOLON);
            }

        }

        private void statement()
        {
            switch (tokens[current].type)
            {
                case TokenType.VAR:
                    variableDeclaration();
                    return;
            }
        }

        private void variableDeclaration()
        {
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
            if (check(TokenType.NOT)) {
                match(TokenType.NOT);
                operand();
            }
            else if (peek() == TokenType.SEMICOLON)
            {
                match(TokenType.INT, TokenType.STRING, TokenType.IDENTIFIER);
            }
            else if (operators.Contains(peek())) {
                binaryExpression();
            }
        }

        private void binaryExpression() {
            operand();
            match(operators.ToArray());
            operand();
        }

        private void operand()
        {
            if (check(TokenType.LEFT_PAREN)) {
                match(TokenType.LEFT_PAREN);
                expression();
                match(TokenType.RIGHT_PAREN);
            } else {
                match(TokenType.INT, TokenType.STRING, TokenType.IDENTIFIER);
            }
        }




    }
}