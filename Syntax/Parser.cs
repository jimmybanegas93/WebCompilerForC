﻿using System;
using Lexer;

namespace Syntax
{
    public class Parser
    {
        public readonly Lexer.Lexer Lexer;

        public Token CurrentToken;
        private readonly Arrays _arrays;
        public readonly LoopsAndConditionals LoopsAndConditionals;
        private readonly Utilities _utilities;
        public readonly Functions Functions;
        public readonly Expressions Expressions;

        public Parser(Lexer.Lexer lexer)
        {
            Lexer = lexer;
            CurrentToken = lexer.GetNextToken();
            _arrays = new Arrays(this);
            LoopsAndConditionals = new LoopsAndConditionals(this);
            _utilities = new Utilities(this);
            Functions = new Functions(this);
            Expressions = new Expressions(this);
        }

        public Utilities Utilities
        {
            get { return _utilities; }
        }

        public void Parse()
        {
            Ccode();

            if (CurrentToken.TokenType != TokenType.EndOfFile)
                throw new Exception("End of file expected");
        }

        private void Ccode()
        {
            ListOfSentences();
        }

        public void ListOfSentences()
        {

            if (Utilities.CompareTokenType(TokenType.CloseCurlyBracket))
            {
                return;
            }
            
            //Lista_Sentencias->Sentence Lista_Sentencias
            if (Enum.IsDefined(typeof(TokenType), CurrentToken.TokenType))
           /* if (_currentToken.TokenType == TokenType.Identifier || _currentToken.TokenType == TokenType.RwInclude 
                || _currentToken.TokenType == TokenType.RwIf || _currentToken.TokenType == TokenType.RwWhile
                || _currentToken.TokenType == TokenType.RwDo || _currentToken.TokenType == TokenType.RwFor 
                || _currentToken.TokenType == TokenType.RwSwitch || _currentToken.TokenType == TokenType.RwStruct 
                || _currentToken.TokenType == TokenType.RwConst || _currentToken.TokenType== TokenType.RwBreak
                || _currentToken.TokenType == TokenType.RwContinue)*/
            {
                Console.WriteLine();

                Sentence();
                ListOfSentences();
            }
            //Lista_Sentencia->Epsilon
            else
            {

            }
        }

        public void ListOfSpecialSentences()
        {
            //Lista_Sentencias->Sentence Lista_Sentencias
            while (!Utilities.CompareTokenType(TokenType.CloseCurlyBracket)
                && !Utilities.CompareTokenType(TokenType.RwBreak)
                && !Utilities.CompareTokenType(TokenType.RwCase))
            {
                SpecialSentence();
                ListOfSpecialSentences();
            }
         
        }

        public void SpecialSentence()
        {

            if (Utilities.CompareTokenType(TokenType.RwChar) || Utilities.CompareTokenType(TokenType.RwString)
                 || Utilities.CompareTokenType(TokenType.RwInt) || Utilities.CompareTokenType(TokenType.RwDate)
                 || Utilities.CompareTokenType(TokenType.RwDouble) || Utilities.CompareTokenType(TokenType.RwBool)
                 || Utilities.CompareTokenType(TokenType.RwLong) || Utilities.CompareTokenType(TokenType.RwFloat))
            {
                SpecialDeclaration();
            }
            else if (Utilities.CompareTokenType(TokenType.RwIf))
            {
                LoopsAndConditionals.If();
            }
            else if (Utilities.CompareTokenType(TokenType.RwWhile))
            {
                LoopsAndConditionals.While();
            }
            else if (Utilities.CompareTokenType(TokenType.RwDo))
            {
                LoopsAndConditionals.Do();
            }
            else if (Utilities.CompareTokenType(TokenType.RwFor))
            {
                LoopsAndConditionals.ForLoop();
            }
            else if (Utilities.CompareTokenType(TokenType.RwSwitch))
            {
                LoopsAndConditionals.Switch();
            }
            else if (Utilities.CompareTokenType(TokenType.RwBreak))
            {
                LoopsAndConditionals.Break();
            }
            else if (Utilities.CompareTokenType(TokenType.RwContinue))
            {
                LoopsAndConditionals.Continue();
            }
            else if (Utilities.CompareTokenType(TokenType.Identifier))
            {
                AssignmentOrFunctionCall();
            }
            else if (Utilities.CompareTokenType(TokenType.RwConst))
            {
                Const();
            }
            else if (Utilities.CompareTokenType(TokenType.RwInclude))
            {
                Include();
            }
            else if (Utilities.CompareTokenType(TokenType.RwReturn))
            {
                ReturnStatement();
            }
            else
            {
                try
                {

                }
                catch (Exception)
                {

                    throw new Exception("Not a valid sentence");
                }

            }
        }

