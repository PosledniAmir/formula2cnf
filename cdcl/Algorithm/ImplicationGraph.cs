﻿using dpll.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cdcl.Algorithm
{
    internal sealed class ImplicationGraph
    {
        private readonly Stack<DecisionTrail> _trail;
        private readonly Dictionary<int, int> _levels;
        private readonly IFormulaPruner _formula;

        public int Level => _trail.Count;

        public ImplicationGraph(IFormulaPruner formula)
        {
            _trail = new Stack<DecisionTrail>();
            _formula = formula;
            _levels = new Dictionary<int, int>();
        }

        public void Decide(int variable, int clause)
        {
            _trail.Push(new DecisionTrail(variable, clause));
            _levels[variable] = _trail.Count;
        }

        public void ByImplication(int variable, int clause)
        {
            if (_trail.Count > 0)
            {
                _trail.Peek().Implication(variable, clause);
            }
            _levels[variable] = _trail.Count;
        }

        private DecisionTrail GetConflict(int level)
        {
            var count = (_trail.Count + 1) - level;
            var result = _trail.Pop();

            for (int i = 1; i < count; i++)
            {
                result = _trail.Pop();
            }

            return result;
        }

        public Tuple<HashSet<int>, int> Conflict(int clause)
        {
            var uip = new HashSet<int>();
            var rest = new HashSet<int>();
            var literals = _formula.Literals(clause).ToList();
            var level = literals.Select(l => _levels[-l]).Max();
            var step = GetConflict(level);

            do
            {
                var responsible = literals.Where(l => _levels.TryGetValue(-l, out var v) && v == level);
                var outsiders = literals.Where(l => _levels.TryGetValue(-l, out var v) && v != level);
                Merge(uip, responsible);
                Merge(rest, outsiders);
                int variable;
                (variable, clause) = step.Pop();
                literals = _formula.Literals(clause).ToList();
                uip.Remove(-variable);
            } while (uip.Count != 1);

            var result =  BuildClause(uip.First(), rest);
            level = result.Select(l => _levels[-l]).Min();
            if (level == 0)
            {
                level = 1;
            }

            return Tuple.Create(result, level);
        }

        private void Merge(HashSet<int> set, IEnumerable<int> items)
        {
            foreach (var item in items)
            {
                set.Add(item);
            }
        }

        private HashSet<int> BuildClause(int variable, HashSet<int> rest)
        {
            var result = new HashSet<int>
            {
                variable
            };
            foreach (var item in rest)
            {
                if (_levels[-item] == 0)
                {
                    continue;
                }
                else if (!result.Contains(-item))
                {
                    result.Add(item);
                }
                else
                {
                    result.Remove(-item);
                }
            }

            return result;
        }

        public void JumpToLevel(int level)
        {
            var toRemove = _levels.Where(k => k.Value >= level).ToList();
            foreach(var item in toRemove)
            {
                _levels.Remove(item.Key);
            }

            while (_trail.Count >= level)
            {
                _trail.Pop();
            }
        }
    }
}
