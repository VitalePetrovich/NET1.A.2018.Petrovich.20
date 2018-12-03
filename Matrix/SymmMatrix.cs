using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrix.Base;

namespace Matrix
{
    public class SymmMatrix<T> : Matrix<T>
    {
        private T[] coreMatrix;

        public SymmMatrix(IEnumerable<T> elements, int rank)
        {

            if (elements == null)
            {
                throw new ArgumentNullException(nameof(elements));
            }

            if (rank <= 0)
            {
                throw new ArgumentException($"{nameof(rank)} must be greater than 0");
            }

            int maxCapasity = (rank + 1) * rank / 2;
            var elementsArray = elements as T[] ?? elements.ToArray();
            if (elementsArray.Count() > maxCapasity)
            {
                throw new ArgumentException($"Rank {rank} matrix cannot contain all elements of the {nameof(elements)}");
            }

            this.Rank = rank;

            this.coreMatrix = new T[maxCapasity];

            elementsArray.CopyTo(this.coreMatrix, 0);
        }

        public override T this[int row, int column]
        {
            get
            {
                if (row < 0 || row > this.Rank)
                {
                    throw new ArgumentException($"Incorrect range of argument {nameof(row)}");
                }

                if (column < 0 || column > this.Rank)
                {
                    throw new ArgumentException($"Incorrect range of argument {nameof(column)}");
                }

                if (row > column)
                {
                    Swap(ref row, ref column);
                }

                return this.coreMatrix[(this.Rank * row) + column - ((row + 1) * row / 2)];
            }

            set
            {
                if (row < 0 || row > this.Rank)
                {
                    throw new ArgumentException($"Incorrect range of argument {nameof(row)}");
                }

                if (column < 0 || column > this.Rank)
                {
                    throw new ArgumentException($"Incorrect range of argument {nameof(column)}");
                }

                if (row > column)
                {
                    Swap(ref row, ref column);
                }

                this.coreMatrix[(this.Rank * row) + column - ((row + 1) * row / 2)] = value;

                this.OnModifyElement(new ModifyElementInfo()
                                     {
                                         Message = $"Element {row},{column} has been changed!",
                                         Row = row,
                                         Column = column
                                     });
                this.OnModifyElement(new ModifyElementInfo()
                                     {
                                         Message = $"Element {column},{row} has been changed!",
                                         Row = column,
                                         Column = row
                                     });
            }
        }

        public override bool ContentEquals(Matrix<T> other, EqualityComparer<T> comparer)
        {
            var otherSymmMatrix = (SymmMatrix<T>)other;

            return this.coreMatrix.Select((x, index) => new { x, index }).All(item => comparer.Equals(item.x, otherSymmMatrix.coreMatrix[item.index]));
        }

        public override Matrix<T> Clone()
        {
            return new SymmMatrix<T>(this.coreMatrix, this.Rank);
        }
        
        public override IEnumerator<T> GetEnumerator()
        {
            foreach (var item in this.coreMatrix)
            {
                yield return item;
            }
        }

        private void Swap(ref int a, ref int b)
        {
            a ^= b ^= a ^= b;
        }
    }
}
