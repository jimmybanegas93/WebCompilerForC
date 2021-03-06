﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Lexer;
using Syntax.Exceptions;
using Syntax.Semantic;
using Syntax.Tree.Acessors;
using Syntax.Tree.Arrays;
using Syntax.Tree.BaseNodes;
using Syntax.Tree.DataTypes;
using Syntax.Tree.Declarations;
using Syntax.Tree.GeneralSentences;
using Syntax.Tree.Identifier;
using Syntax.Tree.LoopsAndConditions.Functions;
using Syntax.Tree.Operators.Unary;
using Syntax.Tree.Struct;

namespace Syntax.Parser
{
    public class Parser
    {
        public Lexer.Lexer Lexer;

        public Token CurrentToken;
        public readonly Arrays Arrays;
        public readonly LoopsAndConditionals LoopsAndConditionals;
        public readonly Functions Functions;
        public readonly Expressions Expressions;
        private readonly Utilities _utilities;

        public Parser(Lexer.Lexer lexer)
        {
            Lexer = lexer;
            CurrentToken = lexer.GetNextToken();
            Arrays = new Arrays(this);
            LoopsAndConditionals = new LoopsAndConditionals(this);
            Functions = new Functions(this);
            Expressions = new Expressions(this);

            _utilities = new Utilities(this);
        }

        public Utilities Utilities
        {
            get { return _utilities; }
        }

        public List<StatementNode> Parse()
        {
            var code = Ccode();

            if (CurrentToken.TokenType != TokenType.EndOfFile)
                throw new Exception("End of file expected at row: " + CurrentToken.Row + " , column: " +
                                    CurrentToken.Column);

            return code;
        }

        private List<StatementNode> Ccode()
        {
            return ListOfSentences();
        }

        public List<StatementNode> ListOfSentences()
        {
            if (Utilities.CompareTokenType(TokenType.EndOfFile))
            {
                return new List<StatementNode>();
            }

            if (Utilities.CompareTokenType(TokenType.CloseCurlyBracket))
            {
                return new List<StatementNode>();
            }

            //Lista_Sentencias->Sentence Lista_Sentencias
            // if (Enum.IsDefined(typeof(TokenType), CurrentToken.TokenType))
            if (!Utilities.CompareTokenType(TokenType.EndOfFile))
            {
                Console.WriteLine();

                var statement = Sentence();
                var statementList = ListOfSentences();

                statementList.Insert(0, statement);
                return statementList;

            }
            //Lista_Sentencia->Epsilon
            else
            {
                return new List<StatementNode>();
            }
        }

        public List<StatementNode> ListOfSpecialSentences()
        {
            //Lista_Sentencias->Sentence Lista_Sentencias
            while (!Utilities.CompareTokenType(TokenType.CloseCurlyBracket)
                   && !Utilities.CompareTokenType(TokenType.RwBreak)
                   && !Utilities.CompareTokenType(TokenType.RwCase))
            {
                var statement = SpecialSentence();
                var statementList = ListOfSpecialSentences();
                statementList.Insert(0, statement);
                return statementList;
            }

            return new List<StatementNode>();
        }

        public StatementNode SpecialSentence()
        {
            if (Utilities.CompareTokenType(TokenType.HTMLContent) || Utilities.CompareTokenType(TokenType.CloseCCode))
            {
                Utilities.NextToken();
            }

            if (Utilities.CompareTokenType(TokenType.EndOfFile))
            {
                //return;
            }

            if (Utilities.CompareTokenType(TokenType.RwChar) || Utilities.CompareTokenType(TokenType.RwString)
                || Utilities.CompareTokenType(TokenType.RwInt) || Utilities.CompareTokenType(TokenType.RwDate)
                || Utilities.CompareTokenType(TokenType.RwDouble) || Utilities.CompareTokenType(TokenType.RwBool)
                || Utilities.CompareTokenType(TokenType.RwLong) || Utilities.CompareTokenType(TokenType.RwFloat)
                || Utilities.CompareTokenType(TokenType.RwExtern))
            {
                if (Utilities.CompareTokenType(TokenType.RwExtern))
                {
                    Utilities.NextToken();
                }
                return SpecialDeclaration();
            }
            if (Utilities.CompareTokenType(TokenType.RwIf))
            {
                return LoopsAndConditionals.If();
            }
            if (Utilities.CompareTokenType(TokenType.RwWhile))
            {
                return LoopsAndConditionals.While();
            }
            if (Utilities.CompareTokenType(TokenType.RwDo))
            {
                return LoopsAndConditionals.Do();
            }
            if (Utilities.CompareTokenType(TokenType.RwFor))
            {
                return LoopsAndConditionals.ForLoop();
            }
            if (Utilities.CompareTokenType(TokenType.RwSwitch))
            {
                return LoopsAndConditionals.Switch();
            }
            if (Utilities.CompareTokenType(TokenType.RwBreak))
            {
                if (!StackContext.Context.CanDeclareBreak)
                    throw new SemanticException(
                        $"You cant´t declare a BREAK outside a Loop or condition, Row: {CurrentToken.Row}, Column: {CurrentToken.Column}");

                return LoopsAndConditionals.Break();
            }
            if (Utilities.CompareTokenType(TokenType.RwDefault))
            {
                return LoopsAndConditionals.DefaultCase();
            }
            if (Utilities.CompareTokenType(TokenType.RwContinue))
            {
                if (!StackContext.Context.CanDeclareContinue)
                    throw new SemanticException(
                        $"You cant´t declare a CONTINUE in the current context, Row: {CurrentToken.Row}, Column: {CurrentToken.Column}");

                return LoopsAndConditionals.Continue();
            }
            if (Utilities.CompareTokenType(TokenType.Identifier)
                || Utilities.CompareTokenType(TokenType.OpMultiplication)
                || Utilities.CompareTokenType(TokenType.OpenParenthesis)
                || Utilities.CompareTokenType(TokenType.OpDecrement)
                || Utilities.CompareTokenType(TokenType.OpIncrement))
            {
                var identifier = new IdentifierNode
                {
                    Accessors = new List<AccessorNode>(),
                    Assignation = new AssignationNode(),
                    PointerNodes = new List<PointerNode>()
                };

                if (Utilities.CompareTokenType(TokenType.OpenParenthesis))
                {
                    Utilities.NextToken();

                    if (Utilities.CompareTokenType(TokenType.OpMultiplication))
                    {
                        List<PointerNode> listOfPointer = new List<PointerNode>();
                        IsPointer(listOfPointer);
                        identifier.PointerNodes = listOfPointer;
                    }

                    if (Utilities.CompareTokenType(TokenType.OpIncrement))
                    {
                        identifier.IncrementOrdecrement = new PreIncrementOperatorNode {Value = CurrentToken.Lexeme};
                        Utilities.NextToken();
                    }

                    if (Utilities.CompareTokenType(TokenType.OpDecrement))
                    {
                        identifier.IncrementOrdecrement = new PreDecrementOperatorNode {Value = CurrentToken.Lexeme};
                        Utilities.NextToken();
                    }

                    if (!Utilities.CompareTokenType(TokenType.Identifier))
                    {
                        throw new Exception("Identifier expected at row: " + CurrentToken.Row + " , column: " +
                                            CurrentToken.Column);
                    }

                    var name = CurrentToken.Lexeme;
                    identifier.Value = name;

                    Utilities.NextToken();

                    if (!Utilities.CompareTokenType(TokenType.CloseParenthesis))
                    {
                        throw new Exception("Closing parenthesis required at row: " + CurrentToken.Row + " , column: " +
                                            CurrentToken.Column);
                    }

                    return AssignmentOrFunctionCall(identifier);
                }

                if (Utilities.CompareTokenType(TokenType.OpMultiplication))
                {
                    List<PointerNode> listOfPointer = new List<PointerNode>();
                    IsPointer(listOfPointer);
                    identifier.PointerNodes = listOfPointer;
                }

                if (Utilities.CompareTokenType(TokenType.OpIncrement))
                {
                    identifier.IncrementOrdecrement = new PreIncrementOperatorNode {Value = CurrentToken.Lexeme};
                    Utilities.NextToken();
                }
                if (Utilities.CompareTokenType(TokenType.OpDecrement))
                {
                    identifier.IncrementOrdecrement = new PreDecrementOperatorNode {Value = CurrentToken.Lexeme};
                    Utilities.NextToken();
                }

                if (string.IsNullOrEmpty(identifier.Value))
                {
                    var name = CurrentToken.Lexeme;
                    identifier.Value = name;
                }

                return AssignmentOrFunctionCall(identifier);
            }
            if (Utilities.CompareTokenType(TokenType.RwConst))
            {
                return Const();
            }
            if (Utilities.CompareTokenType(TokenType.RwInclude))
            {
                return Include();
            }
            if (Utilities.CompareTokenType(TokenType.RwReturn))
            {
                if (!StackContext.Context.CanDeclareReturn)
                    throw new SemanticException(
                        $"You cant´t declare a RETURN in current scope, Row: {CurrentToken.Row}, Column: {CurrentToken.Column}");

                return ReturnStatement();
            }
            if (Utilities.CompareTokenType(TokenType.RwStruct)
                || Utilities.CompareTokenType(TokenType.RwTypedef))
            {
                throw new Exception("Not a valid sentence at row: " + CurrentToken.Row + " , column: " +
                                    CurrentToken.Column);
            }

            return null;
        }

