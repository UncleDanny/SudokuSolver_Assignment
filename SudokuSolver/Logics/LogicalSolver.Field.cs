using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace SudokuSolver.Logics {

    public class Cell {

        public int Value { get; set; }
        public int ID { get; }

        public Row Row { get; set; }
        public Column Column { get; set; }
        public Square Square { get; set; }

        public IEnumerable<BoardStructure> Structures {
            get {
                yield return Row;
                yield return Column;
                yield return Square;
            }
        }

        private static readonly int[] solution = new int[81]{
            8,1,2,7,5,3,6,4,9,
            9,4,3,6,8,2,1,7,5,
            6,7,5,4,9,1,2,8,3,
            
            1,5,4,2,3,7,8,9,6,
            3,6,9,8,4,5,7,2,1,
            2,8,7,1,6,9,5,3,4,

            5,2,1,9,7,4,3,6,8,
            4,3,8,5,2,6,9,1,7,
            7,9,6,3,1,8,4,5,2,
        };


        public static readonly int[] DefaultOptions = { 1,2,3,4,5,6,7,8,9 };

        public bool HasValue {
            get => Value > 0;
            set {

                if ( !value ) {
                    Value = 0;
                } else {

                    throw new Exception( "Cannot set HasValue to true." );
                }
            }
        }

        public List<int> Options { get; }
        public int OptionCount { get => Options.Count; }

        public Cell( int value, int id ) {

            Value = value;
            ID = id;

            if ( value > 0 ) {
                Options = new List<int> { value };
            } else {
                Options = new List<int>( DefaultOptions );
            }
        }

        public bool SameOptions( Cell comp ) {

            if ( OptionCount != comp.OptionCount ) {
                return false;
            }

            foreach ( int i in Options ) {

                if ( !comp.Options.Contains( i ) ) {
                    return false;
                }
            }

            return true;
        }

        public override string ToString() {

            return Value.ToString();
        }

        public void RemoveOption( int option ) {

            //Debug.Assert( option != solution[ID] );
            Options.Remove( option );
        }

        public void RemoveOptionAtIndex( int index ) {

            RemoveOption( Options[index] );
        }

        public void RemoveAllExcept( IEnumerable<int> range ) {

            for( int i = OptionCount - 1; i > -1; i-- ) {

                if ( range.Contains( Options[i] ) ){
                    continue;
                }

                RemoveOptionAtIndex( i );
            }
        }

        public void RemoveAllExcept( int option ) {

            RemoveAllExcept( new int[] { option } );
        }
    }

    public abstract class BoardStructure {

        public Cell[] Cells { get; set; }
        public Cell this[ int index ] {
            get => Cells[ index ];
            set {
                Cells[ index ] = value;
            }
        }

        public BoardStructure() {

            Cells = new Cell[ 9 ];
        }

        public bool Contains( int value ) {

            foreach ( var field in Cells ) {

                if ( field.Value == value ) {
                    return true;
                }
            }

            return false;
        }

        public bool IsCorrect() {

            for ( int i = 1; i < 10; i++ ) {

                int count = 0;

                foreach ( var field in Cells ) {

                    if ( field.Value != i ) {
                        continue;
                    }

                    count++;
                }

                if ( count != 1 ) {
                    return false;
                }
            }

            return true;
        }
    }

    public class Square : BoardStructure {

    }

    public class Column : BoardStructure {

    }

    public class Row : BoardStructure {
    }

}