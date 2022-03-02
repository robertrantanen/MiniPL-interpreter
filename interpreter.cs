using System;
using System.Collections.Generic;

namespace MiniPl
{
    class Interpreter
    {

        static private Ast ast;

        Dictionary<string, Token> variables;
        
        public Interpreter(Ast ast_)
        {
            ast = ast_;
        }

        public void start() {
            variables = new Dictionary<string, Token>();
        }

    }
}