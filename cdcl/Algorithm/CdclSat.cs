﻿using dpll.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using watched.Algorithm;

namespace cdcl.Algorithm
{
    internal sealed class CdclSat : AbstractSat
    {
        private readonly ImplicationGraph _graph;

        public CdclSat(IFormulaPruner formula) : base(formula)
        {
            _graph = new ImplicationGraph(formula);
        }

        public override IEnumerable<IReadOnlyList<int>> GetModels()
        {
            var cont = true;
            while (cont)
            {
                var conflictClause = ExhaustiveResolution();
                if (conflictClause == -1)
                {
                    var decisions = Decision();
                    conflictClause = LearnDecisions(decisions);
                    if (Success(decisions))
                    {
                        continue;
                    }
                    else if (Satisfied)
                    {
                        yield return GetModel().ToList();
                    }
                }

                var level = LearnClause(conflictClause);
                if (!BacktrackAndChoose(level))
                {
                    cont = false;
                }
            }
        }

        private bool BacktrackAndChoose(int level)
        {
            for (int i = 0; i < level + 1; i++)
            {
                Backtrack();
            }

            return true;
        }

        private int LearnClause(int conflictClause)
        {
            if (conflictClause == -1)
            {
                throw new ArgumentException();
            }

            var startLevel = _graph.Level;
            var (learned, level) = _graph.Conflict(conflictClause);
            var clause = _clauseChecker.AddClause(learned);
            _graph.JumpToLevel(level);
            return startLevel - level;
        }

        private int LearnDecisions(List<Outcome> decisions)
        {
            foreach (var decision in decisions)
            {
                _graph.Decide(decision.TriedVariable, decision.TriedClause);
            }

            return decisions[^1].ConflictClause;
        }

        private int ExhaustiveResolution()
        {
            var times = 0;
            var (clause, variable) = GetFirstUnitClause();
            while (variable != 0)
            {
                times++;
                var resolution = Resolution(clause, variable);               
                if (!resolution.Success)
                {
                    _graph.ByImplication(variable, clause);
                    return resolution.ConflictClause;
                }

                (clause, variable) = GetFirstUnitClause();
            }
            return -1;
        }
    }
}