        private void ReturnStatement()
        {
            Utilities.NextToken();

            Expressions.Expression();

            if (!Utilities.CompareTokenType(TokenType.EndOfSentence))
            {
                throw new Exception("End of sentence expected");
            }

            Utilities.NextToken();
        }

        public void Sentence()
        {
            if (Utilities.CompareTokenType(TokenType.RwChar) || Utilities.CompareTokenType(TokenType.RwString)
                  || Utilities.CompareTokenType(TokenType.RwInt) || Utilities.CompareTokenType(TokenType.RwDate)
                  || Utilities.CompareTokenType(TokenType.RwDouble) || Utilities.CompareTokenType(TokenType.RwBool)
                  || Utilities.CompareTokenType(TokenType.RwLong) || Utilities.CompareTokenType(TokenType.RwVoid)
                  || Utilities.CompareTokenType(TokenType.RwFloat))
            {
                Declaration();
            }
            else if (Utilities.CompareTokenType(TokenType.RwIf))
            {
                LoopsAndConditionals.If();
            }
            else if (Utilities.CompareTokenType(TokenType.RwWhile))
            {
                LoopsAndConditionals.While();
            }
            else if (Utilities.CompareTokenType(TokenType.RwDo))
            {
                LoopsAndConditionals.Do();
            }
            else if (Utilities.CompareTokenType(TokenType.RwFor))
            {
                LoopsAndConditionals.ForLoop();
            }
            else if (Utilities.CompareTokenType(TokenType.RwSwitch))
            {
                LoopsAndConditionals.Switch();
            }
            else if (Utilities.CompareTokenType(TokenType.RwBreak))
            {
                LoopsAndConditionals.Break();
            }
            else if (Utilities.CompareTokenType(TokenType.RwContinue))
            {
                LoopsAndConditionals.Continue();
            }
            else if (Utilities.CompareTokenType(TokenType.Identifier))
            {
                AssignmentOrFunctionCall();
            }
            else if (Utilities.CompareTokenType(TokenType.RwStruct))
            {
                Struct();
            }
            else if (Utilities.CompareTokenType(TokenType.RwConst))
            {
                Const();
            }
            else if (Utilities.CompareTokenType(TokenType.RwInclude))
            {
                Include();
            }
            else if (Utilities.CompareTokenType(TokenType.RwEnum))
            {
                Enumeration();
            }
            //Return no debería estar aquí porque no es una sentence
            else if (Utilities.CompareTokenType(TokenType.RwReturn))
            {
                ReturnStatement();
            }
            else
            {
                try
                {

                }
                catch (Exception)
                {

                    throw new Exception("Not a valid sentence");
                }
               
            }

        }

        private void Enumeration()
        {
            Utilities.NextToken();

            if (!Utilities.CompareTokenType(TokenType.Identifier))
                throw new Exception("Identifier was expected");

            Utilities.NextToken();

            if (!Utilities.CompareTokenType(TokenType.OpenCurlyBracket))
                throw new Exception("Openning bracket was expected");

            Utilities.NextToken();

            if (!Utilities.CompareTokenType(TokenType.CloseCurlyBracket))
            {
                EnumeratorList();
            }

            if (!Utilities.CompareTokenType(TokenType.CloseCurlyBracket))
                throw new Exception("Closing bracket was expected");
            Utilities.NextToken();

            if (!Utilities.CompareTokenType(TokenType.EndOfSentence))
                throw new Exception("End of sentence was expected");

            Utilities.NextToken();
        }

        private void EnumeratorList()
        {
            EnumItem();

            if (Utilities.CompareTokenType(TokenType.Comma))
            {
                OptionalEnumItem();
            }

        }

        private void OptionalEnumItem()
        {
            Utilities.NextToken();
            EnumeratorList();
        }

        private void EnumItem()
        {
            if (!Utilities.CompareTokenType(TokenType.Identifier))
            {
                throw new Exception("Identifier was expected");
            }

            Utilities.NextToken();

            OptionalIndexPosition();
        }

