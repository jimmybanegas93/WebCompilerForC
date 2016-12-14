﻿using System.Globalization;
using Syntax.Semantic;
using Syntax.Semantic.Types;
using Syntax.Tree.BaseNodes;

namespace Syntax.Tree.DataTypes
{
    public class DecimalNode : LiteralWithOptionalUnaryOpNode
    {
        public decimal Value { get; set; }

        public override BaseType ValidateSemantic()
        {
            return StackContext.Context.GetGeneralType("float");
        }

        public override string Interpret()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}