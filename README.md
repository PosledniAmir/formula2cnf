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
| --------- | --------- | ------- | ----- | ---------- | ----------- | ----------- |
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
| --------- | --------- | ------- | ----- | --------- | ---------- | ----------- |
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
| --------- | --------- | ------- | ----- | -------- | --------- | --------- |
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

# N-Queens

This can be solved using our cnf generator `n-queens` which can be found in project with the same name. The generation of the cnf formule starts with generating a condition for each position on the board - the condition can be interpreted either there is not a queen at p_x_y or the is not a queen on the same row, column and diagonals.

Then generate a condition for each row. In row we want to have at least (but at most one is possible due to cell conditions) one queen and we want to satisfy all cell condition for the row. Last condition checks that each row condition must be satysfied.

## Example solution:

For n = 16 CDCL generates this solution:

```
| | |x| | | | | | | | | | | | | |
| | | | | | | | | |x| | | | | | |
| | | | | |x| | | | | | | | | | |
| | | | | | | | |x| | | | | | | |
| | | | | | | | | | | | | |x| | |
| |x| | | | | | | | | | | | | | |
| | | | | | | | | | | | |x| | | |
| | | | | | | | | | | | | | |x| |
| | | | | | |x| | | | | | | | | |
|x| | | | | | | | | | | | | | | |
| | | | | | | | | | |x| | | | | |
| | | | |x| | | | | | | | | | | |
| | | | | | | | | | | |x| | | | |
| | | |x| | | | | | | | | | | | |
| | | | | | | | | | | | | | | |x|
| | | | | | | |x| | | | | | | | |
```

Solutions can be checked using `dimacs-result-reader`.

## Measurement

We than transform this SAT instance to DIMACS format and check the result using these solvers:

- our cdcl

- Yices2

- Z3

We have generated instances of `n-queens` where `n` is from 4 to 100 and then we have run each sat solver using these commands in powershell:

For CDCL:

```powershell
$timer = [Diagnostics.Stopwatch]::StartNew()
for($i = 4; $i -le 100; $i++)
{
    $timer.restart()
    .\cdcl.exe --cnf -d 10000 -m 1.1 -cache 10000 ..\generator\"queens_${i}.dmc" > ..\generator\"queens_${i}.txt"
    $i
    $timer.elapsed.totalminutes
}
```

For Yices2:

```powershell
$timer = [Diagnostics.Stopwatch]::StartNew()
for($i = 4; $i -le 100; $i++)
{
    $timer.restart()
    .\yices-sat.exe -m ..\..\generator\"queens_${i}.dmc" > ..\..\generator\"queens_${i}.txt"
    $i
    $timer.elapsed.totalminutes
}
```

For Z3:

```powershell
$timer = [Diagnostics.Stopwatch]::StartNew()
for($i = 4; $i -le 100; $i++)
{
    $timer.restart()
    .\z3.exe -dimacs ..\..\generator\"queens_${i}.dmc" > ..\..\generator\"queens_${i}.txt"
    $i
    $timer.elapsed.totalminutes
}
```

These are our running times for CDCL:

| n   | minutes             |
| --- | ------------------- |
| 4   | 0.00253688          |
| 5   | 0.00368188333333333 |
| 6   | 0.00651095666666667 |
| 7   | 0.007577115         |
| 8   | 0.01149651          |
| 9   | 0.00954940166666667 |
| 10  | 0.0320168866666667  |
| 11  | 0.509576175         |
| 12  | 0.05816442          |
| 13  | 0.0474432416666667  |
| 14  | 1.63767982166667    |
| 15  | 1.60584311666667    |
| 16  | 0.8822884           |
| 17  | 3.78659128          |
| 18  | 2.89665627333333    |

There are our running times for YICES:

