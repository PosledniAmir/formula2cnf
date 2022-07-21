# Decision procedures and verification
This github repository serves as a repository for programming exercises for the NAIL094 — Decision procedures and verification. The program is written in C#10, the target framework is .NET6.
# Tseitin Encoding and DIMACS Format
For the code check the project named `formula2cnf`.
```
formula2cnf usage: formula2cnf [input [output]] [--implication | -i].
--implication or -i option will use implications in the encoding instead of equivalence.
```
# DPLL Algorithm
For the code check the project named `dpll`.
```
dpll usage: dpll [input] [--sat | -s | --cnf | -c]
if the format cannot be read from file extension you can specify the format using --sat | -s for smt-lib, --cnf | -c for dimacs
```
## Dpll performance
We will try several examples from the https://www.cs.ubc.ca/~hoos/SATLIB/benchm.html from the `Uniform Random-3-SAT` and the `"Flat" Graph Colouring` category. For each category we will try 5 SAT and 5 UNSAT examples.
## Benchmark table
| Benchmark | Variables | Clauses | Sat   | Min        | Mean        | Max         |
|-----------|-----------|---------|-------|------------|-------------|-------------|
| 3SAT      | 20        | 91      | true  | 0.2442     | 1.8607      | 6.7692      |
| 3SAT      | 50        | 218     | false | 15.0567    | 19.5462     | 24.9693     |
| 3SAT      | 50        | 218     | true  | 1.0584     | 20.3334     | 37.4989     |
| 3SAT      | 75        | 325     | false | 258.0931   | 398.5186    | 644.8791    |
| 3SAT      | 75        | 325     | true  | 17.228     | 174.0889    | 296.0046    |
| 3SAT      | 100       | 430     | false | 2303.8816  | 6841.0832   | 13121.4587  |
| 3SAT      | 100       | 430     | true  | 453.7044   | 2233.5318   | 5511.9429   |
| 3SAT      | 125       | 538     | false | 25865.1285 | 272273.6085 | 771999.6572 |
| 3SAT      | 125       | 538     | true  | 1735.1002  | 11245.5815  | 35834.0529  |
| 3SAT      | 150       | 645     | true  | 6697.556   | 186992.5894 | 372412.7886 |
| FLAT      | 30        | 60      | true  | 0.6394     | 0.7706      | 1.0698      |
| FLAT      | 50        | 115     | true  | 1.3782     | 1.5375      | 2.0261      |
| FLAT      | 75        | 180     | true  | 6.6596     | 81.653      | 175.3035    |
| FLAT      | 100       | 239     | true  | 4.1465     | 4.7234      | 5.9539      |
| FLAT      | 125       | 301     | true  | 83.7987    | 12886.8551  | 53932.9505  |

The min, mean and max columns are in milliseconds. We can see that time needed to solve a formula goes up very fast. As we look at each satisfiable category in the table for 3SAT we can see that the time needed to solve it goes up roughly ten times.
Interesting is, that sometimes the DPLL works fast on the `FLAT` formulas.
# DPLL with watched literals
For the code check the project named `watched`.
```
dpll usage: watched [input] [--sat | -s | --cnf | -c]
if the format cannot be read from file extension you can specify the format using --sat | -s for smt-lib, --cnf | -c for dimacs
```
## Dpll with watched literals performance
We will try several examples from the https://www.cs.ubc.ca/~hoos/SATLIB/benchm.html from the `Uniform Random-3-SAT` and the `"Flat" Graph Colouring` category. For each category we will try 5 SAT and 5 UNSAT examples.
## Benchmark table
| Benchmark | Variables | Clauses | Sat   | Min       | Mean       | Max         |
|-----------|-----------|---------|-------|-----------|------------|-------------|
| 3SAT      | 20        | 91      | true  | 0.1777    | 1.3773     | 4.2842      |
| 3SAT      | 50        | 218     | false | 8.5683    | 11.1301    | 13.3872     |
| 3SAT      | 50        | 218     | true  | 0.6804    | 12.085     | 22.4302     |
| 3SAT      | 75        | 325     | false | 110.6506  | 181.0858   | 298.4542    |
| 3SAT      | 75        | 325     | true  | 7.6178    | 81.5885    | 137.0129    |
| 3SAT      | 100       | 430     | false | 875.395   | 2466.0118  | 4684.8212   |
| 3SAT      | 100       | 430     | true  | 194.3403  | 848.2488   | 1949.9956   |
| 3SAT      | 125       | 538     | false | 7951.4736 | 83697.1212 | 236913.5728 |
| 3SAT      | 125       | 538     | true  | 556.7263  | 3695.0825  | 12279.4317  |
| 3SAT      | 150       | 645     | true  | 2095.2928 | 51922.1218 | 108480.4553 |
| FLAT      | 30        | 60      | true  | 0.4539    | 0.4703     | 0.5178      |
| FLAT      | 50        | 115     | true  | 0.8492    | 1.0503     | 1.7589      |
| FLAT      | 75        | 180     | true  | 3.0766    | 26.3541    | 59.6436     |
| FLAT      | 100       | 239     | true  | 2.1112    | 2.4432     | 3.0636      |
| FLAT      | 125       | 301     | true  | 27.7057   | 3413.2843  | 14495.4035  |

