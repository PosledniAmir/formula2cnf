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
For the UNSAT version we have solved the first 5 formulas in: 218.2502 ms, 218.2502 ms, 218.2502 ms, 218.2502 ms, 218.2502 ms. This gives average of 218.2502 ms.
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
| 75        | 325     | 218.3             | false |
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
```
## Parameters
We can change how many decision we do before first restart and how quickly will our geometric sequence for restart grow. It seems the best results are when the multiplier is between 1.1 and 1.5 but the higher the multiplier is, the bigger the chance is that our cdcl will spend more time in some unsolvable decision branch.
## Cdcl performance
We will try several examples from the https://www.cs.ubc.ca/~hoos/SATLIB/benchm.html from the `Uniform Random-3-SAT` category, we will try both SAT (and possibly UNSAT) examples we will start at 20 variables, 91 clauses and end 150 variables, 645 clauses. For each category we will try 5 SAT and 5 UNSAT examples. Sadly bigger example took to long to solve.
### 20 variables
For the SAT version we have solved the first 5 formulas in: 20.7768 ms, 19.7127 ms, 31.0305 ms, 31.7006 ms, 30.6619 ms. This gives average of 26.7766 ms.
### 50 variables
For the SAT version we have solved the first 5 formulas in: 46.6854 ms, 53.06 ms, 46.1114 ms, 68.5222 ms, 32.9573 ms. This gives average of 49.4673 ms.
For the UNSAT version we have solved the first 5 formulas in: 94.6531 ms, 72.6289 ms, 45.5815 ms, 95.2596 ms, 45.2776 ms. This gives average of 70.6801 ms.
### 75 variables
For the SAT version we have solved the first 5 formulas in: 52.4231 ms, 44.0661 ms, 217.1356 ms, 106.1625 ms, 86.1017 ms. This gives average of 101.1778 ms.
For the UNSAT version we have solved the first 5 formulas in: 435.3746 ms, 186.5247 ms, 785.5186 ms, 315.6756 ms, 309.7535 ms. This gives average of 406.5694 ms.
### 100 variables
For the SAT version we have solved the first 5 formulas in: 202.9892 ms, 126.2317 ms, 128.69 ms, 203.8155 ms, 524.7061 ms. This gives average of 237.2865 ms.
For the UNSAT version we have solved the first 5 formulas in: 8055.8761 ms, 12251.1811 ms, 3937.69 ms, 62408.3985 ms, 6302.7044 ms. This gives average of 18591.1700 ms.
### 125 variables
For the SAT version we have solved the first 5 formulas in: 991.5858 ms, 5161.6533 ms, 168.4024 ms, 103.4658 ms, 551.2405 ms. This gives average of 1395.2700 ms.
For the UNSAT version we have solved the first 5 formulas in: 102050.3119 ms, 477158.0558 ms, 55869.9992 ms, 283165.7257 ms, 224721.6531 ms. This gives average of 228593.1491 ms.
### 150 variables
For the SAT version we have solved the first 5 formulas in: 793.9995 ms, 6808.8594 ms, 38596.514 ms, 5095.3899 ms, 666.6381 ms. This gives average of 10392.2802 ms.
### Conclusion
Let's put our averages into a table to demonstrate how quickly the running time grows. SAT means whether the instances had model (true) or not (false).
| Variables | Clauses | Running time (ms) | SAT   |
|-----------|---------|-------------------|-------|
| 20        | 91      | 27                | true  |
| 50        | 218     | 49                | true  |
| 50        | 218     | 70                | false |
| 75        | 325     | 101               | true  |
| 75        | 325     | 407               | false |
| 100       | 430     | 237               | true  |
| 100       | 430     | 18591             | false |
| 125       | 538     | 1395              | true  |
| 125       | 538     | 228593            | false |
| 150       | 645     | 10392             | true  |

We can notice two things, on smaller formulas the CDCL is usually a bit slower than DPLL algorithm, for UNSAT this difference is even bigger - I suspect this is due to restarts in CDCL. On the otherhand for bigger SAT clauses CDCL runs faster.
