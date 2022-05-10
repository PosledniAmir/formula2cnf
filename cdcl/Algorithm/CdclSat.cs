using dpll.Algorithm;
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
        private readonly LearnedClauseCache _cache;
        private int _restart;

        public int Learned => _clauseChecker.Learned;

        public CdclSat(IFormulaPruner formula) : base(formula)
        {
            _graph = new ImplicationGraph(formula);
            _restart = 100;
            _cache = new LearnedClauseCache(formula);
        }

        public override IEnumerable<IReadOnlyList<int>> GetModels()
        {
            _restart = 100;
            var cont = true;
            var currentDecision = 0;
            while (cont)
            {
                var conflictClause = ExhaustiveResolution();
                if (conflictClause == -1)
                {
                    currentDecision++;
                    if (ShouldRestart(currentDecision))
                    {
                        Restart();
                        currentDecision = 0;
                        continue;
                    }

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
                if (level == -1)
                {
                    cont = false;
                }
                BacktrackAndChoose(level);
            }
        }

        private bool ShouldRestart(int decisions)
        {
            return decisions > _restart && !Satisfied;
        }

        private void Restart()
        {
            _restart *= (int)(_restart * 1.1);
            throw new NotImplementedException();
        }

        private void BacktrackAndChoose(int level)
        {
            for (int i = 0; i < level; i++)
            {
                Backtrack();
            }
            Backtrack();
        }

        private  int LearnClause(int conflictClause)
        {
            if (conflictClause == -1)
            {
                throw new ArgumentException();
            }

            var startLevel = _graph.Level;
            var (learned, level) = _graph.Conflict(conflictClause);
            if (level == 0)
            {
                return -1;
            }

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
                _graph.ByImplication(variable, clause);
                if (!resolution.Success)
                {
                    return resolution.ConflictClause;
                }

                (clause, variable) = GetFirstUnitClause();
            }
            return -1;
        }
    }
}
