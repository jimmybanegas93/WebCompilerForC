﻿using System;
using Lexer;
using Syntax.Tree.BaseNodes;

namespace Syntax.Tree.Acessors
{
    public class DeReferenceNode : StatementNode
    {
        //Para  suma (int &entrada)
        public string Value;
        public override void ValidateSemantic()
        {
          //  throw new NotImplementedException();
        }

        public override void Interpret()
        {
            throw new NotImplementedException();
        }
    }
}