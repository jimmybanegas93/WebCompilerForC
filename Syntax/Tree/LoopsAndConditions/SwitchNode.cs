﻿using System;
using System.Collections.Generic;
using Lexer;
using Syntax.Exceptions;
using Syntax.Semantic;
using Syntax.Semantic.Types;
using Syntax.Tree.BaseNodes;
using Syntax.Tree.GeneralSentences;

namespace Syntax.Tree.LoopsAndConditions
{
    public class SwitchNode : StatementNode
    {
        public ExpressionNode Expression;
        public List<CaseStatement> CaseStatements;
        public override void ValidateSemantic()
        {
            StackContext.Context.Stack.Push(new TypesTable());
       
            var conditional = Expression.ValidateSemantic();

            //if (!(conditional is BooleanType))
            //    throw new SemanticException($"A boolean expression was expected, not a {conditional} at Row: {Position.Row} , Column {Position.Column}");

            foreach (var statement in CaseStatements)
            {
                statement.ValidateSemantic();
            }

            StackContext.Context.PastContexts.Add(CodeGuid, StackContext.Context.Stack.Pop());
        }

        public override void Interpret()
        {
            StackContext.Context.Stack.Push(StackContext.Context.PastContexts[CodeGuid]);

            dynamic switchExpre = Expression.Interpret();

            var defaultStatements = new List<StatementNode>();

            foreach (var statement in CaseStatements)
            {
                if (statement.Expression == null)
                {
                    defaultStatements = statement.Sentences;
                }
                else
                {
                    dynamic result = statement.Expression.Interpret();

                    if (result.Value == switchExpre.Value)
                    {
                        foreach (var sentence in statement.Sentences)
                        {
                            sentence.Interpret();
                        }

                        return;
                    }
                }

                foreach (var defaultStatement in defaultStatements)
                {
                    defaultStatement.Interpret();
                }
            }
         

            StackContext.Context.Stack.Pop();
        }
    }
}
