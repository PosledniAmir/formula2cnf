﻿using cryptoarithmetics.Parsing;
using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptoarithmetics
{
    internal sealed class ConditionGenerator
    {
        private readonly FormulaBuilder _builder;
        private readonly ParserResult _result;
        public readonly IReadOnlyDictionary<string, IntExpr> Constants;

        public ConditionGenerator(FormulaBuilder builder, ParserResult result)
        {
            _builder = builder;
            _result = result;
            Constants = CreateConstants(_builder, _result.Words);
        }

        private BoolExpr CreateUniqueCondition(IEnumerable<KeyValuePair<string, IntExpr>> constants)
        {
            return _builder.CreateUniqueConditon(constants);
        }

        public IEnumerable<BoolExpr> CreateRangeConditions()
        {
            return Constants.Select(pair => _builder.CreateBaseCondition(pair.Value, _result.FirstLetters.Contains(pair.Key)));
        }

        private BoolExpr CreateClauseCondition(bool unique, IReadOnlyList<Tuple<Token, string>> words)
        {
            BoolExpr result = null;
            var acc = _builder.CreateWordCondition(words.First().Item2, Constants);

            for (int i = 1; i < words.Count - 1; i += 2)
            {
                var op = words[i].Item1;
                var next = _builder.CreateWordCondition(words[i + 1].Item2, Constants);
                if (op == Token.Plus)
                {
                    acc = _builder.CreateWordOperation(FormulaBuilder.Operation.Plus, acc, next);
                }
                else if (op == Token.Minus)
                {
                    acc = _builder.CreateWordOperation(FormulaBuilder.Operation.Minus, acc, next);
                }
                else if (op == Token.Equal)
                {
                    result = _builder.CreateEquality(acc, next);
                }
                else
                {
                    throw new ArgumentException("Expected operator.");
                }
            }

            if (result == null)
            {
                throw new ArgumentException("Expected = operator.");
            }

            if (unique)
            {
                var constants = words
                    .SelectMany(pair => pair.Item2)
                    .Select(c => c.ToString())
                    .Distinct()
                    .Where(c => Constants.ContainsKey(c))
                    .Select(c => new KeyValuePair<string, IntExpr>(c, Constants[c]));
                result = _builder.CreateClauseOperation(FormulaBuilder.ClauseOperation.And, result, _builder.CreateUniqueConditon(constants));
            }

            return result;
        }

        public BoolExpr CreateInstanceCondition(bool unique)
        {
            var clauses = new List<List<Tuple<Token, string>>>();
            var temp = new List<Tuple<Token, string>>();

            foreach (var (token, word) in _result.Tokens)
            {
                if (token == Token.And || token == Token.Or)
                {
                    clauses.Add(temp);
                    temp = new List<Tuple<Token, string>>();
                    clauses.Add(new List<Tuple<Token, string>> { new Tuple<Token, string>(token, word) });
                }
                else if (token == Token.LeftBracket || token == Token.RightBracket)
                {
                    continue;
                }
                else
                {
                    temp.Add(Tuple.Create(token, word));
                }
            }

            clauses.Add(temp);
            temp = new List<Tuple<Token, string>>();

            var acc = CreateClauseCondition(unique, clauses.First());

            for (int i = 1; i < clauses.Count - 1; i += 2)
            {
                if (clauses[i].Count != 1)
                {
                    throw new ArgumentException("Expected operator.");
                }
                var op = clauses[i].First().Item1;
                var next = CreateClauseCondition(unique, clauses[i+1]);
                if (op == Token.And) 
                {
                    acc = _builder.CreateClauseOperation(FormulaBuilder.ClauseOperation.And, acc, next);
                }
                else if (op == Token.Or) 
                {
                    acc = _builder.CreateClauseOperation(FormulaBuilder.ClauseOperation.Or, acc, next);
                }
                else 
                { 
                    throw new ArgumentException("Expected operator."); 
                }
            }

            return acc;
        }

        private static Dictionary<string, IntExpr> CreateConstants(FormulaBuilder builder, IEnumerable<string> words)
        {
            return words
                .SelectMany(word => word.AsEnumerable())
                .ToHashSet()
                .Select(c => c.ToString())
                .ToDictionary(key => key, item => builder.CreateConstant(item));
        }
    }
}