        private StatementNode ReturnStatement()
        {
            var returnStatement = new ReturnStatementNode {Position = CurrentToken};

            Utilities.NextToken();

            if (!Utilities.CompareTokenType(TokenType.EndOfSentence))
            {
                returnStatement.ReturnExpression = Expressions.Expression();
            }

            if (!Utilities.CompareTokenType(TokenType.EndOfSentence))
            {
                throw new Exception("End of sentence expected at row: " + CurrentToken.Row + " , column: " +
                                    CurrentToken.Column);
            }

            Utilities.NextToken();

            return returnStatement;
        }

        public StatementNode Sentence()
        {

            if (Utilities.CompareTokenType(TokenType.HTMLContent) || Utilities.CompareTokenType(TokenType.CloseCCode))
            {
                Utilities.NextToken();
            }

            if (Utilities.CompareTokenType(TokenType.RwChar) || Utilities.CompareTokenType(TokenType.RwString)
                || Utilities.CompareTokenType(TokenType.RwInt) || Utilities.CompareTokenType(TokenType.RwDate)
                || Utilities.CompareTokenType(TokenType.RwDouble) || Utilities.CompareTokenType(TokenType.RwBool)
                || Utilities.CompareTokenType(TokenType.RwLong) || Utilities.CompareTokenType(TokenType.RwVoid)
                || Utilities.CompareTokenType(TokenType.RwFloat) || Utilities.CompareTokenType(TokenType.RwExtern))
            {
                if (Utilities.CompareTokenType(TokenType.RwExtern))
                {
                    Utilities.NextToken();
                }
                return Declaration();
            }
            if (Utilities.CompareTokenType(TokenType.RwIf))
            {
                return LoopsAndConditionals.If();
            }
            if (Utilities.CompareTokenType(TokenType.RwWhile))
            {
                return LoopsAndConditionals.While();
            }
            if (Utilities.CompareTokenType(TokenType.RwDo))
            {
                return LoopsAndConditionals.Do();
            }
            if (Utilities.CompareTokenType(TokenType.RwFor))
            {
                return LoopsAndConditionals.ForLoop();
            }
            if (Utilities.CompareTokenType(TokenType.RwSwitch))
            {
                return LoopsAndConditionals.Switch();
            }
            if (Utilities.CompareTokenType(TokenType.RwBreak))
            {
                if (!StackContext.Context.CanDeclareBreak)
                    throw new SemanticException(
                        $"You cant´t declare a BREAK outside a Loop or condition, Row: {CurrentToken.Row}, Column: {CurrentToken.Column}");

                return LoopsAndConditionals.Break();
            }
            if (Utilities.CompareTokenType(TokenType.RwDefault))
            {
                return LoopsAndConditionals.DefaultCase();
            }
            if (Utilities.CompareTokenType(TokenType.RwContinue))
            {
                if (!StackContext.Context.CanDeclareContinue)
                    throw new SemanticException(
                        $"You cant´t declare a CONTINUE in current scope, Row: {CurrentToken.Row}, Column: {CurrentToken.Column}");

                return LoopsAndConditionals.Continue();
            }
            if (Utilities.CompareTokenType(TokenType.Identifier)
                || Utilities.CompareTokenType(TokenType.OpMultiplication)
                || Utilities.CompareTokenType(TokenType.OpenParenthesis)
                || Utilities.CompareTokenType(TokenType.OpDecrement)
                || Utilities.CompareTokenType(TokenType.OpIncrement))
            {

                var identifier = new IdentifierNode
                {
                    Accessors = new List<AccessorNode>(),
                    Assignation = new AssignationNode(),
                    PointerNodes = new List<PointerNode>()
                };

                if (Utilities.CompareTokenType(TokenType.OpenParenthesis))
                {
                    Utilities.NextToken();

                    if (Utilities.CompareTokenType(TokenType.OpMultiplication))
                    {
                        List<PointerNode> listOfPointer = new List<PointerNode>();
                        IsPointer(listOfPointer);
                        identifier.PointerNodes = listOfPointer;
                    }

                    if (Utilities.CompareTokenType(TokenType.OpIncrement))
                    {
                        identifier.IncrementOrdecrement = new PreIncrementOperatorNode {Value = CurrentToken.Lexeme};
                        Utilities.NextToken();
                    }

                    if (Utilities.CompareTokenType(TokenType.OpDecrement))
                    {
                        identifier.IncrementOrdecrement = new PreDecrementOperatorNode {Value = CurrentToken.Lexeme};
                        Utilities.NextToken();
                    }

                    if (!Utilities.CompareTokenType(TokenType.Identifier))
                    {
                        throw new Exception("Identifier expected at row: " + CurrentToken.Row + " , column: " +
                                            CurrentToken.Column);
                    }

                    var name = CurrentToken.Lexeme;
                    identifier.Value = name;

                    Utilities.NextToken();

                    if (!Utilities.CompareTokenType(TokenType.CloseParenthesis))
                    {
                        throw new Exception("Closing parenthesis required at row: " + CurrentToken.Row + " , column: " +
                                            CurrentToken.Column);
                    }

                    return AssignmentOrFunctionCall(identifier);
                }

                if (Utilities.CompareTokenType(TokenType.OpMultiplication))
                {
                    List<PointerNode> listOfPointer = new List<PointerNode>();
                    IsPointer(listOfPointer);
                    identifier.PointerNodes = listOfPointer;
                }

                if (Utilities.CompareTokenType(TokenType.OpIncrement))
                {
                    identifier.IncrementOrdecrement = new PreIncrementOperatorNode {Value = CurrentToken.Lexeme};
                    Utilities.NextToken();
                }
                if (Utilities.CompareTokenType(TokenType.OpDecrement))
                {
                    identifier.IncrementOrdecrement = new PreDecrementOperatorNode {Value = CurrentToken.Lexeme};
                    Utilities.NextToken();
                }

                if (string.IsNullOrEmpty(identifier.Value))
                {
                    var name = CurrentToken.Lexeme;
                    identifier.Value = name;
                }

                return AssignmentOrFunctionCall(identifier);
            }
            if (Utilities.CompareTokenType(TokenType.RwStruct)
                || Utilities.CompareTokenType(TokenType.RwTypedef))
            {
                if (Utilities.CompareTokenType(TokenType.RwTypedef))
                {
                    Utilities.NextToken();
                }

                return Struct();
            }
            if (Utilities.CompareTokenType(TokenType.RwConst))
            {
                return Const();
            }
            if (Utilities.CompareTokenType(TokenType.RwInclude))
            {
                return Include();
            }
            if (Utilities.CompareTokenType(TokenType.RwEnum))
            {
                return Enumeration();
            }
            //Return no debería estar aquí porque no es una sentence
            if (Utilities.CompareTokenType(TokenType.RwReturn))
            {
                if (!StackContext.Context.CanDeclareReturn)
                    throw new SemanticException(
                        $"You cant´t declare a RETURN in current scope, Row: {CurrentToken.Row}, Column: {CurrentToken.Column}");

                return ReturnStatement();
            }

            return null;
        }

