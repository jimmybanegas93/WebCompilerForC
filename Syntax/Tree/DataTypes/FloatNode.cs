﻿using System.Globalization;
using Syntax.Semantic;
using Syntax.Semantic.Types;
using Syntax.Tree.BaseNodes;

namespace Syntax.Tree.DataTypes
{
    public class FloatNode : LiteralWithOptionalUnaryOpNode
    {
        public float Value { get; set; }

        public override BaseType ValidateSemantic()
        {
            return new FloatType();
        }

        public override string GenerateCode()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}