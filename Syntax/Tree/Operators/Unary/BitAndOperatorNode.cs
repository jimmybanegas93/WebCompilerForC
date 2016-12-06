﻿using System;
using Syntax.Semantic;
using Syntax.Tree.BaseNodes;

namespace Syntax.Tree.Operators.Unary
{
    public class BitAndOperatorNode : UnaryOperator
    {
        public BitAndOperatorNode()
        {
        }

        public override BaseType ValidateSemantic()
        {
            throw new NotImplementedException();
        }

        public override string GenerateCode()
        {
            return  "&" + Operand.GenerateCode();
        }
    }
}