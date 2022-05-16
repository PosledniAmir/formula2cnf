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
We will try several examples from the https://www.cs.ubc.ca/~hoos/SATLIB/benchm.html from the `Uniform Random-3-SAT` category, we will try both SAT (and possibly UNSAT) examples we will start at 20 variables, 91 clauses and end 75 variables, 325 clauses. For each category we will try 5 SAT and 5 UNSAT examples. Sadly bigger example took to long to solve.
### 20 variables
For the SAT version we have solved the first 5 formulas in: 13.9244 ms, 11.1115 ms, 14.221 ms, 13.5392 ms, 13.2869 ms. This gives average of 13.2166 ms.
### 50 variables
For the SAT version we have solved the first 5 formulas in: 106.1836 ms, 491.2626 ms, 11.1711 ms, 357.6828 ms, 19.6191 ms. This gives average of 197.1838 ms.
For the UNSAT version we have solved the first 5 formulas in: 1243.034 ms, 1230.173 ms, 1600.7059 ms, 1694.7185 ms, 2754.1332 ms. This gives average of 1704.5529 ms.
### 75 variables
For the SAT version we have solved the first 5 formulas in: 2905.1712 ms, 18040.8449 ms, 6233.7036 ms, 24925.6289 ms, 76908.4313 ms. This gives average of 25802.7559 ms.
For the UNSAT version we have solved the first 5 formulas in: 141533.6872 ms, 597852.2636 ms, 114975.938 ms, 116615.7823 ms, 114112.5811 ms. This gives average of 217018.0504 ms.
### Conclusion
Let's put our averages into a table to demonstrate how quickly the running time grows. SAT means whether the instances had model (true) or not (false).
| Variables | Clauses | Running time (ms) | SAT   |
|-----------|---------|-------------------|-------|
| 20        | 91      | 13.2              | true  |
| 50        | 218     | 197.2             | true  |
| 50        | 218     | 1704.55           | false |
| 75        | 325     | 25802.76          | true  |
| 75        | 325     | 217018.05         | false |

As we compare running times we can notice solving solvable instance is about 10 times faster than solving unsolvable instance on CNF of same size. Another observation is how quickly grows the running time when we add 25 variables.

# DPLL with watched literals
For the code check the project named `watched`.
```
dpll usage: watched [input] [--sat | -s | --cnf | -c]
if the format cannot be read from file extension you can specify the format using --sat | -s for smt-lib, --cnf | -c for dimacs
```
## Dpll performance
We will try several examples from the https://www.cs.ubc.ca/~hoos/SATLIB/benchm.html from the `Uniform Random-3-SAT` category, we will try both SAT (and possibly UNSAT) examples we will start at 20 variables, 91 clauses and end 150 variables, 645 clauses. For each category we will try 5 SAT and 5 UNSAT examples. Sadly bigger example took to long to solve.
### 20 variables
For the SAT version we have solved the first 5 formulas in: 65.469 ms, 19.8753 ms, 23.8422 ms, 22.6758 ms, 23.1383 ms. This gives average of 31 ms.
### 50 variables
For the SAT version we have solved the first 5 formulas in: 36.85 ms, 46.185 ms, 35.1395 ms, 27.1319 ms, 24.4759 ms. This gives average of 33.9565 ms.
For the UNSAT version we have solved the first 5 formulas in: 39.5593 ms, 39.3603 ms, 30.7397 ms, 40.3192 ms, 31.0708 ms. This gives average of 36.2099 ms.
### 75 variables
For the SAT version we have solved the first 5 formulas in: 35.4949 ms, 30.5103 ms, 40.903 ms, 182.16 ms, 34.7116 ms. This gives average of 64.756 ms.
For the UNSAT version we have solved the first 5 formulas in: 228.0842 ms, 221.3104 ms, 109.8599 ms, 105.9338 ms, 150.7477 ms. This gives average of 163.1872 ms.
### 100 variables
For the SAT version we have solved the first 5 formulas in: 128.6135 ms, 100.2524 ms, 371.6963 ms, 731.873 ms, 666.468 ms. This gives average of 399.7806 ms.
For the UNSAT version we have solved the first 5 formulas in: 1198.5934 ms, 1314.0236 ms, 1144.7816 ms, 1359.7368 ms, 1127.6781 ms. This gives average of 1228.9627 ms.
### 125 variables
For the SAT version we have solved the first 5 formulas in: 436.1463 ms, 390.3966 ms, 473.5576 ms, 49.92 ms, 455.2213 ms. This gives average of 361.0484 ms.
For the UNSAT version we have solved the first 5 formulas in: 3852.7798 ms, 10750.3128 ms, 5032.2208 ms, 8398.7169 ms, 9814.6599 ms. This gives average of 7569.738 ms.
### 150 variables
For the SAT version we have solved the first 5 formulas in: 141193.616 ms, 28648.311 ms, 68079.4933 ms, 10840.2466 ms, 34902.7831 ms. This gives average of 56732.89 ms.
### Conclusion
Let's put our averages into a table to demonstrate how quickly the running time grows. SAT means whether the instances had model (true) or not (false).
| Variables | Clauses | Running time (ms) | SAT   |
|-----------|---------|-------------------|-------|
| 20        | 91      | 31                | true  |
| 50        | 218     | 34                | true  |
| 50        | 218     | 36                | false |
| 75        | 325     | 64.8              | true  |
| 75        | 325     | 163.2             | false |
| 100       | 430     | 399.8             | true  |
| 100       | 430     | 1229              | false |
| 125       | 538     | 361               | true  |
| 125       | 538     | 7569              | false |
| 150       | 645     | 56732             | true  |