        private void OptionalIndexPosition()
        {
            if (Utilities.CompareTokenType(TokenType.OpSimpleAssingment))
            {
                Utilities.NextToken();
                if (!Utilities.CompareTokenType(TokenType.LiteralNumber))
                    throw new Exception("Literal number was expected");
                Utilities.NextToken();
            }
            else
            {
                
            } 
        }

        private void AssignmentOrFunctionCall()
        {
            Utilities.NextToken();

            ValueForPreId();

            if (Utilities.CompareTokenType(TokenType.EndOfSentence))
            {
                Utilities.NextToken();
            }
            else
            {
                throw new Exception("End of sentence symbol ; expected");
            }
        }

        private void ValueForPreId()
        {
            if (Utilities.CompareTokenType(TokenType.OpSimpleAssingment))
            {
                Utilities.NextToken();
                Expressions.Expression();
            }
            else if (Utilities.CompareTokenType(TokenType.OpenParenthesis))
            {
                Functions.CallFunction();
            }
        }

        private void Include()
        {
            Utilities.NextToken();

            //Literal strings as a parameter for includes
            if (Utilities.CompareTokenType(TokenType.LiteralString))
            {
                Utilities.NextToken();
            }
            else
            {
                throw new Exception("Literal string expected");
            }
        }

        private void Const()
        {
             Utilities.NextToken();

             DataType();

            if (!Utilities.CompareTokenType(TokenType.Identifier))
            {
                throw new Exception("Identifier expected");
            }

            Utilities.NextToken();

            if (!Utilities.CompareTokenType(TokenType.OpSimpleAssingment))
            {
                throw new Exception("Assignment expected");
            }

            Expressions.Expression();

            if (!Utilities.CompareTokenType(TokenType.EndOfSentence))
            {
                throw new Exception("End Of Sentence expected");
            }
            Expressions.Expression();
        }

        public void DataType()
        {
           if (Utilities.CompareTokenType(TokenType.RwChar) 
                ||Utilities.CompareTokenType(TokenType.RwString)
                ||Utilities.CompareTokenType(TokenType.RwInt)
                ||Utilities.CompareTokenType(TokenType.RwDate)
                ||Utilities.CompareTokenType(TokenType.RwDouble)
                ||Utilities.CompareTokenType(TokenType.RwBool)
                ||Utilities.CompareTokenType(TokenType.RwLong)
                ||Utilities.CompareTokenType(TokenType.RwFloat)
                ||Utilities.CompareTokenType(TokenType.RwVoid))
           {
               Utilities.NextToken();
           }
           else
           {
                throw new Exception("A Data Type was expected");
            }
        }

        private void Struct()
        {
            Utilities.NextToken();

            if (!Utilities.CompareTokenType(TokenType.Identifier))
                throw new Exception("Identifier was expected"); 

            Utilities.NextToken();

            if (!Utilities.CompareTokenType(TokenType.OpenCurlyBracket))
                throw new Exception("Openning bracket was expected");

            Utilities.NextToken();

            if (!Utilities.CompareTokenType(TokenType.CloseCurlyBracket))
            {
                DeclarationOfStruct();
            }

            if (!Utilities.CompareTokenType(TokenType.CloseCurlyBracket))
                throw new Exception("Closing bracket was expected");
            Utilities.NextToken();

            if (!Utilities.CompareTokenType(TokenType.EndOfSentence))
                throw new Exception("End of sentence was expected");

            Utilities.NextToken();
        }

        private void DeclarationOfStruct()
        {
            if (!Utilities.CompareTokenType(TokenType.CloseCurlyBracket))
            {
                GeneralDeclaration();

                if (Utilities.CompareTokenType(TokenType.OpenSquareBracket))
                {
                    Utilities.NextToken();
                    _arrays.ArrayIdentifier();
                }
                else if (Utilities.CompareTokenType(TokenType.EndOfSentence))
                {
                    Utilities.NextToken();
                    DeclarationOfStruct();
                }
                else
                {
                    throw new Exception("End of sentence symbol ; expected");
                }
            }
        }

