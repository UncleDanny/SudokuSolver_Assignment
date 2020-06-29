using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SudokuSolver.Logics.Solvers {
    
    public class TwoDirectionTechnique : SolverTechnique {

        public TwoDirectionTechnique( LogicalSolver logicalSolver ) : base( logicalSolver ) {
        }

        public override void ReduceOptions() {

            foreach( var cell in LogicalSolver.Cells ) {

                    if ( cell.HasValue ) {
                        continue;
                    }

                    for( int i = cell.OptionCount - 1; i > -1; i-- ) { 

                        int v = cell.Options[i];
                        if ( cell.Square.Contains(v) || cell.Row.Contains( v ) || cell.Column.Contains( v ) ) {

                            cell.RemoveOption( v );
                        }
                    }
            }
        }
    }
}