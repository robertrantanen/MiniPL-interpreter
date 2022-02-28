using System;

namespace MiniPl
{
    class Token
    {

        public TokenType type { get; set; }
        public string value { get; set; }
        public int line { get; set; }


        public Token(TokenType type_, string value_, int line_)
        {
            type = type_;
            value = value_;
            line = line_;
        }

        public override string ToString()
        {
            return type + " " + value + " " + line;
        }
    }

    enum TokenType
    {
        LEFT_PAREN, RIGHT_PAREN, DOUBLEDOT, MINUS, PLUS, COLON, SEMICOLON, SLASH, STAR,
        STATEMENT, EQUAL, GREATER, LESS, AND, NOT, IDENTIFIER, STRINGTYPE, INTTYPE, BOOLTYPE,
        VAR, FOR, END, IN, DO, READ, PRINT, INT, STRING, BOOL, ASSERT, EOF
    }
}