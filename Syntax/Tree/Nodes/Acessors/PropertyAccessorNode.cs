﻿using System;
using Syntax.Tree.Nodes.BaseNodes;

namespace Syntax.Tree.Nodes.Acessors
{
    public class PropertyAccessorNode : AccessorNode
    {
        public IdentifierNode IdentifierNode { get; set; }
        public override BaseType ValidateSemantic()
        {
            throw new NotImplementedException();
        }

        public override string GenerateCode()
        {
            return "." + IdentifierNode.Value;
        }
    }
}
