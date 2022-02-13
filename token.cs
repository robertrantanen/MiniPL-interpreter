using System;

namespace MiniPl
{
    class Token
    {
        private TokenType type;
        private string value;
        private int line;

        public Token(TokenType type_, string value_, int line_)
        {
            type = type_;
            value = value_;
            line = line_;
        }

        public TokenType GetTokenType()
        {
            return type;
        }

        public string GetValue()
        {
            return value;
        }

        public int GetLine()
        {
            return line;
        }
    }

    enum TokenType
    {
        LEFT_PAREN, RIGHT_PAREN, DOUBLEDOT, MINUS, PLUS, COLON, SEMICOLON, SLASH, STAR,
        STATEMENT, EQUAL, GREATER, LESS, AND, NOT, IDENTIFIER, STRINGTYPE, INTTYPE,
        VAR, FOR, END, IN, DO, READ, PRINT, INT, STRING, BOOL, ASSERT
    }
}