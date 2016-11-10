using System;
using Lexer;

namespace Syntax
{
    public class Arrays
    {
        private Parser _parser;

        public Arrays(Parser parser)
        {
            _parser = parser;
        }

        public void IsArrayDeclaration()
        {
            if (!_parser.Utilities.CompareTokenType(TokenType.OpenSquareBracket))
                throw new Exception("An openning bracket [ symbol was expected");

            _parser.Utilities.NextToken();

            bool hasSize;
            bool isUnidimensional = true;

            SizeForArray(out hasSize);

            if (!_parser.Utilities.CompareTokenType(TokenType.CloseSquareBracket))
                throw new Exception("An closing bracket ] symbol was expected");

            _parser.Utilities.NextToken();

            if (_parser.Utilities.CompareTokenType(TokenType.OpenSquareBracket))
            {
                BidArray(out isUnidimensional);
            }
        
            if (_parser.Utilities.CompareTokenType(TokenType.OpSimpleAssignment) && !isUnidimensional)
            {
                OptionalInitOfArray();
            }
            
            if ((_parser.Utilities.CompareTokenType(TokenType.EndOfSentence) && hasSize && isUnidimensional))
            {
                _parser.Utilities.NextToken();
            }
            else if (_parser.Utilities.CompareTokenType(TokenType.EndOfSentence) && !isUnidimensional)
            {
                _parser.Utilities.NextToken();
            }
            else if (isUnidimensional && _parser.Utilities.CompareTokenType(TokenType.OpSimpleAssignment))
            {
                if (!_parser.Utilities.CompareTokenType(TokenType.OpSimpleAssignment))
                {
                    throw new Exception("An assignment symbol was expected");
                }

                OptionalInitOfArray();
            }
            else if (isUnidimensional && !hasSize)
            {
                if (!_parser.Utilities.CompareTokenType(TokenType.OpSimpleAssignment))
                {
                    throw new Exception("An assignment symbol was expected");
                }

                OptionalInitOfArray();
            }
            else if (_parser.Utilities.CompareTokenType(TokenType.Comma))
            {
                 _parser.OptionalExpression();

                if (!_parser.Utilities.CompareTokenType(TokenType.EndOfSentence))
                {
                    throw new Exception("An End of sentence ; symbol was expected");
                }

                _parser.Utilities.NextToken();
            }
            else
            {
                throw new Exception("An End of sentence ; symbol was expected");
            }

        }

        private void OptionalInitOfArray()
        {
            _parser.Utilities.NextToken();
            if (_parser.Utilities.CompareTokenType(TokenType.OpenCurlyBracket))
            {
                _parser.Utilities.NextToken();
                _parser.ListOfExpressions();
                if (_parser.Utilities.CompareTokenType(TokenType.CloseCurlyBracket))
                   _parser.Utilities.NextToken();
            }

            if (!_parser.Utilities.CompareTokenType(TokenType.EndOfSentence))
            {
                throw new Exception("End of sentence was expected");
            }
            _parser.Utilities.NextToken();
        }

        public void BidArray(out bool isUnidimensional)
        {
            if (!_parser.Utilities.CompareTokenType(TokenType.OpenSquareBracket))
                throw new Exception("An openning bracket [ symbol was expected");

            _parser.Utilities.NextToken();

            SizeForBidArray();

            if (_parser.Utilities.CompareTokenType(TokenType.CloseSquareBracket))
            {
                _parser.Utilities.NextToken();
            }
            else
            {
                throw new Exception("An closing bracket ] symbol was expected");
            }

            isUnidimensional = false;
        }

        public void SizeForBidArray()
        {
            if (_parser.Utilities.CompareTokenType(TokenType.LiteralNumber) 
                ||_parser.Utilities.CompareTokenType(TokenType.LiteralOctal)
                || _parser.Utilities.CompareTokenType(TokenType.LiteralHexadecimal) 
                ||_parser.Utilities.CompareTokenType(TokenType.Identifier))
            {
                //Para casos como  sum += arr[i++];   
                if (_parser.Utilities.CompareTokenType(TokenType.Identifier)    )
                {
                    _parser.Expressions.Expression();
                }
                else
                {
                    _parser.Utilities.NextToken();
                }
              
            }
            else
            {
                throw new Exception("Initialization of array is required");
            }
        }

        private void SizeForArray(out bool hasSize)
        {
            if (_parser.Utilities.CompareTokenType(TokenType.LiteralNumber) || _parser.Utilities.CompareTokenType(TokenType.LiteralOctal)
                || _parser.Utilities.CompareTokenType(TokenType.LiteralHexadecimal) || _parser.Utilities.CompareTokenType(TokenType.Identifier))
            {
                _parser.Utilities.NextToken();
                hasSize = true;
            }
            else
            {
                hasSize = false;
            }
        }

        public void ArrayIdentifier()
        {
            SizeForArray();

            if (!_parser.Utilities.CompareTokenType(TokenType.CloseSquareBracket))
            {
                throw new Exception("An closing bracket ] symbol was expected");
            }
            else
            {
                
            }
        }

        private void SizeForArray()
        {
            if (_parser.Utilities.CompareTokenType(TokenType.LiteralNumber)
                || _parser.Utilities.CompareTokenType(TokenType.LiteralOctal)
                || _parser.Utilities.CompareTokenType(TokenType.LiteralHexadecimal) 
                || _parser.Utilities.CompareTokenType(TokenType.Identifier))
            {
                _parser.Utilities.NextToken();
              
            }
            else
            {
            }
        }
    }
}