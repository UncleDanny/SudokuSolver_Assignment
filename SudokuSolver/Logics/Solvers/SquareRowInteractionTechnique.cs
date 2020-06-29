using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SudokuSolver.Logics.Solvers {
    public class SquareRowInteractionTechnique : SolverTechnique {

        public SquareRowInteractionTechnique( LogicalSolver logicalSolver ) : base( logicalSolver ) {
        }

        private bool SingleStructure( IEnumerable<BoardStructure> structures ) {

            var first = structures.First();

            foreach ( var struc in structures ) {

                if ( struc != first ) {

                    return false;
                }
            }

            return true;
        }

        public override void ReduceOptions() {

            foreach ( var square in LogicalSolver.Squares ) {

                for ( int num = 1; num <= 9; num++ ) {

                    List<Cell> possibleCells = new List<Cell>(3);

                    if ( square.Contains( num ) ) {

                        break;
                    }

                    foreach ( var cell in square.Cells ) {

                        if ( cell.Options.Contains( num ) ) {

                            possibleCells.Add( cell );
                            if ( possibleCells.Count > 3 ) {
                                break;
                            }
                        }
                    }

                    if ( possibleCells.Count < 2 || possibleCells.Count > 3 ) {
                        continue;
                    }

                    if ( SingleStructure( possibleCells.Select( o => o.Row ) ) ) {

                        foreach ( var cell in possibleCells[ 0 ].Row.Cells ) {

                            if ( possibleCells.Contains( cell ) ) {
                                continue;
                            }

                            cell.RemoveOption( num );
                        }

                    } else if ( SingleStructure( possibleCells.Select( o => o.Column ) ) ) {

                        foreach ( var cell in possibleCells[ 0 ].Column.Cells ) {

                            if ( possibleCells.Contains( cell ) ) {
                                continue;
                            }

                            cell.RemoveOption( num );
                        }
                    }
                }
            }

        }
    }
}