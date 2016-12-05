﻿using System.Collections.Generic;
using Syntax.Exceptions;
using Syntax.Semantic.Types;
using Syntax.Tree.BaseNodes;

namespace Syntax.Tree.LoopsAndConditions
{
    public class CaseStatement : StatementNode
    {
        public ExpressionNode Expression { get; set; }
        public List<StatementNode> Sentences { get; set; }
        public override void ValidateSemantic()
        {
            var conditional = Expression.ValidateSemantic();

            if (!(conditional is BooleanType))
                throw new SemanticException($"A boolean expression is expected, not a {conditional}");

            foreach (var statement in Sentences)
            {
                statement.ValidateSemantic();
            }
        }

        public override string GenerateCode()
        {
            throw new System.NotImplementedException();
        }
    }
}