﻿using System;
using Syntax.Exceptions;
using Syntax.Semantic;
using Syntax.Semantic.Types;
using Syntax.Tree.BaseNodes;

namespace Syntax.Tree.Acessors
{
    public class ArrayAccessorNode : AccessorNode
    {
        public ExpressionNode IndexExpression { get; set; }
        public override BaseType ValidateSemantic()
        {
          //  throw new NotImplementedException();
          return new BooleanType();
        }

    
        public override string GenerateCode()
        {
            return "[" + IndexExpression.GenerateCode() + "]";
        }

        public override BaseType ValidateSemanticType(string type)
        {
            var expressionType = IndexExpression.ValidateSemantic();

            //if (type == expressionType)
            //{
            //    return expressionType;
            //}

            throw new SemanticException($"Types don't match {type} and {expressionType}");
        }
    }
}