        private StatementNode Enumeration()
        {
            var enumerationNode = new EnumerationNode {EnumDeclarations = new List<string>()};

            Utilities.NextToken();

            if (!Utilities.CompareTokenType(TokenType.Identifier))
                throw new Exception("Identifier was expected at row: " + CurrentToken.Row + " , column: " +
                                    CurrentToken.Column);

            enumerationNode.Name = new IdentifierNode {Value = CurrentToken.Lexeme};

            Utilities.NextToken();

            if (!Utilities.CompareTokenType(TokenType.OpenCurlyBracket))
                throw new Exception("Openning bracket was expected at row: " + CurrentToken.Row + " , column: " +
                                    CurrentToken.Column);

            Utilities.NextToken();

            if (!Utilities.CompareTokenType(TokenType.CloseCurlyBracket))
            {
                List<StatementNode> items = new List<StatementNode>();
                enumerationNode.EnumItems = EnumeratorList(items);
            }

            if (!Utilities.CompareTokenType(TokenType.CloseCurlyBracket))
                throw new Exception("Closing bracket was expected at row: " + CurrentToken.Row + " , column: " +
                                    CurrentToken.Column);
            Utilities.NextToken();

            if (Utilities.CompareTokenType(TokenType.Identifier))
            {
                List<string> namesOfEnums = new List<string>();

                while (Utilities.CompareTokenType(TokenType.Identifier))
                {
                    namesOfEnums.Add(CurrentToken.Lexeme);
                    Utilities.NextToken();
                    if (Utilities.CompareTokenType(TokenType.Comma))
                    {
                        Utilities.NextToken();
                    }
                }

                enumerationNode.EnumDeclarations = new List<string>(namesOfEnums);
            }

            if (!Utilities.CompareTokenType(TokenType.EndOfSentence))
                throw new Exception("End of sentence was expected at row: " + CurrentToken.Row + " , column: " +
                                    CurrentToken.Column);

            Utilities.NextToken();

            return enumerationNode;
        }

        private List<StatementNode> EnumeratorList(List<StatementNode> items)
        {
            var item = EnumItem();
            items.Add(item);

            if (Utilities.CompareTokenType(TokenType.Comma))
            {
                OptionalEnumItem(items);
            }

            return items;
        }

        private void OptionalEnumItem(List<StatementNode> items)
        {
            Utilities.NextToken();
            EnumeratorList(items);
        }

        private StatementNode EnumItem()
        {
            if (!Utilities.CompareTokenType(TokenType.Identifier))
            {
                throw new Exception("Identifier was expected at row: " + CurrentToken.Row + " , column: " +
                                    CurrentToken.Column);
            }
            var value = CurrentToken.Lexeme;

            var name = new IdentifierNode {Accessors = null, Value = value};

            Utilities.NextToken();

            var position = OptionalIndexPosition();

            return new EnumItemNode
            {
                ItemName = name,
                OptionalPosition = position
            };
        }

        private IntegerNode OptionalIndexPosition()
        {
            var integerNode = new IntegerNode();

            if (Utilities.CompareTokenType(TokenType.OpSimpleAssignment))
            {
                Utilities.NextToken();
                if (!Utilities.CompareTokenType(TokenType.LiteralNumber))
                    throw new Exception("Literal number was expected at row: " + CurrentToken.Row + " , column: " +
                                        CurrentToken.Column);

                integerNode.Value = int.Parse(CurrentToken.Lexeme);

                Utilities.NextToken();

                return integerNode;
            }
            else
            {

            }

            return new IntegerNode();
        }

