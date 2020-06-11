using System;
using System.Collections.Generic;
using System.Linq;
using DND.Default;
using DND.Parser;
using NUnit.Framework;

namespace DND.Test
{
    public class TokenParserTests
    {
        ITokenParser _parser;
        LiveParser _liveParser;

        [SetUp]
        public void Setup()
        {
            _parser = new TokenParser(
                new[] {new LinkTokenType()});
            _liveParser = new LiveParser(_parser);
        }

        const string TestOne = "this string has no tokens";
        const string TestTwo = "this string has {one} token located at position 16";
        const string TestThree = "this {string} has two {tokens} located at positions 5 and 22";
        const string TestFour = "{this} string starts with a token";
        const string TestFive = "this string ends with a {token}";
        const string TestSix = "this has an open token at the {end";
        const string TestSeven = "this is a {test} of the {";
        const string TestEight = "so {long} as we{don't} fuck it up {";

        [Test]
        public void Test1()
        {
            Test(TestOne, 0);
        }

        [Test]
        public void Test2()
        {
            Test(TestTwo, 1, new[] {16}, new[] {"{one}"}, new[] {"one"});
        }

        [Test]
        public void Test3()
        {
            Test(TestThree, 2, new[] {5, 22}, new[] {"{string}", "{tokens}"}, new[] {"string", "tokens"});
        }

        [Test]
        public void Test4()
        {
            Test(TestFour, 1, new[] {0}, new[] {"{this}"}, new[] {"this"});
        }

        [Test]
        public void Test5()
        {
            Test(TestFive, 1, new[] {24}, new[] {"{token}"}, new[] {"token"});
        }

        [Test]
        public void Test6()
        {
            Test(TestSix, 1, new[] {30}, new[] {"{end"}, new[] {"{end"},
                tokens => Assert.True(tokens.All(p => p.IsOpen)));
        }

        [Test]
        public void Test7()
        {
            Test(TestSeven, 2, new[] {10, 24}, new[] {"{test}", "{"}, new[] {"test", "{"},
                tokens => Assert.True(tokens.LastOrDefault()?.IsOpen ?? false));
        }

        [Test]
        public void Test8()
        {
            Test(TestEight, 3, new []{3, 15, 34}, new []{"{long}", "{don't}", "{"}, new []{"long", "don't", "{"},
                tokens =>
                {
                    var a = tokens.ToArray();
                    Assert.AreEqual(3, a.Length);
                    Assert.AreEqual("long", a[0].Value);
                    Assert.AreEqual("don't", a[1].Value);
                    Assert.AreEqual("{", a[2].Value);
                });
        }

    void Test(string val, int count, int[] positions = null, string[] rawValues = null, string[] values = null, Action<IEnumerable<Token>> additionalTest = null)
        {
            TestLiveParser(val, count, positions, rawValues, values, additionalTest);
            TestParser(val, count, positions, rawValues, values, additionalTest);
        }

        void TestLiveParser(string input, int count, int[] positions = null, string[] rawValues = null, string[] values = null, Action<IEnumerable<Token>> additionalTest = null)
        {
            var parser = new LiveParser(_parser);
            parser.SetText(input);
            Assert.AreEqual(count, parser.Tokens.Count);
            if (count == 0)
                return;
            Assert.AreEqual(positions.Length, parser.Tokens.Count);
            for (int i = 0; i < parser.Tokens.Count; i++)
                Test(parser.Tokens[i], positions[i], rawValues[i], values[i]);

            additionalTest?.Invoke(parser.Tokens);
        }

        void TestParser(string input, int count, int[] positions = null, string[] rawValues = null, string[] values = null, Action<IEnumerable<Token>> additionalTest = null)
        {
            var tokens = _parser.Parse(input).ToArray();
            Assert.AreEqual(count, tokens.Length);
            if (count == 0)
                return;
            for (int i = 0; i < tokens.Length; i++)
                Test(tokens[i], positions[i], rawValues[i], values[i]);

            additionalTest?.Invoke(tokens);
        }

        void Test(Token token, int position, string rawValue, string value)
        {
            Assert.AreEqual(position, token.Position);
            Assert.AreEqual(rawValue, token.RawValue);
            Assert.AreEqual(value, token.Value);
        }
    }
}