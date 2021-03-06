﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Matrix.Base;

namespace Matrix
{
    /// <summary>
    /// Square matrix class.
    /// </summary>
    /// <typeparam name="T">Type of elements.</typeparam>
    public class SqMatrix<T> : Matrix<T>
    {
        private T[] coreMatrix;
        
        public SqMatrix(IEnumerable<T> elements, int rank)
        {
            if (elements == null)
            {
                throw new ArgumentNullException(nameof(elements));
            }

            if (rank <= 0)
            {
                throw new ArgumentException($"{nameof(rank)} must be greater than 0");
            }

            int maxCapasity = rank * rank;
            var elementsArray = elements as T[] ?? elements.ToArray();
            if (elementsArray.Count() > maxCapasity)
            {
                throw new ArgumentException($"Rank {rank} matrix cannot contain all elements of the {nameof(elements)}");
            }

            this.Rank = rank;
            
            this.coreMatrix = new T[maxCapasity];
            
            elementsArray.CopyTo(this.coreMatrix, 0);
        }
        
        /// <summary>
        /// Matrix indexer.
        /// </summary>
        /// <param name="row">Number of row (lowest is 0).</param>
        /// <param name="column">Number of column (lowest is 0).</param>
        /// <exception cref="ArgumentException">Throws if any argument is out of range.</exception>
        /// <returns>Matrix element.</returns>
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

                return this.coreMatrix[(row * this.Rank) + column];
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

                this.coreMatrix[(row * this.Rank) + column] = value;

                this.OnModifyElement(new ModifyElementInfo()
                                {
                                    Message = $"Element {row},{column} has been changed!",
                                    Row = row,
                                    Column = column
                                });
            }
        }

        public override bool ContentEquals(Matrix<T> other, EqualityComparer<T> comparer)
        {
            var otherSqMatrix = (SqMatrix<T>)other;

            return this.coreMatrix.Select((x, index) => new { x, index }).All(item => comparer.Equals(item.x, otherSqMatrix.coreMatrix[item.index]));
        }
        
        /// <summary>
        /// Create clone of current matrix.
        /// </summary>
        /// <returns>Clone of current matrix.</returns>
        public override Matrix<T> Clone()
        {
            return new SqMatrix<T>(this.coreMatrix, this.Rank);
        }

        public override IEnumerator<T> GetEnumerator()
        {
            foreach (var item in this.coreMatrix)
            {
                yield return item;
            }
        }
    }
}
