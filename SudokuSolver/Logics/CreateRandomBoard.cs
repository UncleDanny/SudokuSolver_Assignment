using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Logics
{
    public class CreateRandomBoard
    {
        private readonly Random random;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateRandomBoard"/> class.
        /// </summary>
        public CreateRandomBoard(int[][] board)
        {
            Board = board;
            random = new Random();
        }

        public int[][] Board { get; private set; }

        /// <summary>
        /// Generates a random solvable sudoku board.
        /// </summary>
        public void GenerateRandomBoard()
        {
            EmptyBoard();
            for (int i = 0; i < 9; i += 3)
            {
                FillSquare(i, i);
            }

            CSPSolver solver = new CSPSolver(Board);
            solver.Solve();
            RemoveNumbers();
        }

        /// <summary>
        /// Empties the board.
        /// </summary>
        private void EmptyBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Board[i][j] = 0;
                }
            }
        }

        /// <summary>
        /// Fills the square with all numbers from 1 to 9.
        /// </summary>
        /// <param name="row">The first row of the square.</param>
        /// <param name="col">The first column of the square.</param>
        private void FillSquare(int row, int col)
        {
            int number;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    do
                    {
                        number = random.Next(1, 10);
                    }
                    while (InSquare(row, col, number));

                    Board[row + i][col + j] = number;
                }
            }
        }

        /// <summary>
        /// Checks if a number is already found in the square.
        /// </summary>
        /// <param name="row">The first row of the square.</param>
        /// <param name="col">The first column of the square.</param>
        /// <param name="num">The number to be matched.</param>
        /// <returns>A boolean value whether the number is in the square or not.</returns>
        private bool InSquare(int row, int col, int num)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Board[row + i][col + j] == num)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the given row contains the given number, and returns the index in the row.
        /// </summary>
        /// <param name="row">The row to look in.</param>
        /// <param name="number">The number to look for.</param>
        /// <returns>The index of the numbers in the row.</returns>
        private int FindInRow(int row, int number)
        {
            for (int i = 0; i < 9; i++)
            {
                if (Board[row][i] == number)
                {
                    return i;
                }
            }

            throw new Exception("Number must be between 0 and 9");
        }

        /// <summary>
        /// Removes random numbers to create a solvable board.
        /// </summary>
        private void RemoveNumbers()
        {
            List<int> range = new List<int>(Enumerable.Range(1, 10));
            for (int i = 0; i < 9; i++)
            {
                int numberCount = 8;
                int[] rowNumbers = Enumerable.Range(0, 9).ToArray();
                ShuffleArray(rowNumbers);

                for (int num = 0; num < numberCount; num++)
                {
                    int index = FindInRow(rowNumbers[num], range[i]);
                    Board[rowNumbers[num]][index] = 0;
                }
            }
        }

        /// <summary>
        /// Shuffles the values in an array.
        /// </summary>
        /// <typeparam name="T">The element type of the array.</typeparam>
        /// <param name="array">The array to shuffle.</param>
        private void ShuffleArray<T>(T[] array)
        {
            for (int row = 0; row < array.Length; row++)
            {
                int index = random.Next(9);
                Swap(ref array[row], ref array[index]);
            }
        }

        /// <summary>
        /// Swaps the values of the input values.
        /// </summary>
        /// <typeparam name="T">The element type of the values.</typeparam>
        /// <param name="value">The value that swaps with the second.</param>
        /// <param name="value2">The value that swaps with the first.</param>
        private void Swap<T>(ref T value, ref T value2) 
        {
            T temp = value;
            value = value2;
            value2 = temp;
        }
    }
}