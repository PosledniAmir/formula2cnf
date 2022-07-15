using dpll.Algorithm;
using dpll.Runner;
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
        private readonly int _limit;
        private readonly int _cacheLimit;
        private float _mult;

        public int Learned => _clauseChecker.Learned;

        public CdclSat(IFormulaPruner formula) : base(formula)
        {
            _graph = new ImplicationGraph(formula);
            _restart = 1000;
            _limit = 1000;
            _mult = 1.1f;
            _cacheLimit = 1000;
            _cache = new LearnedClauseCache(formula, _cacheLimit);
        }

        public CdclSat(IFormulaPruner formula, int restart, float mult, int cache) : this(formula)
        {
            _restart = restart;
            _limit = restart;
            _mult = mult;
            _cacheLimit = cache;
            _cache = new LearnedClauseCache(formula, _cacheLimit);
        }

        public override IEnumerable<IReadOnlyList<int>> GetModels()
        {
            _restart = _limit;
            _cache.Set(_cacheLimit);
            var cont = true;
            var round = 0;
            while (cont)
            {
                round++;
                if (ShouldRestart(round))
                {
                    Restart();
                    round = 0;
                    continue;
                }

                var conflictClause = ExhaustiveResolution();
                if (conflictClause == -1)
                {
                    var decision = Decision();
                    _graph.Decide(decision.TriedVariable, decision.TriedClause);
                    conflictClause = decision.ConflictClause;

                    if (decision.Success)
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
                else
                {
                    BacktrackAndChoose(level);
                }
            }
        }

        private bool ShouldRestart(int decisions)
        {
            return decisions > _restart && !Satisfied;
        }

        private void Restart()
        {
            _restart = (int)(_restart * _mult);
            _stack.Reset();
            _graph.Restart();
            _cache.Remap(_clauseChecker.Reset(_cache.Reset()));
        }

        private void BacktrackAndChoose(int level)
        {
            for (int i = 0; i < level; i++)
            {
                Backtrack();
            }
            Backtrack();
        }

        private int LearnClause(int conflictClause)
        {
            if (conflictClause == -1)
            {
                throw new ArgumentException();
            }

            var startLevel = _graph.Level;
            var learned = _graph.Conflict(conflictClause);
            if (learned.Level == 0)
            {
                return -1;
            }

            var clause = _clauseChecker.AddClause(learned.Clause);
            _cache.Learned(clause, learned);
            _graph.JumpToLevel(learned.Level);
            return startLevel - learned.Level;
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

        public override SatStats GetStats(TimeSpan elapsed)
        {
            return new SatStats(elapsed, Decisions, Resolutions, Learned);
        }
    }
}
