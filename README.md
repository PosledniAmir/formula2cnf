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
We will try several examples from the https://www.cs.ubc.ca/~hoos/SATLIB/benchm.html from the `Uniform Random-3-SAT` category, we will try both SAT (and possibly UNSAT) examples we will start at 20 variables, 91 clauses and end 75 variables, 325 clauses. For each category we will try 5 SAT and 5 UNSAT examples. Sadly bigger example took to long to solve.
### 20 variables
For the SAT version we have solved the first 5 formulas in: 19.0807 ms, 15.1395 ms, 20.0352 ms, 19.3031 ms, 18.5031 ms. This gives average of 18.4123 ms.
### 50 variables
For the SAT version we have solved the first 5 formulas in: 39.6481 ms, 363.3449 ms, 14.9839 ms, 389.0846 ms, 35.2505 ms. This gives average of 168.4624 ms.
For the UNSAT version we have solved the first 5 formulas in: 797.2367 ms, 753.0211 ms, 830.7626 ms, 778.5681 ms, 1187.2942 ms. This gives average of 869.3765 ms.
### 75 variables
For the SAT version we have solved the first 5 formulas in: 1252.9627 ms, 1415.7515 ms, 3756.0825 ms, 13768.3877 ms, 72004.1975 ms. This gives average of 18439.4764 ms.
For the UNSAT version we have solved the first 5 formulas in: 46602.4061 ms, 173289.5823 ms, 63258.4603 ms, 27627.5506 ms, 33201.0309 ms. This gives average of 68795.8060 ms.
### 100 variables
For the SAT version we have solved the first 5 formulas in: 44450.9345 ms, 4420860.2635 ms, 143164.1246 ms, 877283.7648 ms, 6054.1923 ms. This gives average of 1098362.6559 ms.
### Conclusion
Let's put our averages into a table to demonstrate how quickly the running time grows. SAT means whether the instances had model (true) or not (false).
| Variables | Clauses | Running time (ms) | SAT   |
|-----------|---------|-------------------|-------|
| 20        | 91      | 18.4              | true  |
| 50        | 218     | 168.5             | true  |
| 50        | 218     | 869.4             | false |
| 75        | 325     | 18439.5           | true  |
| 75        | 325     | 68795.8           | false |
| 100       | 430     | 1098362.7         | true  |

In comparison these results to the version without watched literals we can see some improvement. Solving for SAT 75 was on average 1.4 times faster. For of the same size unsat it was 3 times faster.
