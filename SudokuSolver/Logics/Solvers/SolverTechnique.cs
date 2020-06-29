using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SudokuSolver.Logics.Solvers {

    public abstract class SolverTechnique {

        public LogicalSolver LogicalSolver { get; }

        public SolverTechnique( LogicalSolver logicalSolver ) {

            LogicalSolver = logicalSolver;
        }

        public static IEnumerable<Type> GetSolverTypes() {

            return typeof( SolverTechnique )
                .Assembly
                .GetTypes()
                .Where( 
                    type => 
                        type.IsSubclassOf( typeof( SolverTechnique ) ) && 
                        !type.IsAbstract
                    );

            //yield return typeof( TwoDirectionTechnique );
            //yield return typeof( SingleOptionTechnique );
            //yield return typeof( NakedSubsetTechnique );
            //yield return typeof( HiddenSubsetTechnique );
            //yield return typeof( SquareRowInteractionTechnique );
        }

        public abstract void ReduceOptions();
    }
}