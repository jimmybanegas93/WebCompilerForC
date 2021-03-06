﻿using System;
using Lexer;
using Syntax.Interpret;
using Syntax.Semantic;
using Syntax.Semantic.Types;
using Syntax.Tree.BaseNodes;

namespace Syntax.Tree.Operators.Unary
{
    public class ExpressionUnaryNode : ExpressionNode
    {
        public UnaryOperator UnaryOperator;

        public ExpressionNode Factor;
        public TokenType Type { get; set; }

        public override BaseType ValidateSemantic()
        {
            return Factor.ValidateSemantic();
        }

        public override Value Interpret()
        {
            return Factor.Interpret();
        }
    }
}
