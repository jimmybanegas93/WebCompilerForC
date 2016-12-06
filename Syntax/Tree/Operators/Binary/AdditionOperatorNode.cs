﻿using System;
using System.Collections.Generic;
using Syntax.Semantic;
using Syntax.Semantic.Types;
using Syntax.Tree.BaseNodes;

namespace Syntax.Tree.Operators.Binary
{
    public class AdditionOperatorNode : BinaryOperatorNode
    {

        public AdditionOperatorNode()
        {
            Validation = new Dictionary<Tuple<BaseType, BaseType>, BaseType>
            {
                {
                    new Tuple<BaseType, BaseType>(TypesTable.Instance.GetVariable("int"),
                        TypesTable.Instance.GetVariable("int")),
                    TypesTable.Instance.GetVariable("int")
                },
                {
                    new Tuple<BaseType, BaseType>(TypesTable.Instance.GetVariable("float"),
                        TypesTable.Instance.GetVariable("float")),
                    TypesTable.Instance.GetVariable("float")
                },
                {
                    new Tuple<BaseType, BaseType>(TypesTable.Instance.GetVariable("int"),
                        TypesTable.Instance.GetVariable("float")),
                    TypesTable.Instance.GetVariable("float")
                },
                {
                    new Tuple<BaseType, BaseType>(TypesTable.Instance.GetVariable("float"),
                        TypesTable.Instance.GetVariable("int")),
                    TypesTable.Instance.GetVariable("float")
                },
                {
                    new Tuple<BaseType, BaseType>(TypesTable.Instance.GetVariable("char"),
                        TypesTable.Instance.GetVariable("char")),
                    TypesTable.Instance.GetVariable("string")
                },
                {
                    new Tuple<BaseType, BaseType>(TypesTable.Instance.GetVariable("string"),
                        TypesTable.Instance.GetVariable("string")),
                    TypesTable.Instance.GetVariable("string")
                },
                {
                    new Tuple<BaseType, BaseType>(TypesTable.Instance.GetVariable("char"),
                        TypesTable.Instance.GetVariable("string")),
                    TypesTable.Instance.GetVariable("string")
                },
                {
                    new Tuple<BaseType, BaseType>(TypesTable.Instance.GetVariable("string"),
                        TypesTable.Instance.GetVariable("char")),
                    TypesTable.Instance.GetVariable("string")
                },
                {
                    new Tuple<BaseType, BaseType>(TypesTable.Instance.GetVariable("bool"),
                        TypesTable.Instance.GetVariable("bool")),
                    TypesTable.Instance.GetVariable("bool")
                },
                {
                    new Tuple<BaseType, BaseType>(TypesTable.Instance.GetVariable("bool"),
                        TypesTable.Instance.GetVariable("int")),
                    TypesTable.Instance.GetVariable("bool")
                },
                {
                    new Tuple<BaseType, BaseType>(TypesTable.Instance.GetVariable("int"),
                        TypesTable.Instance.GetVariable("bool")),
                    TypesTable.Instance.GetVariable("bool")
                }
            };
        }

        public override string GenerateCode()
        {
            return LeftOperand.GenerateCode() + "+" + RightOperand.GenerateCode();
        }
    }
}