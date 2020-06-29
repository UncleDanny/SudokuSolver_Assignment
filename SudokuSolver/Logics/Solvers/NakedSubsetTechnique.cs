using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SudokuSolver.Logics.Solvers {

    public class NakedSubsetTechnique : SolverTechnique {

        private struct Pair : IEnumerable<int> {
            public int A { get; }
            public int B { get; }

            public Pair( int a, int b ) {

                A = a;
                B = b;
            }

            public IEnumerator<int> GetEnumerator() {

                yield return A;
                yield return B;
            }

            IEnumerator IEnumerable.GetEnumerator() {

                return GetEnumerator();
            }
        }

        private static readonly Pair[] Pairs;

        static NakedSubsetTechnique() {

            int index = 0;
            Pairs = new Pair[ 36 ];

            for ( int i = 1; i <= 8; i++ ) {

                for ( int j = i + 1; j <= 9; j++ ) {

                    Pairs[ index ] = new Pair( i, j );
                    index++;
                }
            }
        }

        public NakedSubsetTechnique( LogicalSolver logicalSolver ) : base( logicalSolver ) {

        }

        private void ScanPairs( BoardStructure structure ) {

            foreach ( var pair in Pairs ) {

                Cell A = null;
                Cell B = null;

                int count = 0;

                foreach ( var cell in structure.Cells ) {

                    if ( cell.HasValue ) {

                        continue;
                    }

                    if ( cell.OptionCount != 2 ) {

                        continue;
                    }

                    bool hasA = cell.Options.Contains( pair.A );
                    bool hasB = cell.Options.Contains( pair.B );

                    if ( !hasA || !hasB ) {
                        continue;
                    }

                    count++;

                    if ( A is null ) {

                        A = cell;
                    } else if ( B is null ) {

                        B = cell;
                    } else {

                        break;
                    }
                }

                if ( count != 2 ) {

                    continue;
                }

                // Found a pair!
                foreach ( var cell in structure.Cells ) {

                    if ( cell.HasValue || cell == A || cell == B ) {
                        continue;
                    }

                    foreach ( int opt in pair ) {

                        if ( !cell.Options.Contains( opt ) ) {

                            continue;
                        }

                        cell.RemoveOption( opt );
                    }
                }
            }
        }

        public override void ReduceOptions() {

            foreach ( var row in LogicalSolver.Rows ) {

                ScanPairs( row );
            }

            foreach ( var col in LogicalSolver.Columns ) {

                ScanPairs( col );
            }

            foreach ( var square in LogicalSolver.Squares ) {

                ScanPairs( square );
            }
        }
    }
}