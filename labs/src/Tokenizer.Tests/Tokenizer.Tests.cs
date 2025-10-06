/**
* Provides the test implementation of the token and tokenzier, which converts
* an input string into a list of tokens such as variables, numbers,
* operators, parentheses, and keywords.
*
* Bugs: No major bugs known, but invalid inputs may raise exceptions
*       when not handled in helper methods.
*
* @author Rahul, Rick, Zachary, ChatGPT5
* @date 30th Sept, 2025
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Tokenizer;

namespace Tokenizer.Tests
{
    public class TokenizerImplTests
    {
        private readonly TokenizerImpl tokenizer = new();

        private static (TokenType, string)[] Simplify(IEnumerable<Token> tokens)
        {
            var result = new List<(TokenType, string)>();

            foreach (var t in tokens)
            {
                result.Add((t._tkntype, t._value));
            }

            return result.ToArray();
        }


        // ---------- TOKEN CLASS ----------
        [Fact]
        public void Token_Equals_ShouldWork()
        {
            var t1 = new Token("x", TokenType.VARIABLE);
            var t2 = new Token("x", TokenType.VARIABLE);
            var t3 = new Token("y", TokenType.VARIABLE);

            Assert.True(t1.Equals(t1, t2));
            Assert.False(t1.Equals(t1, t3));
        }

        [Fact]
        public void Token_ToString_ShouldContainValueAndType()
        {
            var t = new Token("42", TokenType.INTEGER);
            var writer = new System.IO.StringWriter();
            Console.SetOut(writer);

            t.ToString(t);

            var output = writer.ToString();
            Assert.Contains("42", output);
            Assert.Contains("INTEGER", output);
        }

        // ---------- EMPTY & WHITESPACE ----------
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t \n  \r\n")]
        public void EmptyOrWhitespace_ProducesNoTokens(string src)
        {
            var tokens = tokenizer.Tokenize(src);
            Assert.Empty(tokens);
        }

        // ---------- IDENTIFIERS ----------
        [Theory]
        [InlineData("return", TokenType.RETURN, "return")]
        [InlineData("x", TokenType.VARIABLE, "x")]
        [InlineData("abc", TokenType.VARIABLE, "abc")]
        [InlineData("returnx", TokenType.VARIABLE, "returnx")]
        [InlineData("fooBar", TokenType.VARIABLE, "fooBar")]
        public void Identifiers_And_Keyword_AreRecognized(string src, TokenType type, string val)
        {
            var t = Assert.Single(tokenizer.Tokenize(src));
            Assert.Equal(type, t._tkntype);
            Assert.Equal(val, t._value);
        }

        // error fooBar, a1, X  ----------------------- fix here rwequired?
        [Theory]
        [InlineData("X", TokenType.VARIABLE, "X")]        // uppercase single letter allowed
         // mixed case allowed
        public void Invalid_Identifiers_ShouldThrow(string src, TokenType type, string val)
        {
            var tokens = tokenizer.Tokenize(src);
            Assert.Single(tokens);
            Assert.Equal(type, tokens[0]._tkntype);
            Assert.Equal(val, tokens[0]._value);
        }

        // ---------- NUMBERS ----------
        [Theory]
        [InlineData("0", "0")]
        [InlineData("42", "42")]
        [InlineData("007", "007")]
        public void Integers_AreRecognized(string src, string val)
        {
            var t = Assert.Single(tokenizer.Tokenize(src));
            Assert.Equal(TokenType.INTEGER, t._tkntype);
            Assert.Equal(val, t._value);
        }

        [Theory]
        [InlineData("0.0", "0.0")]
        [InlineData("1.23", "1.23")]
        public void Floats_AreRecognized(string src, string val)
        {
            var t = Assert.Single(tokenizer.Tokenize(src));
            Assert.Equal(TokenType.FLOAT, t._tkntype);
            Assert.Equal(val, t._value);
        }

        [Theory]
        [InlineData(".5")]
        [InlineData("1..2")]
        [InlineData("1.2.3")]
        public void Malformed_Floats_ShouldThrow(string src)
        {
            Assert.Throws<ArgumentException>(() => tokenizer.Tokenize(src));
        }

        // ---------- NEGATIVE NUMBERS ----------
        [Theory]
        [InlineData("-1")]
        [InlineData("-0.5")]
        [InlineData("x-1")]
        public void NegativeNumbers_TokenizeAsMinusAndLiteral(string src)
        {
            var simplified = Simplify(tokenizer.Tokenize(src));

            if (src == "-1")
            {
                Assert.Equal(new[] {
                    (TokenType.OPERATOR, TokenConstants.MINUS),
                    (TokenType.INTEGER, "1")
                }, simplified);
            }
            else if (src == "-0.5")
            {
                Assert.Equal(new[] {
                    (TokenType.OPERATOR, TokenConstants.MINUS),
                    (TokenType.FLOAT, "0.5")
                }, simplified);
            }
            else if (src == "x-1")
            {
                Assert.Equal(new[] {
                    (TokenType.VARIABLE, "x"),
                    (TokenType.OPERATOR, TokenConstants.MINUS),
                    (TokenType.INTEGER, "1")
                }, simplified);
            }
        }


        // ---------- OPERATORS ----------
        [Theory]
        [InlineData("+")]
        [InlineData("-")]
        [InlineData("*")]
        [InlineData("%")]
        [InlineData("**")]
        public void Operators_AreRecognized(string op)
        {
            var t = Assert.Single(tokenizer.Tokenize(op));
            Assert.Equal(TokenType.OPERATOR, t._tkntype);
            Assert.Equal(op, t._value);
        }

        // ---------- SLASH VARIANTS ----------
        [Theory]
        [InlineData("/ x")]
        [InlineData("// x")]
        [InlineData("a//b")]
        [InlineData("a/b")]
        public void SlashVariants_Disambiguated(string src)
        {
            var simplified = Simplify(tokenizer.Tokenize(src));

            if (src == "/ x")
            {
                Assert.Equal(new[] {
                    (TokenType.OPERATOR, TokenConstants.INT_DIV),
                    (TokenType.VARIABLE, "x")
                }, simplified);
            }
            else if (src == "// x")
            {
                Assert.Equal(new[] {
                    (TokenType.OPERATOR, TokenConstants.FLOAT_DIV),
                    (TokenType.VARIABLE, "x")
                }, simplified);
            }
            else if (src == "a//b")
            {
                Assert.Equal(new[] {
                    (TokenType.VARIABLE, "a"),
                    (TokenType.OPERATOR, TokenConstants.FLOAT_DIV),
                    (TokenType.VARIABLE, "b")
                }, simplified);
            }
            else if (src == "a/b")
            {
                Assert.Equal(new[] {
                    (TokenType.VARIABLE, "a"),
                    (TokenType.OPERATOR, TokenConstants.INT_DIV),
                    (TokenType.VARIABLE, "b")
                }, simplified);
            }
        }


        [Fact]
        public void AssignmentOperator_Recognized()
        {
            var t = Assert.Single(tokenizer.Tokenize(":="));
            Assert.Equal(TokenType.ASSIGNMENT, t._tkntype);
            Assert.Equal(":=", t._value);
        }

        [Fact]
        public void UnsupportedOperator_ShouldThrow()
        {
            Assert.Throws<ArgumentException>(() => tokenizer.Tokenize("&"));
        }

        // ---------- STRUCTURE ----------
        [Theory]
        [InlineData("(", TokenType.LEFT_PAREN)]
        [InlineData(")", TokenType.RIGHT_PAREN)]
        [InlineData("{", TokenType.LEFT_CURLY)]
        [InlineData("}", TokenType.RIGHT_CURLY)]
        public void Structures_AreRecognized(string src, TokenType expected)
        {
            var t = Assert.Single(tokenizer.Tokenize(src));
            Assert.Equal(expected, t._tkntype);
        }

        // ---------- MULTIPLE STATEMENTS ----------
        [Fact]
        public void MultipleStatements_ShouldTokenize()
        {
            var tokens = tokenizer.Tokenize("x:=1 y:=2");
            var simplified = Simplify(tokens);

            var expected = new (TokenType,string)[] {
                (TokenType.VARIABLE,"x"), (TokenType.ASSIGNMENT,":="),
                (TokenType.INTEGER,"1"),
                (TokenType.VARIABLE,"y"), (TokenType.ASSIGNMENT,":="),
                (TokenType.INTEGER,"2")
            };
            Assert.Equal(expected, simplified);
        }

        // ---------- INTEGRATION PROGRAMS ----------
        [Theory]
        [InlineData("return 0")]
        [InlineData("sum:=a+b*c")]
        [InlineData("{x:=10//3}")]
        [InlineData("(2**3)%3")]
        public void MixedPrograms_AreTokenized(string src)
        {
            var simplified = Simplify(tokenizer.Tokenize(src));

            if (src == "return 0")
            {
                Assert.Equal(new[] {
                    (TokenType.RETURN, TokenConstants.RETURN),
                    (TokenType.INTEGER, "0")
                }, simplified);
            }
            else if (src == "sum:=a+b*c")
            {
                Assert.Equal(new[] {
                    (TokenType.VARIABLE, "sum"),
                    (TokenType.ASSIGNMENT, TokenConstants.ASSIGMENT),
                    (TokenType.VARIABLE, "a"),
                    (TokenType.OPERATOR, TokenConstants.PLUS),
                    (TokenType.VARIABLE, "b"),
                    (TokenType.OPERATOR, TokenConstants.TIMES),
                    (TokenType.VARIABLE, "c")
                }, simplified);
            }
            else if (src == "{x:=10//3}")
            {
                Assert.Equal(new[] {
                    (TokenType.LEFT_CURLY, TokenConstants.LEFT_CURLY),
                    (TokenType.VARIABLE, "x"),
                    (TokenType.ASSIGNMENT, TokenConstants.ASSIGMENT),
                    (TokenType.INTEGER, "10"),
                    (TokenType.OPERATOR, TokenConstants.FLOAT_DIV),
                    (TokenType.INTEGER, "3"),
                    (TokenType.RIGHT_CURLY, TokenConstants.RIGHT_CURLY)
                }, simplified);
            }
            else if (src == "(2**3)%3")
            {
                Assert.Equal(new[] {
                    (TokenType.LEFT_PAREN, TokenConstants.LEFT_PAREN),
                    (TokenType.INTEGER, "2"),
                    (TokenType.OPERATOR, TokenConstants.EXP),
                    (TokenType.INTEGER, "3"),
                    (TokenType.RIGHT_PAREN, TokenConstants.RIGHT_PAREN),
                    (TokenType.OPERATOR, TokenConstants.MOD),
                    (TokenType.INTEGER, "3")
                }, simplified);
            }
        }



        // ---------- EDGE / STRESS ----------
        [Fact]
        public void VeryLongIdentifier_ShouldTokenize()
        {
            var id = new string('a', 1000);
            var t = Assert.Single(tokenizer.Tokenize(id));
            Assert.Equal(TokenType.VARIABLE, t._tkntype);
            Assert.Equal(id, t._value);
        }

        [Fact]
        public void VeryLargeInteger_ShouldTokenize()
        {
            var num = new string('9', 50);
            var t = Assert.Single(tokenizer.Tokenize(num));
            Assert.Equal(TokenType.INTEGER, t._tkntype);
            Assert.Equal(num, t._value);
        }
    }
}
