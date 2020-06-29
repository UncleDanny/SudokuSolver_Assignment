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
            CSPSolver cspSolver = new CSPSolver(sudoku);
            cspSolver.Solve();
            return cspSolver.Board;
        }

        public int[][] Create(int[][] sudoku)
        {
            CreateRandomBoard cRandom = new CreateRandomBoard(sudoku);
            cRandom.GenerateRandomBoard();
            return cRandom.Board;
        }
    }
}