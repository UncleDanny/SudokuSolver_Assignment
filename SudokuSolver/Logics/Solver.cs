using SudokuSolver.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SudokuSolver.Logics
{
    public class Solver
    {

        public int[][] Solve(int[][] sudoku)
        {
            LogicalSolver solver = new LogicalSolver( sudoku );
            solver.Solve();

            Debug.Assert( solver.IsCorrect() );

            return solver.ExportSolution();
        }

        public int[][] SolveGuessing(int[][] sudoku)
        {
            return sudoku;
        }

        public int[][] Create(int[][] sudoku)
        {
            return sudoku;
        }
    }
}