| n   | minutes              |
| --- | -------------------- |
| 4   | 0.00032324           |
| 5   | 0.000124098333333333 |
| 6   | 0.000129325          |
| 7   | 0.000148313333333333 |
| 8   | 0.000153188333333333 |
| 9   | 0.000174523333333333 |
| 10  | 0.00020909           |
| 11  | 0.000230056666666667 |
| 12  | 0.000258895          |
| 13  | 0.000309303333333333 |
| 14  | 0.000347753333333333 |
| 15  | 0.000453798333333333 |
| 16  | 0.000482026666666667 |
| 17  | 0.000563445          |
| 18  | 0.000908743333333333 |
| 19  | 0.000777055          |
| 20  | 0.00673467166666667  |
| 21  | 0.00200879666666667  |
| 22  | 0.00163801333333333  |
| 23  | 0.00235425           |
| 24  | 0.00256575833333333  |
| 25  | 0.00275467666666667  |
| 26  | 0.00378208           |
| 27  | 0.00286383833333333  |
| 28  | 0.00369508           |
| 29  | 0.003742135          |
| 30  | 0.00348769333333333  |
| 31  | 0.00531705666666667  |
| 32  | 0.00541879166666667  |
| 33  | 0.00864744166666667  |
| 34  | 0.00593313166666667  |
| 35  | 0.00767529333333333  |
| 36  | 0.00795686           |
| 37  | 0.0114684266666667   |
| 38  | 0.0096004            |
| 39  | 0.00850353666666667  |
| 40  | 0.01265644           |
| 41  | 0.014564065          |
| 42  | 0.022299225          |
| 43  | 0.0190875983333333   |
| 44  | 0.0177676616666667   |
| 45  | 0.0182967783333333   |
| 46  | 0.0197447916666667   |
| 47  | 0.02961658           |
| 48  | 0.0224529766666667   |
| 49  | 0.029927615          |
| 50  | 0.0243497983333333   |
| 51  | 0.0228894166666667   |
| 52  | 0.055384295          |
| 53  | 0.0515160333333333   |
| 54  | 0.0435744516666667   |
| 55  | 0.0824962866666667   |
| 56  | 0.0433428183333333   |
| 57  | 0.0579269816666667   |
| 58  | 0.09047874           |
| 59  | 0.047281485          |
| 60  | 0.11390378           |

There are our running times for Z3:

| n   | minutes              |
| --- | -------------------- |
| 4   | 0.000207766666666667 |
| 5   | 0.00374310166666667  |
| 6   | 0.0141897116666667   |
| 7   | 0.00118491           |
| 8   | 0.00153757833333333  |
| 9   | 0.00127635833333333  |
| 10  | 0.00066086           |
| 11  | 0.000668345          |
| 12  | 0.005789415          |
| 13  | 0.000883291666666667 |
| 14  | 0.000901318333333333 |
| 15  | 0.001018085          |
| 16  | 0.00253811666666667  |
| 17  | 0.00132182333333333  |
| 18  | 0.002004155          |
| 19  | 0.00468713666666667  |
| 20  | 0.0014501            |
| 21  | 0.006756795          |
| 22  | 0.00426930833333333  |
| 23  | 0.00307876166666667  |
| 24  | 0.00349286833333333  |
| 25  | 0.00320997           |
| 26  | 0.0121096766666667   |
| 27  | 0.00414225666666667  |
| 28  | 0.0161751416666667   |
| 29  | 0.02408986           |
| 30  | 0.0346212666666667   |
| 31  | 0.037151875          |
| 32  | 0.0157891083333333   |
| 33  | 0.0484799583333333   |
| 34  | 0.0759917983333333   |
| 35  | 0.0166635583333333   |
| 36  | 0.330402836666667    |
| 37  | 0.0560252733333333   |
| 38  | 0.209028568333333    |
| 39  | 0.460858433333333    |
| 40  | 0.482793616666667    |
| 41  | 0.471854395          |
| 42  | 0.653358675          |
| 43  | 0.606289676666667    |
| 44  | 0.456803445          |
| 45  | 0.706352246666667    |
| 46  | 0.47380617           |
| 47  | 0.496884448333333    |
| 48  | 1.0603049            |
| 49  | 0.646658466666667    |
| 50  | 0.850956171666667    |
| 51  | 0.97750024           |
| 52  | 0.0447063766666667   |
| 53  | 1.05214992833333     |
| 54  | 0.890298911666667    |
| 55  | 0.97239257           |
| 56  | 0.661392483333333    |
| 57  | 0.707686895          |
| 58  | 1.18638380833333     |
| 59  | 1.08720095666667     |
| 60  | 1.20346609166667     |

# Cryptoarithmetics

In the project `cryptoarithmetics` there can be found an utility that can solve this problem. It uses Z3 library as a dependency.

## Usage description:

```
Cryptoarithmetics usage: cryptoarithmetics [input] [--unique | -u] -k <base> -c <computeSize> -p <printSize>
[input] = text file with cryptoarithmetics instance
-u, -unique = specifies if solution should have unique digit per letter per satisfying clauses
<base> = base of the solution
<computeSize> = number of solutions computed
<printSize> = number of solutions printed";


```

## Solutions
As an input we have used this file:

