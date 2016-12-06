﻿using System;
using Syntax.Semantic;
using Syntax.Semantic.Types;
using Syntax.Tree.BaseNodes;

namespace Syntax.Tree.DataTypes
{
    class CharNode : ExpressionNode
    {
        public string Value { get; set; }
        public override BaseType ValidateSemantic()
        {
            //return new CharType();
            return TypesTable.Instance.GetVariable("char");
        }

        public override string GenerateCode()
        {
            return Value;
        }
    }
}