        public void ChooseIdType()
        {
            if (Utilities.CompareTokenType(TokenType.OpBitAnd))
            {
                Utilities.NextToken();

                if (Utilities.CompareTokenType(TokenType.Identifier))
                {
                    Utilities.NextToken();
                }
                else
                {
                    throw new Exception("An Identifier was expected");
                }
            }
            else if (Utilities.CompareTokenType(TokenType.OpMultiplication))
            {
                Utilities.NextToken();

                if (Utilities.CompareTokenType(TokenType.OpMultiplication))
                {
                    IsPointer();
                    Utilities.NextToken();
                }

                if (Utilities.CompareTokenType(TokenType.Identifier))
                {
                    Utilities.NextToken();
                }
                else
                {
                    throw new Exception("An Identifier was expected");
                }
            }
            else if (Utilities.CompareTokenType(TokenType.Identifier))
            {
                Utilities.NextToken();
            }
            else
            {
                throw new Exception("An Identifier was expected");
            }
        }

        private void Declaration()
        {
            GeneralDeclaration();
            TypeOfDeclaration();
        }

        private void TypeOfDeclaration()
        {
            if (Utilities.CompareTokenType(TokenType.OpSimpleAssingment))
            {
                ValueForId();
                if (Utilities.CompareTokenType(TokenType.EndOfSentence))
                {
                    Utilities.NextToken();
                }
                else if (Utilities.CompareTokenType(TokenType.Comma))
                {
                    Functions.MultiDeclaration();
                }
                else
                {
                    throw new Exception("An End of sentence ; symbol was expected");
                }
            }
            else if (Utilities.CompareTokenType(TokenType.Comma))
            {
                Functions.MultiDeclaration();
            }
            else if (Utilities.CompareTokenType(TokenType.OpenSquareBracket))
            {
                _arrays.IsArrayDeclaration();
            }
            else if (Utilities.CompareTokenType(TokenType.OpenParenthesis))
            {
                Functions.IsFunctionDeclaration();
            }
            else if (Utilities.CompareTokenType(TokenType.EndOfSentence))
            {
                Utilities.NextToken();
            }
            else
            {
                throw new Exception("An End of sentence ; symbol was expected");
            }
        }

        public void ListOfExpressions()
        {
            Expressions.Expression();

            if (Utilities.CompareTokenType(TokenType.Comma))
            {
                OptionalExpression();
            }
        }

        private void OptionalExpression()
        {
            if (Utilities.CompareTokenType(TokenType.Comma))
            {
                Utilities.NextToken();
                ListOfExpressions();
            }
            else
            {
                
            }
        }

        private void ValueForId()
        {
            if (Utilities.CompareTokenType(TokenType.OpSimpleAssingment))
            {
                Utilities.NextToken();
                Expressions.Expression();
            }
            else
            {
                
            }
        }

        public void ListOfId()
        {
            Utilities.NextToken();

            if (Utilities.CompareTokenType(TokenType.Identifier))
            {
                Utilities.NextToken();
                OtherIdOrValue();
            }
            else
            {
                throw new Exception("An Identifier was expected");
            }
        }

        private void OtherIdOrValue()
        {
            ValueForId();
            Functions.OptionalId();
        }

        private void GeneralDeclaration()
        {
            DataType();

            //Utilities.NextToken();

            if (Utilities.CompareTokenType(TokenType.OpMultiplication))
            {
                IsPointer();
            }

            if (Utilities.CompareTokenType(TokenType.Identifier))
            {
                Utilities.NextToken();
            }
        }

        private void IsPointer()
        {
            Utilities.NextToken();

            if (Utilities.CompareTokenType(TokenType.OpMultiplication))
            {
                IsPointer();
              // Utilities.NextToken();
            }
            else
            {
                
            }
        }

        private void SpecialDeclaration()
        {
            GeneralDeclaration();
            TypeOfDeclarationForFunction();
        }

        private void TypeOfDeclarationForFunction()
        {
            if (Utilities.CompareTokenType(TokenType.OpSimpleAssingment))
            {
                ValueForId();
                if (Utilities.CompareTokenType(TokenType.EndOfSentence))
                {
                    Utilities.NextToken();
                }
                else if (Utilities.CompareTokenType(TokenType.Comma))
                {
                    Functions.MultiDeclaration();
                }
                else
                {
                    throw new Exception("An End of sentence ; symbol was expected");
                }
            }
            else if (Utilities.CompareTokenType(TokenType.Comma))
            {
                Functions.MultiDeclaration();
            }
            else if (Utilities.CompareTokenType(TokenType.OpenSquareBracket))
            {
                _arrays.IsArrayDeclaration();
            }
            else if (Utilities.CompareTokenType(TokenType.EndOfSentence))
            {
                Utilities.NextToken();
            }
            else
            {
                throw new Exception("An End of sentence ; symbol was expected");
            }
        }
    }
}