```
SO + MANY + MORE + MEN + SEEM + TO + SAY + THAT + THEY + MAY + SOON + TRY + TO + STAY + AT + HOME + SO + AS + TO + SEE + OR + HEAR + THE + SAME + ONE + MAN + TRY + TO + MEET + THE + TEAM + ON + THE + MOON + AS + HE + HAS + AT + THE + OTHER + TEN = TESTS
HAIKU+SUSHI=KIMONO
KENDO+KIMONO=SASHIMI 
HAIKU+KIMONO+SUSHI=SASHIMI
JAPAN+NIKKO+TOKYO=KYOTO
(OSAKA+SUSHI+TOKYO=HAIKU) || (NIKKO+OSAKA+SUSHI=TOKYO)
(KOBE+OSAKA+TOKYO=KYOTO) && (KENDO+OSAKA+SAKE=KIMONO)
(JAPAN+KYOTO+NIKKO=KIMONO) && (HAIKU+NARA+NIKKO=KIMONO) && (HAIKU+OSAKA+TOKYO=KYOTO)
(JAPAN+SUSHI+NIKKO=KIMONO) && (HAIKU+NARA+SAKE=KIMONO) || (HAIKU+OSAKA+TOKYO=KYOTO)
```
### Base 2, not unique

```
SAT status: UNSATISFIABLE
Solved in: 52 ms

SAT status: 10010+10110=101000
Solved in: 31 ms

SAT status: UNSATISFIABLE
Solved in: 21 ms

SAT status: 10110+110000+10111=1011101
Solved in: 21 ms

SAT status: UNSATISFIABLE
Solved in: 20 ms

SAT status: UNSATISFIABLE
Solved in: 22 ms

SAT status: UNSATISFIABLE
Solved in: 17 ms

SAT status: UNSATISFIABLE
Solved in: 23 ms

SAT status: UNSATISFIABLE
Solved in: 24 ms
```
### Base 10, not unique

```
SAT status: 12 + 1620 + 1229 + 192 + 1991 + 92 + 160 + 9869 + 9890 + 160 + 1222 + 920 + 92 + 1960 + 69 + 8219 + 12 + 61 + 92 + 199 + 22 + 8962 + 989 + 1619 + 229 + 162 + 920 + 92 + 1999 + 989 + 9961 + 22 + 989 + 1222 + 61 + 89 + 861 + 69 + 989 + 29892 + 992 = 99191
Solved in: 99 ms

SAT status: 89011+11180=100191
Solved in: 20 ms

SAT status: 90181+929111=1019292 
Solved in: 19 ms

SAT status: 20792+979878+12127=1012797
Solved in: 20 ms

SAT status: 13337+70993+13983=98313
Solved in: 24 ms

SAT status: (11959+16136+11561=39656) || (96551+11959+16136=11561)
Solved in: 22 ms

SAT status: UNSATISFIABLE
Solved in: 19 ms

SAT status: UNSATISFIABLE
Solved in: 19 ms

SAT status: (90901+10113+13994=939414) && (10390+1000+1090=939414) || (10390+41090+44964=96444)
Solved in: 22 ms
```
### Base 10, unique

```
SAT status: 31 + 2764 + 2180 + 206 + 3002 + 91 + 374 + 9579 + 9504 + 274 + 3116 + 984 + 91 + 3974 + 79 + 5120 + 31 + 73 + 91 + 300 + 18 + 5078 + 950 + 3720 + 160 + 276 + 984 + 91 + 2009 + 950 + 9072 + 16 + 950 + 2116 + 73 + 50 + 573 + 79 + 950 + 19508 + 906 = 90393
Solved in: 778 ms

SAT status: 92315+45493=137808
Solved in: 1521 ms

SAT status: 94576+923656=1018232 
Solved in: 1493 ms

SAT status: 80293+924767+13182=1018242
Solved in: 122 ms

SAT status: 10904+45886+26836=83626
Solved in: 1414 ms

SAT status: 25474+50536+92782=34670) || (16772+25474+50536=92782)
Solved in: 18962 ms

SAT status: UNSATISFIABLE
Solved in: 21 ms

SAT status: UNSATISFIABLE
Solved in: 24 ms

SAT status: UNSATISFIABLE
Solved in: 91697 ms
```
### Base 16, unique

