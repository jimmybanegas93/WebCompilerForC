﻿using System;
using Syntax.Semantic;
using Syntax.Semantic.Types;
using Syntax.Tree.BaseNodes;

namespace Syntax.Tree.Operators.Unary
{
    public class ExpressionUnaryNode : ExpressionNode
    {
        public UnaryOperator UnaryOperator;

        public ExpressionNode Factor;
        public override BaseType ValidateSemantic()
        {
            //  throw new NotImplementedException();
         
            return Factor.ValidateSemantic();
        }

        public override string GenerateCode()
        {
            throw new NotImplementedException();
        }
    }
}