        private StatementNode AssignmentOrFunctionCall(IdentifierNode identifier)
        {
            var name = CurrentToken.Lexeme;

            Utilities.NextToken();

            Token position = new Token {Row = CurrentToken.Row, Column = CurrentToken.Column};

            if (Utilities.CompareTokenType(TokenType.OpIncrement)
                || Utilities.CompareTokenType(TokenType.OpDecrement))
            {
                if (Utilities.CompareTokenType(TokenType.OpIncrement))
                {
                    var increment = new PostIncrementOperatorNode {Position = position};

                    identifier.IncrementOrdecrement = increment;
                }
                if (Utilities.CompareTokenType(TokenType.OpDecrement))
                {
                    var decrement = new PostDecrementOperatorNode {Position = position};
                    identifier.IncrementOrdecrement = decrement;
                }

                Utilities.NextToken();

                if (Utilities.CompareTokenType(TokenType.OpSimpleAssignment))
                {
                    Utilities.NextToken();
                    var expression = Expressions.Expression();
                    identifier.Assignation = new AssignationNode {RightValue = expression, Position = position};
                }

                if (!Utilities.CompareTokenType(TokenType.EndOfSentence))
                {
                    throw new Exception("End of sentence ; expected at row: " + CurrentToken.Row + " , column: " +
                                        CurrentToken.Column);
                }
            }

            var accessors = new List<AccessorNode>();

            var id = Expressions.IndexOrArrowAccess(name, accessors);

            identifier.Accessors.AddRange(accessors);
            identifier.Value = ((IdentifierExpression) id).Name;

            position = new Token {Row = CurrentToken.Row, Column = CurrentToken.Column};

            identifier.Position = position;

            if (Utilities.CompareTokenType(TokenType.OpIncrement)
                || Utilities.CompareTokenType(TokenType.OpDecrement))
            {
                Utilities.NextToken();
            }

            bool isFunctionCall;

            var expressionList = ValueForPreId(out isFunctionCall);

            if (Utilities.CompareTokenType(TokenType.EndOfSentence))
            {
                Utilities.NextToken();
            }
            else
            {
                throw new Exception("End of sentence symbol ; expected at row: " + CurrentToken.Row + " , column: " +
                                    CurrentToken.Column);
            }

            if (isFunctionCall)
            {
                return new FunctionCallNode {Name = identifier, Parameters = expressionList, Position = position};
            }

            if (expressionList.Count > 0)
            {
                identifier.Assignation.RightValue = expressionList[0];
            }

            return identifier;
        }

        private List<ExpressionNode> ValueForPreId(out bool isFunctioncall)
        {
            if (Utilities.CompareTokenType(TokenType.OpSimpleAssignment)
                || Utilities.CompareTokenType(TokenType.OpAddAndAssignment)
                || Utilities.CompareTokenType(TokenType.OpSusbtractAndAssignment)
                || Utilities.CompareTokenType(TokenType.OpMultiplyAndAssignment)
                || Utilities.CompareTokenType(TokenType.OpDivideAssignment)
                || Utilities.CompareTokenType(TokenType.OpModulusAssignment)
                || Utilities.CompareTokenType(TokenType.OpBitShiftLeftAndAssignment)
                || Utilities.CompareTokenType(TokenType.OpBitShiftRightAndAssignment)
                || Utilities.CompareTokenType(TokenType.OpBitwiseAndAssignment)
                || Utilities.CompareTokenType(TokenType.OpBitwiseXorAndAssignment)
                || Utilities.CompareTokenType(TokenType.OpBitwiseInclusiveOrAndAssignment))
            {
                var type = CurrentToken.TokenType;

                Utilities.NextToken();

                var expression = Expressions.Expression();

                var expressionUnaryNode = expression as ExpressionUnaryNode;
                if (expressionUnaryNode != null) expressionUnaryNode.Type = type;

                List<ExpressionNode> expressionList = new List<ExpressionNode>();
                expressionList.Add(expression);
                isFunctioncall = false;
                return expressionList;
            }
            if (Utilities.CompareTokenType(TokenType.OpenParenthesis))
            {
                isFunctioncall = true;
                return Functions.CallFunction();
            }

            isFunctioncall = false;
            return new List<ExpressionNode>();
        }

        private StatementNode Include()
        {
            string reference;
            Utilities.NextToken();

            //Literal strings as a parameter for includes
            if (Utilities.CompareTokenType(TokenType.LiteralString))
            {
                reference = CurrentToken.Lexeme;
                Utilities.NextToken();
            }
            else
            {
                throw new Exception("Literal string expected at row: " + CurrentToken.Row + " , column: " +
                                    CurrentToken.Column);
            }

            return new IncludeNode {ReferencedClass = reference};
        }

        private StatementNode Const()
        {
            var position = new Token {Row = CurrentToken.Row, Column = CurrentToken.Column};

            List<PointerNode> listOfPointer = new List<PointerNode>();
            Utilities.NextToken();

            StatementNode dataType = DataType();

            var typeNode = new IdentifierNode
            {
                Accessors = new List<AccessorNode>(),
                Value = ((IdentifierNode) dataType).Value,
                Position = position
            };

            if (Utilities.CompareTokenType(TokenType.OpMultiplication))
            {
                IsPointer(listOfPointer);
            }

            if (!Utilities.CompareTokenType(TokenType.Identifier))
            {
                throw new Exception("Identifier expected at row: " + CurrentToken.Row + " , column: " +
                                    CurrentToken.Column);
            }

            var name = CurrentToken.Lexeme;

            var nameNode = new IdentifierNode {Accessors = new List<AccessorNode>(), Value = name, Position = position};

            Utilities.NextToken();

            if (!Utilities.CompareTokenType(TokenType.OpSimpleAssignment))
            {
                throw new Exception("Assignment expected at row: " + CurrentToken.Row + " , column: " +
                                    CurrentToken.Column);
            }

            Utilities.NextToken();

            var expression = Expressions.Expression();

            if (!Utilities.CompareTokenType(TokenType.EndOfSentence))
            {
                throw new Exception("End Of Sentence expected at row: " + CurrentToken.Row + " , column: " +
                                    CurrentToken.Column);
            }
            Utilities.NextToken();

            var assignation = new AssignationNode
            {
                LeftValue = new IdentifierNode(),
                RightValue = expression,
                Position = position
            };

            return new ConstantNode
            {
                ConstName = nameNode,
                TypeOfConst = typeNode,
                PointersList = listOfPointer,
                Assignation = assignation,
                Position = position
            };
        }