```
SAT status: 21 + C8FE + C1A4 + C4F + 244C + 71 + 28E + 7387 + 734E + C8E + 211F + 7AE + 71 + 278E + 87 + 31C4 + 21 + 82 + 71 + 244 + 1A + 348A + 734 + 28C4 + 1F4 + C8F + 7AE + 71 + C447 + 734 + 748C + 1F + 734 + C11F + 82 + 34 + 382 + 87 + 734 + 1734A + 74F = 74272
Solved in: 1280 ms

SAT status: F6315+454F3=13B808
Solved in: 8336 ms

SAT status: F78A9+F23989=101B232 
Solved in: 718 ms

SAT status: E02F3+F2ADCD+131E2=101E2A2
Solved in: 687 ms

SAT status: 4BDB1+1388F+2F8EF=8EF2F
Solved in: 1217 ms

SAT status: (A4B2B+404E5+FA23A=EB520) || (1522A+A4B2B+404E5=FA23A)
Solved in: 1482 ms

SAT status: UNSATISFIABLE
Solved in: 20 ms

SAT status: UNSATISFIABLE
Solved in: 22 ms

SAT status: (76B68+4A452+82993=920383) && (5629A+86D6+469C=920383) || (5629A+34696+139E3=9E313)
Solved in: 26547 ms
```

# Backbones
In the project `backbones` there is an utility to find backbones for cnf formulae in DIMACS format. The usage is:
```
backbones usage: backbones [input]
[input] = path directory with CNF formulae encoded in DIMACS
```
## Algorithm
The algorithm first computes score for each variable. We have used Jeroslo-Wang heuristics, which gives a very good evaluation to an intuition: "If a variable appears often in short formulas, then it might be a backbone." After the first run we now have a set of candidates, where each literal could be a back bone. Then we sort the candidates according to their Jeroslow-Wang score and we check whether the formula can be evaluated without this literal (forcing its negation). If the formula is still satisfiable, we check the result against our current candidates potentionally filtering false candidates out. We repeat this process until we check all candidates or the candidate set is empty.
## Solutions
We have downloaded 45 files from [SATLIB](https://www.cs.ubc.ca/~hoos/SATLIB/benchm.html) and tried our backbone utility on them.
| filename                   | calls | backbones |
|----------------------------|-------|-----------|
| CBS_k3_n100_m403_b10_0.cnf |   22  |     10    |
| CBS_k3_n100_m403_b10_1.cnf |   21  |     10    |
| CBS_k3_n100_m403_b10_2.cnf |   24  |     10    |
| CBS_k3_n100_m403_b10_3.cnf |   20  |     10    |
| CBS_k3_n100_m403_b10_4.cnf |   23  |     10    |
| CBS_k3_n100_m403_b30_0.cnf |   41  |     30    |
| CBS_k3_n100_m403_b30_1.cnf |   36  |     30    |
| CBS_k3_n100_m403_b30_2.cnf |   39  |     30    |
| CBS_k3_n100_m403_b30_3.cnf |   44  |     30    |
| CBS_k3_n100_m403_b30_4.cnf |   38  |     30    |
| CBS_k3_n100_m403_b50_0.cnf |   58  |     50    |
| CBS_k3_n100_m403_b50_1.cnf |   61  |     50    |
| CBS_k3_n100_m403_b50_2.cnf |   57  |     50    |
| CBS_k3_n100_m403_b50_3.cnf |   58  |     50    |
| CBS_k3_n100_m403_b50_4.cnf |   58  |     50    |
| CBS_k3_n100_m403_b70_0.cnf |   78  |     70    |
| CBS_k3_n100_m403_b70_1.cnf |   78  |     70    |
| CBS_k3_n100_m403_b70_2.cnf |   75  |     70    |
| CBS_k3_n100_m403_b70_3.cnf |   78  |     70    |
| CBS_k3_n100_m403_b70_4.cnf |   76  |     70    |
| CBS_k3_n100_m403_b90_0.cnf |   94  |     90    |
| CBS_k3_n100_m403_b90_1.cnf |   96  |     90    |
| CBS_k3_n100_m403_b90_2.cnf |   96  |     90    |
| CBS_k3_n100_m403_b90_3.cnf |   96  |     90    |
| CBS_k3_n100_m403_b90_4.cnf |   94  |     90    |
| uf100-01.cnf               |   49  |     41    |
| uf100-02.cnf               |   66  |     61    |
| uf100-03.cnf               |   58  |     47    |
| uf100-04.cnf               |   88  |     82    |
| uf100-05.cnf               |   64  |     58    |
| uf150-01.cnf               |   42  |     21    |
| uf150-02.cnf               |   38  |     17    |
| uf150-03.cnf               |   126 |     119   |
| uf150-04.cnf               |   72  |     59    |
| uf150-05.cnf               |   77  |     63    |

It seems that to find backbones we need on average 1.18 calls per a backbone in CNF formula.