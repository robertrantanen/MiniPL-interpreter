using System;
using System.Collections.Generic;

namespace MiniPl
{
    class Scanner
    {

        static private int start = 0;
        static private int current = 0;
        static private int end = 0;
        static private int line = 1;
        static private string text = "";

        public List<Token> scan(string file)
        {
            start = 0;
            current = 0;
            line = 1;
            text = file;
            end = text.Length;
            List<Token> tokens = new List<Token>();

            Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>();
            keywords["var"] = TokenType.VAR;
            keywords["for"] = TokenType.FOR;
            keywords["end"] = TokenType.END;
            keywords["in"] = TokenType.IN;
            keywords["do"] = TokenType.DO;
            keywords["read"] = TokenType.READ;
            keywords["print"] = TokenType.PRINT;
            keywords["assert"] = TokenType.ASSERT;
            keywords["bool"] = TokenType.BOOLTYPE;
            keywords["string"] = TokenType.STRINGTYPE;
            keywords["int"] = TokenType.INTTYPE;

            //Console.WriteLine(text);
            while (start < end)
            {
                char c = text[start];
                //Console.WriteLine(c);
                switch (c)
                {
                    case ';':
                        tokens.Add(new Token(TokenType.SEMICOLON, c.ToString(), line));
                        break;
                    case '+':
                        tokens.Add(new Token(TokenType.PLUS, c.ToString(), line));
                        break;
                    case '-':
                        tokens.Add(new Token(TokenType.MINUS, c.ToString(), line));
                        break;
                    case '(':
                        tokens.Add(new Token(TokenType.LEFT_PAREN, c.ToString(), line));
                        break;
                    case ')':
                        tokens.Add(new Token(TokenType.RIGHT_PAREN, c.ToString(), line));
                        break;
                    case '&':
                        tokens.Add(new Token(TokenType.AND, c.ToString(), line));
                        break;
                    case '!':
                        tokens.Add(new Token(TokenType.NOT, c.ToString(), line));
                        break;
                    case '=':
                        tokens.Add(new Token(TokenType.EQUAL, c.ToString(), line));
                        break;
                    case '<':
                        tokens.Add(new Token(TokenType.LESS, c.ToString(), line));
                        break;
                    case '*':
                        tokens.Add(new Token(TokenType.STAR, c.ToString(), line));
                        break;
                    case ':':
                        if (peek(start).Equals('='))
                        {
                            tokens.Add(new Token(TokenType.STATEMENT, text.Substring(start, 2), line));
                            start++;
                        }
                        else
                        {
                            tokens.Add(new Token(TokenType.COLON, c.ToString(), line));
                        }
                        break;
                    case '.':
                        if (peek(start).Equals('.'))
                        {
                            tokens.Add(new Token(TokenType.DOUBLEDOT, text.Substring(start, 2), line));
                            start++;
                        }
                        break;
                    case '/':
                        if (peek(start).Equals('/'))
                        {
                            start++;
                            skipComment();

                        }
                        else if (peek(start).Equals('*'))
                        {
                            start++;
                            skipMultiComment();
                        }
                        else
                        {
                            tokens.Add(new Token(TokenType.SLASH, c.ToString(), line));
                        }
                        break;
                    case '\"':
                        tokens.Add(new Token(TokenType.STRING, scanString(), line));
                        break;
                    case ' ':
                    case '\r':
                    case '\t':
                        break;
                    case '\n':
                        line++;
                        break;
                    default:
                        if (isDigit(c))
                        {
                            tokens.Add(new Token(TokenType.INT, scanNumber(), line));
                        }
                        else if (isAlphabetic(c))
                        {
                            string val = scanIdentifier();
                            if (keywords.ContainsKey(val))
                            {
                                TokenType type = keywords[val];
                                tokens.Add(new Token(type, val, line));
                            }
                            else
                            {
                                tokens.Add(new Token(TokenType.IDENTIFIER, val, line));
                            }
                        }
                        else
                        {
                            Error e = new Error("LEXICAL ERROR: invalid token " + c, line);
                            Console.WriteLine(e);
                        }
                        break;
                }



                start++;
            }
            tokens.Add(new Token(TokenType.EOF, "EOF", line));
            return tokens;

        }

        private Char peek(int i)
        {
            if (i < end)
            {
                return text[i + 1];
            }
            else
            {
                Error e = new Error("LEXICAL ERROR: end of file", line);
                Console.WriteLine(e);
                return '\0';
            }
        }

        private bool isDigit(char c)
        {
            return (c >= '0' & c <= '9');
        }

        private bool isAlphabetic(char c)
        {
            return (c >= 'a' & c <= 'z') ||
                   (c >= 'A' & c <= 'Z') ||
                    c == '_';
        }

        private bool isAlphabeticOrNumeric(char c)
        {
            return isAlphabetic(c) || isDigit(c);
        }

        private string scanString()
        {
            string s = "";
            current = start;
            while (true)
            {
                current++;
                if (current == end)
                {
                    Error e = new Error("LEXICAL ERROR: unclosed string", line);
                    Console.WriteLine(e);
                    break;
                }
                else if (text[current].Equals('\"'))
                {
                    start = current;
                    break;
                }
                else if (text[current].Equals('\\'))
                {
                    char next = peek(current);
                    if (next.Equals('\"'))
                    {
                        s = s + '\"';
                        current++;
                    }
                    else if (next.Equals('\''))
                    {
                        s = s + '\'';
                        current++;
                    }
                    else if (next.Equals('\\'))
                    {
                        s = s + '\\';
                        current++;
                    }
                    else if (next.Equals('n'))
                    {
                        s = s + Environment.NewLine;
                        current++;
                    }
                }
                else
                {
                    s = s + text[current];
                }

            }
            return s;
        }

        private string scanNumber()
        {
            string s = "";
            current = start;
            while (current < end)
            {
                if (isDigit(text[current]))
                {
                    s = s + text[current];
                }
                else
                {
                    start = current - 1;
                    break;
                }
                current++;
            }
            return s;
        }

        private string scanIdentifier()
        {
            string s = "";
            current = start;
            while (current < end)
            {
                if (isAlphabeticOrNumeric(text[current]))
                {
                    s = s + text[current];
                }
                else
                {
                    start = current - 1;
                    break;
                }
                current++;
            }
            return s;
        }

        private void skipComment()
        {
            current = start;
            while (current < end-1)
            {
                current++;
                char c = text[current];
                if (c.Equals('\n'))
                {
                    line++;
                    start = current;
                    break;
                }
            }
        }

        private void skipMultiComment()
        {
            current = start;
            int tempLine = line;
            while (current < end-1)
            {
                current++;
                if (current == end)
                {
                    Error e = new Error("LEXICAL ERROR: unclosed comment", line);
                    Console.WriteLine(e);
                    break;
                }
                char c = text[current];
                if (c.Equals('\n'))
                {
                    tempLine++;
                }
                if (c.Equals('*'))
                {
                    if (peek(current).Equals('/'))
                    {
                        current++;
                        start = current;
                        line = tempLine;
                        break;
                    }
                }
            }
        }
    }
}