        public StatementNode DataType()
        {
            if (Utilities.CompareTokenType(TokenType.RwChar)
                || Utilities.CompareTokenType(TokenType.RwString)
                || Utilities.CompareTokenType(TokenType.RwInt)
                || Utilities.CompareTokenType(TokenType.RwDate)
                || Utilities.CompareTokenType(TokenType.RwDouble)
                || Utilities.CompareTokenType(TokenType.RwBool)
                || Utilities.CompareTokenType(TokenType.RwLong)
                || Utilities.CompareTokenType(TokenType.RwFloat)
                || Utilities.CompareTokenType(TokenType.RwVoid))
            {
                var type = CurrentToken.Lexeme;

                Utilities.NextToken();
                return new IdentifierNode {Accessors = new List<AccessorNode>(), Value = type};
            }
            throw new Exception("A Data Type was expected at row: " + CurrentToken.Row + " , column: " +
                                CurrentToken.Column);
        }

        private StatementNode Struct()
        {
            var structNode = new StructNode();

            Utilities.NextToken();

            if (!Utilities.CompareTokenType(TokenType.Identifier))
                throw new Exception("Identifier was expected at row: " + CurrentToken.Row + " , column: " +
                                    CurrentToken.Column);

            structNode.Name = new IdentifierExpression
            {
                Accessors = new List<AccessorNode>(),
                Name = CurrentToken.Lexeme
            };

            Utilities.NextToken();

            List<StructItemNode> structItems = new List<StructItemNode>();

            var structDeclaration = StructDeclarationOrInitialization(structItems, structNode.Name.Name);

            structNode.ListOfItems = structItems;

            if (!Utilities.CompareTokenType(TokenType.EndOfSentence))
                throw new Exception("End of sentence was expected at row: " + CurrentToken.Row + " , column: " +
                                    CurrentToken.Column);

            Utilities.NextToken();

            return structDeclaration ?? structNode;
        }

        private StatementNode StructDeclarationOrInitialization(List<StructItemNode> structItems, string name)
        {
            //Declaracion de variable struct no de el struct como tal
            //struct point my_point = { 3, 7 };
            if (Utilities.CompareTokenType(TokenType.Identifier) ||
                Utilities.CompareTokenType(TokenType.OpMultiplication))
            {
                List<IdentifierNode> listOptional = new List<IdentifierNode>();
                var value = new List<ExpressionNode>();

                var general = new GeneralDeclarationNode {DataType = new IdentifierNode {Value = name}};

                if (Utilities.CompareTokenType(TokenType.OpMultiplication))
                {
                    var listOfPointer = new List<PointerNode>();
                    IsPointer(listOfPointer);
                    general.ListOfPointer = listOfPointer;
                }

                general.NameOfVariable = new IdentifierNode {Value = CurrentToken.Lexeme};
                Utilities.NextToken();

                //Posible inicializacion de los valores, así como se inicializa un arreglo
                //struct point my_point = { 3, 7 };
                //struct point *p = &my_point;


                if (Utilities.CompareTokenType(TokenType.Comma))
                {
                    Functions.OptionalId(listOptional);
                }
                var position = new Token {Row = CurrentToken.Row, Column = CurrentToken.Column};

                if (Utilities.CompareTokenType(TokenType.OpSimpleAssignment))
                {
                    value = InitializationForStruct();

                    general.NameOfVariable.Assignation = new AssignationForArray
                    {
                        RightValue = value,
                        Position = position,
                        ArrayIdentifier = general.NameOfVariable
                    };
                }

                if (Utilities.CompareTokenType(TokenType.OpenSquareBracket))
                {
                    var accessors = new List<AccessorNode>();

                    var id = Expressions.IndexOrArrowAccess(name, accessors);

                    general.NameOfVariable.Accessors = accessors;
                }


                if (!Utilities.CompareTokenType(TokenType.EndOfSentence))
                {
                    throw new Exception("Openning bracket was expected at row: " + CurrentToken.Row + " , column: " +
                                        CurrentToken.Column);
                }

                if (listOptional.Count > 0)
                {
                    return new MultideclarationNode
                    {
                        GeneralNode = general,
                        ListOfVariables = listOptional,
                        Position = position
                    };
                }

                return new StructDeclaration {General = general};
            }
            else
            {
                if (!Utilities.CompareTokenType(TokenType.OpenCurlyBracket))
                    throw new Exception("Openning bracket was expected at row: " + CurrentToken.Row + " , column: " +
                                        CurrentToken.Column);

                Utilities.NextToken();

                if (!Utilities.CompareTokenType(TokenType.CloseCurlyBracket))
                {
                    DeclarationOfStruct(false, structItems, null);
                }

                if (!Utilities.CompareTokenType(TokenType.CloseCurlyBracket))
                    throw new Exception("Closing bracket was expected at row: " + CurrentToken.Row + " , column: " +
                                        CurrentToken.Column);
                Utilities.NextToken();

                if (Utilities.CompareTokenType(TokenType.Identifier))
                {
                    Utilities.NextToken();
                }
            }

            return null;
        }

        private List<ExpressionNode> InitializationForStruct()
        {
            Utilities.NextToken();
            if (Utilities.CompareTokenType(TokenType.OpBitAnd))
            {
                var val = ChooseIdType(" ");

                if (val.Reference != null)
                {
                    var list = new List<ExpressionNode>();
                    var name = val.NameOfVariable.Value;

                    list.Add(new BitAndOperatorNode {Value = name});

                    return list;
                }
            }
            else if (Utilities.CompareTokenType(TokenType.OpenCurlyBracket))
            {
                return InitElementsOfStruct();
            }
            return null;
        }

        private List<ExpressionNode> InitElementsOfStruct()
        {
            if (!Utilities.CompareTokenType(TokenType.OpenCurlyBracket))
                throw new Exception("Openning bracket was expected at row: " + CurrentToken.Row + " , column: " +
                                    CurrentToken.Column);

            Utilities.NextToken();

            List<ExpressionNode> list = new List<ExpressionNode>();
            ListOfExpressions(list);

            if (Utilities.CompareTokenType(TokenType.CloseCurlyBracket))
                Utilities.NextToken();

            return list;
        }

