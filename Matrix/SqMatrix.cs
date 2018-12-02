using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Matrix
{
    /// <summary>
    /// Square matrix class.
    /// </summary>
    /// <typeparam name="T">Type of elements.</typeparam>
    public class SqMatrix<T> : IEquatable<SqMatrix<T>>, ICloneable
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

            this.Rank = rank;

            int elementsCount = elements.Count();

            int numberOfElements = (elementsCount % rank == 0) ? elementsCount : ((elementsCount / rank) + 1) * rank;

            this.coreMatrix = new T[numberOfElements];
            
            elements.ToArray().CopyTo(this.coreMatrix, 0);
        }

        public event EventHandler<ModifyElementInfo> ModifyElement;

        /// <summary>
        /// Return enum of rows array.
        /// </summary>
        public IEnumerable<T[]> Rows => this.coreMatrix.Select((x, i) => new { x = x, i = i })
                                                       .GroupBy(x => x.i / this.Rank)
                                                       .Select(x => x.Select(y => y.x).ToArray());

        /// <summary>
        /// Return enum of columns array.
        /// </summary>
        public IEnumerable<T[]> Columns => this.coreMatrix.Select((x, i) => new { x = x, i = i })
                                                          .GroupBy(x => x.i % this.Rank)
                                                          .Select(x => x.Select(y => y.x).ToArray());

        /// <summary>
        /// Return matrix rank.
        /// </summary>
        public int Rank { get; }

        /// <summary>
        /// Matrix indexer.
        /// </summary>
        /// <param name="row">Number of row (lowest is 0).</param>
        /// <param name="column">Number of column (lowest is 0).</param>
        /// <exception cref="ArgumentException">Throws if any argument is out of range.</exception>
        /// <returns>Matrix element.</returns>
        public T this[int row, int column]
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
        
        /// <summary>
        /// Equals protocol.
        /// </summary>
        /// <param name="other">Other matrix.</param>
        /// <returns>TRUE: if matrix are equals.</returns>
        public bool Equals(SqMatrix<T> other) 
            => this.Equals(other, EqualityComparer<T>.Default);

        /// <summary>
        /// Equals protocol.
        /// </summary>
        /// <param name="other">Other matrix.</param>
        /// <param name="comparer">Comparer to elements.</param>
        /// <returns>TRUE: if matrix are equals.</returns>
        public bool Equals(SqMatrix<T> other, EqualityComparer<T> comparer)
        {
            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            if (other == null)
            {
                return false;
            }

            if (this.Rank != other.Rank)
            {
                return false;
            }
            
            return this.coreMatrix.Select((x, index) => new { x, index }).All(item => comparer.Equals(item.x, other.coreMatrix[item.index]));
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            return this.Equals((SqMatrix<T>)obj);
        }

        public override int GetHashCode()
        {
            return this.Rank;
        }
        
        /// <summary>
        /// Create clone of current matrix.
        /// </summary>
        /// <returns>Clone of current matrix.</returns>
        public SqMatrix<T> Clone()
        {
            return new SqMatrix<T>(this.coreMatrix, this.Rank);
        }

        object ICloneable.Clone()
            => this.Clone();

        private void OnModifyElement(ModifyElementInfo info)
        {
            this.ModifyElement?.Invoke(this, info);
        }
    }
}
