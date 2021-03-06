﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
    public class ReserverdWords
    {
        public readonly Dictionary<string, TokenType> _keywords;
        public Dictionary<string, TokenType> _operators;
        public Dictionary<string, TokenType> _separators;
        public Dictionary<string, int> _hexLetters;
       // public List<char> _octalNumbers;

        //This one, the especial is used for the operators which are composed by two operators
        public List<string> _specialSymbols;

        public ReserverdWords()
        {
            _keywords = new Dictionary<string, TokenType>();
            _operators = new Dictionary<string, TokenType>();
            _separators = new Dictionary<string,TokenType>();
            _specialSymbols = new List<string>();

            InitializeKeywords();
            InitializeOperators();
            InitializeSeparators();
            InitializeSpecial();
      
        }
      
        private void InitializeSpecial()
        {
            _specialSymbols.Add("<");
            _specialSymbols.Add(">");
            _specialSymbols.Add("=");
            _specialSymbols.Add("&");
            _specialSymbols.Add("|");
            _specialSymbols.Add("+");
            _specialSymbols.Add("-");
            _specialSymbols.Add("/");
            _specialSymbols.Add("*");
            _specialSymbols.Add(">>");
            _specialSymbols.Add("<<");
        }

        private void InitializeSeparators()
        {
            _separators.Add("(", TokenType.OpenParenthesis);
            _separators.Add(")", TokenType.CloseParenthesis);
            _separators.Add("[", TokenType.OpenSquareBracket);
            _separators.Add("]", TokenType.CloseSquareBracket);
            _separators.Add("{", TokenType.OpenCurlyBracket);
            _separators.Add("}", TokenType.CloseCurlyBracket);
            _separators.Add(";", TokenType.EndOfSentence);
            _separators.Add(",", TokenType.Comma);
            _separators.Add(".", TokenType.Dot);
            _separators.Add(":", TokenType.Colon);
        }

        private void InitializeOperators()
        {
            //Arithmetic Operators ,    //Increment and decrement operators
            _operators.Add("+", TokenType.OpAdd);
            _operators.Add("-", TokenType.OpSubstraction);
            _operators.Add("*", TokenType.OpMultiplication);
            _operators.Add("/", TokenType.OpDivision);
            _operators.Add("%", TokenType.OpModule);
            _operators.Add("++", TokenType.OpIncrement);
            _operators.Add("--", TokenType.OpDecrement);

            //Relational Operators
            _operators.Add("<", TokenType.OpLessThan);
            _operators.Add("<=", TokenType.OpLessThanOrEqualTo);
            _operators.Add(">", TokenType.OpGreaterThan);
            _operators.Add(">=", TokenType.OpGreaterThanOrEqualTo);
            _operators.Add("==", TokenType.OpEqualTo);
            _operators.Add("!=", TokenType.OpNotEqualTo);
            _operators.Add("->", TokenType.OpPointerStructs);

            //special for html
            //_operators.Add("<%", TokenType.OpenCCode);
            _operators.Add("%>", TokenType.CloseCCode);

            //Logical Operators
            _operators.Add("&&", TokenType.OpAnd);
            _operators.Add("||", TokenType.OpLogicalOr);
            _operators.Add("!", TokenType.OpNot);

            //Asignament Operators
            _operators.Add("=", TokenType.OpSimpleAssignment);
            _operators.Add("+=", TokenType.OpAddAndAssignment);
            _operators.Add("-=", TokenType.OpSusbtractAndAssignment);
            _operators.Add("*=", TokenType.OpMultiplyAndAssignment);
            _operators.Add("/=", TokenType.OpDivideAssignment);
            _operators.Add("%=", TokenType.OpModulusAssignment);
            _operators.Add("<<=", TokenType.OpBitShiftLeftAndAssignment);
            _operators.Add(">>=", TokenType.OpBitShiftRightAndAssignment);
            _operators.Add("&=", TokenType.OpBitwiseAndAssignment);
            _operators.Add("^=", TokenType.OpBitwiseXorAndAssignment);
            _operators.Add("|=", TokenType.OpBitwiseInclusiveOrAndAssignment);

            //Bitwise Operators
            _operators.Add("&", TokenType.OpBitAnd);
            _operators.Add("^", TokenType.OpBitXor);
            _operators.Add("~", TokenType.OpComplement);
            _operators.Add("<<", TokenType.OpBitShiftLeft);
            _operators.Add(">>", TokenType.OpBitShiftRight);
            _operators.Add("|", TokenType.OpBitOr);
            _operators.Add("?",TokenType.ConditionalExpression);
        }

        private void InitializeKeywords()
        {
           //C language reserved words
            _keywords.Add("auto",TokenType.RwAuto);
            _keywords.Add("break", TokenType.RwBreak);
            _keywords.Add("case", TokenType.RwCase);
            _keywords.Add("char", TokenType.RwChar);
            _keywords.Add("string", TokenType.RwString);
            _keywords.Add("continue", TokenType.RwContinue);
            _keywords.Add("do", TokenType.RwDo);
            _keywords.Add("default", TokenType.RwDefault);
            _keywords.Add("const", TokenType.RwConst);
            _keywords.Add("double", TokenType.RwDouble);
            _keywords.Add("else", TokenType.RwElse);
            _keywords.Add("enum", TokenType.RwEnum);
            _keywords.Add("extern", TokenType.RwExtern);
            _keywords.Add("for", TokenType.RwFor);
            _keywords.Add("foreach", TokenType.RwForEach);
            _keywords.Add("if", TokenType.RwIf);
            _keywords.Add("goto", TokenType.RwGoto);
            _keywords.Add("float", TokenType.RwFloat);
            _keywords.Add("int", TokenType.RwInt);
            _keywords.Add("long", TokenType.RwLong);
            _keywords.Add("register", TokenType.RwRegister);
            _keywords.Add("return", TokenType.RwReturn);
            _keywords.Add("signed", TokenType.RwSigned);
            _keywords.Add("static", TokenType.RwStatic);
           // _keywords.Add("sizeof", TokenType.RwSizeOf);
            _keywords.Add("short", TokenType.RwShort);
            _keywords.Add("struct", TokenType.RwStruct);
            _keywords.Add("switch",TokenType.RwSwitch);
            _keywords.Add("typedef", TokenType.RwTypedef);
            _keywords.Add("union", TokenType.RwUnion);
            _keywords.Add("void", TokenType.RwVoid);
            _keywords.Add("while", TokenType.RwWhile);
            _keywords.Add("volatile", TokenType.RwVolatile);
            _keywords.Add("unsigned", TokenType.RwUnsigned);
            _keywords.Add("date", TokenType.RwDate);
            _keywords.Add("#include", TokenType.RwInclude);
            _keywords.Add("bool", TokenType.RwBool);
            _keywords.Add("true", TokenType.RwTrue);
            _keywords.Add("false", TokenType.RwFalse);
        }
    }
}