        private void DeclarationOfStruct(bool isMultideclaration, List<StructItemNode> structItems,
            StructItemNode itemMultideclaration)
        {
            if (!Utilities.CompareTokenType(TokenType.CloseCurlyBracket))
            {
                var position = new Token {Row = CurrentToken.Row, Column = CurrentToken.Column};

                var structItem = new StructItemNode {Position = position};

                if (!isMultideclaration)
                {
                    GeneralDeclarationNode itemDeclaration = GeneralDeclaration();

                    structItem.ItemDeclaration = itemDeclaration;
                }
                else
                {
                    if (Utilities.CompareTokenType(TokenType.OpMultiplication))
                    {
                        List<PointerNode> listOfPointer = new List<PointerNode>();
                        IsPointer(listOfPointer);
                    }

                    if (!Utilities.CompareTokenType(TokenType.Identifier))
                    {

                        throw new Exception("Identifier was expected at row: " + CurrentToken.Row + " , column: " +
                                            CurrentToken.Column);
                    }
                    Utilities.NextToken();
                }

                if (Utilities.CompareTokenType(TokenType.OpenSquareBracket))
                {
                    Utilities.NextToken();
                    var accesor = Arrays.ArrayIdentifier();

                    if (!isMultideclaration)
                    {
                        structItem.ItemDeclaration.NameOfVariable.Accessors.Add(accesor);
                    }
                    else
                    {
                        itemMultideclaration.ItemDeclaration.NameOfVariable.Accessors.Add(accesor);
                    }

                    if (!Utilities.CompareTokenType(TokenType.CloseSquareBracket))
                        throw new Exception("Closing bracket was expected at row: " + CurrentToken.Row + " , column: " +
                                            CurrentToken.Column);

                    Utilities.NextToken();

                    if (Utilities.CompareTokenType(TokenType.OpenSquareBracket))
                    {
                        Utilities.NextToken();
                        var accesor2 = Arrays.ArrayIdentifier();

                        if (!isMultideclaration)
                        {
                            structItem.ItemDeclaration.NameOfVariable.Accessors.Add(accesor2);
                        }
                        else
                        {
                            itemMultideclaration.ItemDeclaration.NameOfVariable.Accessors.Add(accesor2);
                        }

                        if (!Utilities.CompareTokenType(TokenType.CloseSquareBracket))
                            throw new Exception("Closing bracket was expected at row: " + CurrentToken.Row +
                                                " , column: " +
                                                CurrentToken.Column);

                        Utilities.NextToken();
                    }
                }


                if (!isMultideclaration)
                {
                    structItems.Add(structItem);
                }
                else
                {
                    structItems.Add(itemMultideclaration);
                }


                if (Utilities.CompareTokenType(TokenType.Comma))
                {
                    Utilities.NextToken();
                    var structItemMul = new StructItemNode();
                    GeneralDeclarationNode itemDeclaration;

                    if (isMultideclaration)
                    {
                        itemDeclaration = new GeneralDeclarationNode
                        {
                            DataType = itemMultideclaration.ItemDeclaration.DataType,
                            ListOfPointer = new List<PointerNode>(),
                            NameOfVariable =
                                new IdentifierNode {Accessors = new List<AccessorNode>(), Value = CurrentToken.Lexeme}
                        };
                    }
                    else
                    {
                        itemDeclaration = new GeneralDeclarationNode
                        {
                            DataType = structItem.ItemDeclaration.DataType,
                            ListOfPointer = new List<PointerNode>(),
                            NameOfVariable =
                                new IdentifierNode {Accessors = new List<AccessorNode>(), Value = CurrentToken.Lexeme}
                        };
                    }

                    structItemMul.ItemDeclaration = itemDeclaration;

                    DeclarationOfStruct(true, structItems, structItemMul);
                }
                else if (Utilities.CompareTokenType(TokenType.EndOfSentence))
                {
                    Utilities.NextToken();
                    DeclarationOfStruct(false, structItems, null);
                }
                else
                {
                    throw new Exception("End of sentence symbol ; expected at row: " + CurrentToken.Row + " , column: " +
                                        CurrentToken.Column);
                }
            }
        }

        public GeneralDeclarationNode ChooseIdType(string dataType)
        {
            var position = new Token {Row = CurrentToken.Row, Column = CurrentToken.Column};

            var identifier = new GeneralDeclarationNode {Position = position};

            identifier.DataType = new IdentifierNode {Value = dataType, Position = position};

            if (Utilities.CompareTokenType(TokenType.OpBitAnd))
            {
                var dereference = new DeReferenceNode {Value = CurrentToken.Lexeme};
                identifier.Reference = dereference;

                Utilities.NextToken();

                if (Utilities.CompareTokenType(TokenType.Identifier))
                {
                    identifier.NameOfVariable = new IdentifierNode {Value = CurrentToken.Lexeme, Position = position};
                    Utilities.NextToken();
                }
                else
                {
                    throw new Exception("An Identifier was expected at row: " + CurrentToken.Row + " , column: " +
                                        CurrentToken.Column);
                }
            }
            else if (Utilities.CompareTokenType(TokenType.OpMultiplication))
            {
                List<PointerNode> listOfPointer = new List<PointerNode> {new PointerNode {Position = position}};

                identifier.ListOfPointer = listOfPointer;

                Utilities.NextToken();

                if (Utilities.CompareTokenType(TokenType.OpMultiplication))
                {
                    IsPointer(listOfPointer);

                    identifier.ListOfPointer = listOfPointer;

                    Utilities.NextToken();
                }

                position = new Token {Row = CurrentToken.Row, Column = CurrentToken.Column};
                if (Utilities.CompareTokenType(TokenType.Identifier))
                {
                    identifier.NameOfVariable = new IdentifierNode {Value = CurrentToken.Lexeme, Position = position};
                    Utilities.NextToken();
                }
                else
                {
                    throw new Exception("An Identifier was expected at row: " + CurrentToken.Row + " , column: " +
                                        CurrentToken.Column);
                }
            }
            else if (Utilities.CompareTokenType(TokenType.Identifier))
            {
                identifier.NameOfVariable = new IdentifierNode {Value = CurrentToken.Lexeme, Position = position};
                var nameOfStruct = CurrentToken.Lexeme;

                Utilities.NextToken();

                if (Utilities.CompareTokenType(TokenType.OpenSquareBracket))
                {
                    var accessors = Arrays.ArrayForFunctionsParameter();
                    identifier.NameOfVariable.Accessors = accessors;
                }

                if (Utilities.CompareTokenType(TokenType.OpMultiplication))
                {
                    //Structs as parameters for function
                    List<PointerNode> listOfPointer = new List<PointerNode>();
                    IsPointer(listOfPointer);
                    identifier.ListOfPointer = listOfPointer;
                }
                if (Utilities.CompareTokenType(TokenType.Identifier))
                {
                    //Structs as parameters for function
                    //  identifier.NameOfVariable.StructValue 

                    position = new Token {Row = CurrentToken.Row, Column = CurrentToken.Column};

                    identifier.NameOfVariable = new IdentifierNode {Value = CurrentToken.Lexeme, Position = position};
                    identifier.NameOfVariable.StructValue = nameOfStruct;

                    Utilities.NextToken();
                }

                identifier.Position = CurrentToken;
            }
            else
            {
                throw new Exception("An Identifier was expected at row: " + CurrentToken.Row + " , column: " +
                                    CurrentToken.Column);
            }

            return identifier;
        }

