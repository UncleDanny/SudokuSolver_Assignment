using SudokuSolver.Logics.Solvers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace SudokuSolver.Logics {

    public class LogicalSolver {

        const int BOARDSIZE = 9;
        public Cell[] Cells { get; }

        public Row[] Rows { get; }
        public Column[] Columns { get; }
        public Square[] Squares { get; }

        public List<SolverTechnique> Techniques { get; }

        public LogicalSolver( int[][] fieldData ) {

            Cells = new Cell[ BOARDSIZE * BOARDSIZE ];

            Rows    = new Row[ BOARDSIZE ];
            Columns = new Column[ BOARDSIZE ];
            Squares = new Square[ BOARDSIZE ];

            // Setup fieldata
            for ( int x = 0; x < BOARDSIZE; x++ ) {

                Columns[ x ]    = new Column();
                Rows[ x ]       = new Row();
                Squares[ x ]    = new Square();

                for ( int y = 0; y < BOARDSIZE; y++ ) {

                    Cells[ x * BOARDSIZE + y ] = new Cell( fieldData[ y ][ x ], y * BOARDSIZE + x );
                }
            }

            //Setup rows & columns
            for ( int x = 0; x < BOARDSIZE; x++ ) {

                for ( int y = 0; y < BOARDSIZE; y++ ) {

                    Rows[ y ][ x ]      = Cells[ x * BOARDSIZE + y ];
                    Columns[ x ][ y ]   = Cells[ x * BOARDSIZE + y ];

                    Cells[ x * BOARDSIZE + y ].Column   = Columns[ x ];
                    Cells[ x * BOARDSIZE + y ].Row      = Rows[ y ];
                }
            }

            // Setup Squares
            for ( int i = 0; i < BOARDSIZE; i++ ) {

                for ( int j = 0; j < BOARDSIZE; j++ ) {

                    int x = ( j % 3 ) + ( ( i % 3 ) * 3 );
                    int y = ( j / 3 ) + ( ( i / 3 ) * 3 );

                    Squares[ i ][ j ] = Cells[ x * BOARDSIZE + y ];
                    Cells[ x * BOARDSIZE + y ].Square = Squares[ i ];
                }
            }

            Techniques = new List<SolverTechnique>();
            FillTechniques();
        }

        private void FillTechniques() {

            foreach( var solverType in SolverTechnique.GetSolverTypes() ) {

                var solver = (SolverTechnique)Activator.CreateInstance( solverType, this );
                Techniques.Add( solver );
            }
        }

        private bool ValidSolution() {

            foreach ( var field in Cells ) {

                if ( !field.HasValue ) {
                    return false;
                }
            }

            return true;
        }

        private int GetTotalOptionCount() {

            int result = 0;
            foreach( var cell in Cells ) {

                result += cell.OptionCount;
            }

            return result;
        }

        private bool FillSingleOptions() {

            bool result = false;

            foreach ( var cell in Cells ) {

                if ( !cell.HasValue && cell.OptionCount == 1 ) {

                    result = true;
                    cell.Value = cell.Options.First();

                    foreach ( var struc in cell.Structures ) {

                        foreach ( var otherCell in struc.Cells ) {

                            if ( otherCell == cell ) {
                                continue;
                            }

                            otherCell.RemoveOption( cell.Value );
                        }
                    }
                }
            }

            return result;
        }

        public bool Solve() {

            int count;
            do {

                count = GetTotalOptionCount();
                foreach ( var solver in Techniques ) {

                    solver.ReduceOptions();

                    while ( FillSingleOptions() ) {}
                }

            } while ( count > GetTotalOptionCount() );

            return ValidSolution();
        }

        public bool IsCorrect() {

            foreach ( var row in Rows ) {

                if ( !row.IsCorrect() ) {
                    return false;
                }
            }

            foreach ( var row in Columns ) {

                if ( !row.IsCorrect() ) {
                    return false;
                }
            }

            foreach ( var row in Squares ) {

                if ( !row.IsCorrect() ) {
                    return false;
                }
            }

            return true;
        }

        public int[][] ExportSolution() {

            int[][] solution = new int[ BOARDSIZE ][];
            for ( int i = 0; i < BOARDSIZE; i++ ) {
                solution[ i ] = new int[ BOARDSIZE ];
            }


            for ( int x = 0; x < BOARDSIZE; x++ ) {

                for ( int y = 0; y < BOARDSIZE; y++ ) {

                    solution[ y ][ x ] = Cells[ x * BOARDSIZE + y ].Value;
                }
            }

            return solution;
        }
    }
}