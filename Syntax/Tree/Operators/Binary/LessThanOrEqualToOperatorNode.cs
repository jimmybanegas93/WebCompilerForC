﻿using System;
using Syntax.Semantic;
using Syntax.Tree.BaseNodes;

namespace Syntax.Tree.Operators.Binary
{
    class LessThanOrEqualToOperatorNode : BinaryOperatorNode
    {
        public LessThanOrEqualToOperatorNode()
        {

        }

        public override BaseType ValidateSemantic()
        {
            throw new NotImplementedException();
        }

        public override string GenerateCode()
        {
            return LeftOperand.GenerateCode() + "<=" + RightOperand.GenerateCode();
        }
    }
}