        private StatementNode Declaration()
        {
            var generalDecla = GeneralDeclaration();
            var typeOfDecla = TypeOfDeclaration(generalDecla);

            return typeOfDecla;
        }

        public StatementNode TypeOfDeclaration(GeneralDeclarationNode generalDecla)
        {
            if (Utilities.CompareTokenType(TokenType.OpSimpleAssignment))
            {
                var value = ValueForId();
                var position = new Token {Row = CurrentToken.Row, Column = CurrentToken.Column};

                generalDecla.NameOfVariable.Assignation = new AssignationNode {RightValue = value, Position = position};

                if (Utilities.CompareTokenType(TokenType.EndOfSentence))
                {
                    Utilities.NextToken();
                }
                else if (Utilities.CompareTokenType(TokenType.Comma))
                {
                    List<IdentifierNode> listOptional = new List<IdentifierNode>();
                    Functions.OptionalId(listOptional);

                    position = new Token {Row = CurrentToken.Row, Column = CurrentToken.Column};

                    if (Utilities.CompareTokenType(TokenType.EndOfSentence))
                    {
                        Utilities.NextToken();
                    }
                    else
                    {
                        throw new Exception("An End of sentence ; symbol was expected at row: " + CurrentToken.Row +
                                            " , column: " + CurrentToken.Column);
                    }

                    return new MultideclarationNode
                    {
                        GeneralNode = generalDecla,
                        ListOfVariables = listOptional,
                        Position = position
                    };
                }
                else
                {
                    throw new Exception("An End of sentence ; symbol was expected at row: " + CurrentToken.Row +
                                        " , column: " + CurrentToken.Column);
                }

            }
            else if (Utilities.CompareTokenType(TokenType.Comma))
            {
                var listOptional = new List<IdentifierNode>();
                Functions.OptionalId(listOptional);

                var position = new Token {Row = CurrentToken.Row, Column = CurrentToken.Column};

                if (Utilities.CompareTokenType(TokenType.EndOfSentence))
                {
                    Utilities.NextToken();
                }
                else
                {
                    throw new Exception("An End of sentence ; symbol was expected at row: " + CurrentToken.Row +
                                        " , column: " + CurrentToken.Column);
                }

                return new MultideclarationNode
                {
                    GeneralNode = generalDecla,
                    ListOfVariables = listOptional,
                    Position = position
                };
            }
            else if (Utilities.CompareTokenType(TokenType.OpenSquareBracket))
            {
                bool isInMultideclaration = false;
                List<IdentifierNode> listOptionalArr = new List<IdentifierNode>();

                var tuppleArray = Arrays.IsArrayDeclaration(isInMultideclaration, listOptionalArr);

                var position = new Token {Row = CurrentToken.Row, Column = CurrentToken.Column};

                generalDecla.NameOfVariable.Accessors.AddRange(tuppleArray.Item1);
                generalDecla.NameOfVariable.Assignation = new AssignationForArray
                {
                    RightValue = tuppleArray.Item2,
                    Position = position,
                    ArrayIdentifier = generalDecla.NameOfVariable
                };

                if (Utilities.CompareTokenType(TokenType.Comma))
                {
                    var listOptional = new List<IdentifierNode>();
                    Functions.OptionalId(listOptional);
                }

                if (Utilities.CompareTokenType(TokenType.EndOfSentence))
                {
                    Utilities.NextToken();
                }
                else
                {
                    throw new Exception("An End of sentence ; symbol was expected at row: " + CurrentToken.Row +
                                        " , column: " + CurrentToken.Column);
                }

                if (listOptionalArr.Count > 0)
                {
                    return new MultideclarationNode
                    {
                        GeneralNode = generalDecla,
                        ListOfVariables = listOptionalArr,
                        Position = position
                    };
                }
            }
            else if (Utilities.CompareTokenType(TokenType.OpenParenthesis))
            {
                var functionDeclaration = Functions.IsFunctionDeclaration(generalDecla);

                return functionDeclaration;
            }
            else if (Utilities.CompareTokenType(TokenType.EndOfSentence))
            {
                Utilities.NextToken();
            }
            else
            {
                throw new Exception("An End of sentence ; symbol was expected at row: " + CurrentToken.Row +
                                    " , column: " + CurrentToken.Column);
            }

            return generalDecla;
        }

        public void ListOfExpressions(List<ExpressionNode> list)
        {
            var expression = Expressions.Expression();
            list.Add(expression);

            if (Utilities.CompareTokenType(TokenType.Comma))
            {
                OptionalExpression(list);
            }
        }

        public void OptionalExpression(List<ExpressionNode> list)
        {
            if (Utilities.CompareTokenType(TokenType.Comma))
            {
                Utilities.NextToken();
                ListOfExpressions(list);
            }
            else
            {

            }
        }

        private ExpressionNode ValueForId()
        {
            ExpressionNode expression = null;
            if (Utilities.CompareTokenType(TokenType.OpSimpleAssignment))
            {
                Utilities.NextToken();

                expression = Expressions.Expression();
            }
            else
            {

            }

            return expression;
        }

        public void ListOfId(List<IdentifierNode> list)
        {
            IdentifierNode identifier = new IdentifierNode();

            Utilities.NextToken();

            if (Utilities.CompareTokenType(TokenType.Identifier)
                || Utilities.CompareTokenType(TokenType.OpMultiplication))
            {
                if (Utilities.CompareTokenType(TokenType.OpMultiplication))
                {
                    List<PointerNode> listOfPointer = new List<PointerNode>();
                    IsPointer(listOfPointer);

                    identifier.PointerNodes = listOfPointer;
                }

                var name = CurrentToken.Lexeme;
                identifier.Value = name;

                Utilities.NextToken();

                if (Utilities.CompareTokenType(TokenType.OpenSquareBracket))
                {
                    bool isInMultiDeclaration = true;

                    //List<IdentifierNode> listOptionalArr = new List<IdentifierNode>();
                    var tupleArray = Arrays.IsArrayDeclaration(isInMultiDeclaration, list);

                    identifier.Accessors = new List<AccessorNode>();

                    identifier.Accessors.AddRange(tupleArray.Item1);
                }

                list.Add(identifier);

                OtherIdOrValue(list, identifier);
            }
            else
            {
                throw new Exception("An Identifier was expected at row: " + CurrentToken.Row + " , column: " +
                                    CurrentToken.Column);
            }
        }