The time is again in milliseconds. On each benchmark we can see improvement over the classic DPLL algorithm. We can solve the hardest 3SAT and FLAT formulas over 3 times faster.

# CDCL
For the code check the project named `cdcl`.
```
dpll usage: cdcl [input] [--sat | -s | --cnf | -c] -d <decisions> -m <multiplier>
if the format cannot be read from file extension you can specify the format using --sat | -s for smt-lib, --cnf | -c for dimacs
<decisions> decsribe how many decisions are performed before the first restart
<multiplier> each restart we update <decisions> = <decisions> * <multiplier>
<cacheSize> size of learned clauses cache
```
## Parameters
We can change how many decision we do before first restart and how quickly will our geometric sequence for restart grow. It seems the best results are when the multiplier is between 1.1 and 1.5 but the higher the multiplier is, the bigger the chance is that our cdcl will spend more time in some unsolvable decision branch. For cache size it was usually good idea to set it around the number of clauses in formula.
## Cdcl performance
We will try several examples from the https://www.cs.ubc.ca/~hoos/SATLIB/benchm.html from the `Uniform Random-3-SAT` and the `"Flat" Graph Colouring` category. For each category we will try 5 SAT and 5 UNSAT examples.
## Benchmark table
| Benchmark | Variables | Clauses | Sat   | Min      | Mean      | Max       |
|-----------|-----------|---------|-------|----------|-----------|-----------|
| 3SAT      | 20        | 91      | true  | 0.2551   | 0.4566    | 0.5908    |
| 3SAT      | 50        | 218     | false | 6.9606   | 8.4247    | 9.978     |
| 3SAT      | 50        | 218     | true  | 2.3105   | 6.3578    | 13.935    |
| 3SAT      | 75        | 325     | false | 25.1731  | 49.4124   | 75.4501   |
| 3SAT      | 75        | 325     | true  | 10.2552  | 39.5438   | 80.9253   |
| 3SAT      | 100       | 430     | false | 187.2836 | 343.1574  | 496.8805  |
| 3SAT      | 100       | 430     | true  | 22.3274  | 121.5055  | 207.5239  |
| 3SAT      | 125       | 538     | false | 770.8266 | 1125.8307 | 1989.6746 |
| 3SAT      | 125       | 538     | true  | 25.4813  | 581.6492  | 1207.4254 |
| 3SAT      | 150       | 645     | true  | 897.5517 | 2048.4883 | 4540.1261 |
| FLAT      | 30        | 60      | true  | 0.4998   | 0.7893    | 1.8932    |
| FLAT      | 50        | 115     | true  | 0.9203   | 0.9473    | 0.9646    |
| FLAT      | 75        | 180     | true  | 3.0055   | 12.5118   | 42.158    |
| FLAT      | 100       | 239     | true  | 2.3725   | 2.8742    | 3.873     |
| FLAT      | 125       | 301     | true  | 9.8877   | 80.0182   | 169.4473  |
The parameters for the CDLC were: Decisions=10000; Multiplier=1.1; Cache=10000; The CDCL uses VSIDS heuristics for choosing which variable to try in decision procedure. The CDCL is the fastest one by far with any combination of parameters we did try.

All our benchmarks can be found in the file 20_07_2022_benchmark.txt.