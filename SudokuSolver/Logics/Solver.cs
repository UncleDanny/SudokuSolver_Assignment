namespace SudokuSolver.Logics
{
    public class Solver
    {
        public int[][] Solve(int[][] sudoku)
        {
            return sudoku;
        }

        public int[][] SolveGuessing(int[][] sudoku)
        {
            CSPSolver cspSolver = new CSPSolver(sudoku);
            cspSolver.Solve();
            return cspSolver.Board;
        }

        public int[][] Create(int[][] sudoku)
        {
            return sudoku;
        }
    }
}