        private void OtherIdOrValue(List<IdentifierNode> listOptional, IdentifierNode identifier)
        {
            var value = ValueForId();

            var position = new Token {Row = CurrentToken.Row, Column = CurrentToken.Column};

            if (value != null)
            {
                identifier.Assignation = new AssignationNode {RightValue = value, Position = position};
            }

            Functions.OptionalId(listOptional);
        }

        private GeneralDeclarationNode GeneralDeclaration()
        {
            List<PointerNode> listOfPointer = new List<PointerNode>();
            IdentifierNode nameOfVariable = new IdentifierNode();

            IdentifierNode dataType = (IdentifierNode) DataType();

            var position = new Token();

            if (Utilities.CompareTokenType(TokenType.OpMultiplication))
            {
                IsPointer(listOfPointer);
            }

            if (Utilities.CompareTokenType(TokenType.Identifier))
            {
                nameOfVariable = new IdentifierNode
                {
                    Accessors = new List<AccessorNode>(),
                    Value = CurrentToken.Lexeme
                };

                position = new Token {Row = CurrentToken.Row, Column = CurrentToken.Column};

                Utilities.NextToken();
            }

            return new GeneralDeclarationNode
            {
                DataType = dataType,
                ListOfPointer = listOfPointer,
                NameOfVariable = nameOfVariable,
                Position = position
            };
        }

        public void IsPointer(List<PointerNode> listOfPointer)
        {

            var position = new Token {Row = CurrentToken.Row, Column = CurrentToken.Column};
            listOfPointer.Add(new PointerNode {Position = position});
            Utilities.NextToken();

            if (Utilities.CompareTokenType(TokenType.OpMultiplication))
            {
                IsPointer(listOfPointer);
            }
            else
            {

            }
        }

        private StatementNode SpecialDeclaration()
        {
            var general = GeneralDeclaration();
            var typeOfDecla = TypeOfDeclarationForFunction(general);

            return typeOfDecla;
        }

        private StatementNode TypeOfDeclarationForFunction(GeneralDeclarationNode generalDeclaration)
        {
            if (Utilities.CompareTokenType(TokenType.OpSimpleAssignment))
            {
                var value = ValueForId();
                var position = new Token {Row = CurrentToken.Row, Column = CurrentToken.Column};
                generalDeclaration.NameOfVariable.Assignation = new AssignationNode
                {
                    RightValue = value,
                    Position = position
                };

                if (Utilities.CompareTokenType(TokenType.EndOfSentence))
                {
                    Utilities.NextToken();
                }
                else if (Utilities.CompareTokenType(TokenType.Comma))
                {
                    List<IdentifierNode> listOptional = new List<IdentifierNode>();
                    Functions.OptionalId(listOptional);

                    position = new Token {Row = CurrentToken.Row, Column = CurrentToken.Column};

                    if (Utilities.CompareTokenType(TokenType.EndOfSentence))
                    {
                        Utilities.NextToken();
                    }
                    else
                    {
                        throw new Exception("An End of sentence ; symbol was expected at row: " + CurrentToken.Row +
                                            " , column: " + CurrentToken.Column);
                    }

                    return new MultideclarationNode
                    {
                        GeneralNode = generalDeclaration,
                        ListOfVariables = listOptional,
                        Position = position
                    };
                }
                else
                {
                    throw new Exception("An End of sentence ; symbol was expected at row: " + CurrentToken.Row +
                                        " , column: " + CurrentToken.Column);
                }
            }
            else if (Utilities.CompareTokenType(TokenType.Comma))
            {
                var listOptional = new List<IdentifierNode>();

                var position = new Token {Row = CurrentToken.Row, Column = CurrentToken.Column};
                Functions.OptionalId(listOptional);

                if (Utilities.CompareTokenType(TokenType.EndOfSentence))
                {
                    Utilities.NextToken();
                }
                else
                {
                    throw new Exception("An End of sentence ; symbol was expected at row: " + CurrentToken.Row +
                                        " , column: " + CurrentToken.Column);
                }

                return new MultideclarationNode
                {
                    GeneralNode = generalDeclaration,
                    ListOfVariables = listOptional,
                    Position = position
                };
            }
            else if (Utilities.CompareTokenType(TokenType.OpenSquareBracket))
            {
                bool isInMultideclaration = true;
                List<IdentifierNode> listOptionalArr = new List<IdentifierNode>();

                var tupleArray = Arrays.IsArrayDeclaration(isInMultideclaration, listOptionalArr);

                generalDeclaration.NameOfVariable.Accessors.AddRange(tupleArray.Item1);

                var position = new Token {Row = CurrentToken.Row, Column = CurrentToken.Column};

                generalDeclaration.NameOfVariable.Assignation = new AssignationForArray
                {
                    RightValue = tupleArray.Item2,
                    Position = position,
                    ArrayIdentifier = generalDeclaration.NameOfVariable
                };

                if (Utilities.CompareTokenType(TokenType.Comma))
                {
                    var listOptional = new List<IdentifierNode>();
                    Functions.OptionalId(listOptional);
                }

                if (Utilities.CompareTokenType(TokenType.EndOfSentence))
                {
                    Utilities.NextToken();
                }
                else
                {
                    throw new Exception("An End of sentence ; symbol was expected at row: " + CurrentToken.Row +
                                        " , column: " + CurrentToken.Column);
                }

                if (listOptionalArr.Count > 0)
                {
                    return new MultideclarationNode
                    {
                        GeneralNode = generalDeclaration,
                        ListOfVariables = listOptionalArr,
                        Position = position
                    };
                }
            }
            else if (Utilities.CompareTokenType(TokenType.EndOfSentence))
            {
                Utilities.NextToken();
            }
            else
            {
                throw new Exception("An End of sentence ; symbol was expected at row: " + CurrentToken.Row +
                                    " , column: " + CurrentToken.Column);
            }

            return generalDeclaration;
        }

        public void ParseServer()
        {
            var sentencesList = ListOfSentences();
            ListOfSentencesToValidate = sentencesList;
        }

        public List<StatementNode> ListOfSentencesToValidate { get; set; }
        
        public void ValidateSemanticServer()
        {
            var root = Parse();
            foreach (var sentencesNode in root)
            {
                sentencesNode.ValidateSemantic();
            }

            foreach (var sentencesNode in root)
            {
                sentencesNode.Interpret();
            }
        }
        public void Interpret()
        {
            var root = Parse();

            foreach (var sentencesNode in root)
            {
                sentencesNode.Interpret();
            }
        }
    }
}
