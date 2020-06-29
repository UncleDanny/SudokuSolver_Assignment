using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SudokuSolver.Logics.Solvers {

    public class SingleOptionTechnique : SolverTechnique {

        public SingleOptionTechnique( LogicalSolver logicalSolver ) : base( logicalSolver ) {
        }

        public override void ReduceOptions() {

            foreach ( var strucs in new BoardStructure[][] { LogicalSolver.Columns, LogicalSolver.Rows, LogicalSolver.Squares } ) {

                foreach ( var struc in strucs ) {

                    for ( int num = 1; num <= 9; num++ ) {

                        Cell opt = null;

                        if( struc.Contains(num) ) {
                            continue; // num is already placed
                        }

                        // Where can we place 'num'?
                        foreach ( var cell in struc.Cells ) {

                            if ( cell.Options.Contains( num ) ) {

                                if ( opt != null ) {

                                    opt = null;
                                    break;
                                }

                                opt = cell;
                            }
                        }

                        if ( opt == null ) {
                            continue;
                        }

                        opt.RemoveAllExcept( num );
                    }
                }
            }
        }
    }
}