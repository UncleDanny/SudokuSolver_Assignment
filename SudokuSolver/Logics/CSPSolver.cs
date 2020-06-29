using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Logics
{
    /// <summary>
    /// Represents a sudoku solver that uses a Constraint Satisfaction Problem approach to solve a sudoku through guessing.
    /// </summary>
    public class CSPSolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CSPSolver"/> class.
        /// </summary>
        public CSPSolver(int[][] board)
        {
            Board = board;
            FillRemainingValues();
        }

        public int[][] Board { get; private set; }

        public List<int[]> RemainingCellValues { get; set; }

        /// <summary>
        /// Recursively guesses the solution to the sudoku.
        /// </summary>
        /// <returns>A bool to continue through the recursion.</returns>
        public bool Solve()
        {
            Tuple<int, int> location = GetSmallestDomainLocation();
            if (location.Item1 == -1)
            {
                return true;
            }
            else
            {
                int row = location.Item1;
                int col = location.Item2;

                foreach (int value in RemainingCellValues[row * 9 + col])
                {
                    Board[row][col] = value;
                    List<int[]> copy = new List<int[]>(RemainingCellValues);
                    FillRemainingValues();

                    if (IsSudokuStillSolvable(row, col))
                    {
                        if (Solve())
                        {
                            return true;
                        }
                    }

                    Board[row][col] = 0;
                    RemainingCellValues = new List<int[]>(copy);
                }
            }

            return false;
        }


        /// <summary>
        /// Fills the remaining values array with the domains for each cell on the board. Adds -1 if the cell is already filled.
        /// </summary>
        private void FillRemainingValues()
        {
            RemainingCellValues = new List<int[]>();
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    int[] value = Board[row][col] == 0 ? GetDomain(row, col) : new int[] { -1 };
                    RemainingCellValues.Add(value);
                }
            }
        }

        /// <summary>
        /// Gets the domain of the cell at the given location coordinates.
        /// </summary>
        /// <param name="row">The row of the cell.</param>
        /// <param name="col">The column of the cell.</param>
        /// <returns>An int array containing the domain of the cell.</returns>
        private int[] GetDomain(int row, int col)
        {
            List<int> domain = Enumerable.Range(1, 9).ToList();
            for (int i = 0; i < 9; i++)
            {
                if (Board[row][i] != 0)
                {
                    domain.Remove(Board[row][i]);
                }

                if (Board[i][col] != 0)
                {
                    domain.Remove(Board[i][col]);
                }
            }

            int boxRow = row - row % 3;
            int boxCol = col - col % 3;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Board[boxRow + i][boxCol + j] != 0)
                    {
                        domain.Remove(Board[boxRow + i][boxCol + j]);
                    }
                }
            }

            return domain.ToArray();
        }

        /// <summary>
        /// Returns the length of the domain if the domain is not empty, or already has a determined value. Otherwise returns 10.
        /// </summary>
        /// <param name="domain">The domain to check the length of.</param>
        /// <returns>A length integer of the given domain.</returns>
        private int GetDomainLength(int[] domain)
        {
            return domain.Contains(0) || domain.Contains(-1) ? 10 : domain.Length;
        }

        /// <summary>
        /// Returns the cell location of the cell that has the smallest domain.
        /// </summary>
        /// <returns>A tuple containing the row and column of the first occurrence, of the smallest domain.</returns>
        private Tuple<int, int> GetSmallestDomainLocation()
        {
            List<int> map = RemainingCellValues.Select(GetDomainLength).ToList();
            int minimum = map.Min();
            if (minimum == 10)
            {
                return new Tuple<int, int>(-1, -1);
            }

            int index = map.IndexOf(minimum);
            return new Tuple<int, int>(index / 9, index % 9);
        }

        /// <summary>
        /// Checks if the sudoku is still solvable with the current input value.
        /// </summary>
        /// <param name="row">The row of the cell.</param>
        /// <param name="col">The column of the cell.</param>
        /// <returns>A boolean whether the domain created would eliminate the possibility of other cells not being fillable.</returns>
        private bool IsSudokuStillSolvable(int row, int col)
        {
            int[] option = RemainingCellValues.ElementAt(row * 9 + col);
            RemainingCellValues.RemoveAt(row * 9 + col);
            if (RemainingCellValues.Where(x => x.Contains(0)).Count() > 0)
            {
                RemainingCellValues.Insert(row * 9 + col, option);
                return false;
            }
            else
            {
                RemainingCellValues.Insert(row * 9 + col, option);
                return true;
            }
        }
    }
}