We cannot compare this results directly to the results in DPLL before as we changed the decision procedure. This change reduced the running time by a lot, we now can solve much larger formulas.

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
We will try several examples from the https://www.cs.ubc.ca/~hoos/SATLIB/benchm.html from the `Uniform Random-3-SAT` category, we will try both SAT (and possibly UNSAT) examples we will start at 20 variables, 91 clauses and end 150 variables, 645 clauses. For each category we will try 5 SAT and 5 UNSAT examples. Sadly bigger example took to long to solve.
### 20 variables
For the SAT version we have solved the first 5 formulas in: 21.0746 ms, 20.157 ms, 31.0608 ms, 32.5324 ms, 31.2800 ms. This gives average of 27.2210 ms.
### 50 variables
For the SAT version we have solved the first 5 formulas in: 49.4838 ms, 53.8879 ms, 46.9868 ms, 70.2093 ms, 32.6764 ms. This gives average of 50.6488 ms.
For the UNSAT version we have solved the first 5 formulas in: 105.1900 ms, 80.7108 ms, 43.1320 ms, 81.1853 ms, 47.2608 ms. This gives average of 71.4958 ms.
### 75 variables
For the SAT version we have solved the first 5 formulas in: 55.6841 ms, 44.8335 ms, 194.3079 ms, 112.7572 ms, 91.0917 ms. This gives average of 99.7349 ms.
For the UNSAT version we have solved the first 5 formulas in: 486.3935 ms, 482.9661 ms, 694.8741 ms, 429.4911 ms, 546.8459 ms. This gives average of 528.1141 ms.
### 100 variables
For the SAT version we have solved the first 5 formulas in: 218.916 ms, 160.3041 ms, 247.8217 ms, 558.4354 ms, 399.5784 ms. This gives average of 317.0111 ms.
For the UNSAT version we have solved the first 5 formulas in: 7228.4105 ms, 22796.5712 ms, 5235.9298 ms, 63556.0065 ms, 11215.8642 ms. This gives average of 22006.5564 ms.
### 125 variables
For the SAT version we have solved the first 5 formulas in: 330.1212 ms, 1398.6251 ms, 1398.6251 ms, 234.7288 ms, 125.3279 ms. This gives average of 697.4856 ms.
For the UNSAT version we have solved the first 5 formulas in: 102050.3119 ms, 450830.3038 ms, 81275.2945 ms, 281459.5269 ms, 423988.9623 ms. This gives average of 228593.1491 ms.
### 150 variables
For the SAT version we have solved the first 5 formulas in: 7749.8011 ms, 1741.6721 ms, 2100.9276 ms, 247.1413 ms, 1445.0457 ms. This gives average of 2656.9176 ms.
### Conclusion
Let's put our averages into a table to demonstrate how quickly the running time grows. SAT means whether the instances had model (true) or not (false).
| Variables | Clauses | Running time (ms) | SAT   |
|-----------|---------|-------------------|-------|
| 20        | 91      | 27                | true  |
| 50        | 218     | 51                | true  |
| 50        | 218     | 71                | false |
| 75        | 325     | 99                | true  |
| 75        | 325     | 528               | false |
| 100       | 430     | 317               | true  |
| 100       | 430     | 22006             | false |
| 125       | 538     | 697               | true  |
| 125       | 538     | 228593            | false |
| 150       | 645     | 2656              | true  |

We can notice two things, on smaller formulas the CDCL is usually a bit slower than DPLL algorithm, for UNSAT this difference is even bigger - I suspect this is due to restarts in CDCL. On the other hand for bigger SAT clauses CDCL runs faster.
