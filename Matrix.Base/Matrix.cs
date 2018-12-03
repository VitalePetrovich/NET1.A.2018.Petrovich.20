namespace Matrix.Base
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Square matrix class.
    /// </summary>
    /// <typeparam name="T">Type of elements.</typeparam>
    public abstract class Matrix<T> : IEquatable<Matrix<T>>, ICloneable, IEnumerable<T>, IEnumerable
    {
        public event EventHandler<ModifyElementInfo> ModifyElement;
        
        /// <summary>
        /// Return matrix rank.
        /// </summary>
        public int Rank { get; protected set; }

        /// <summary>
        /// Matrix indexer.
        /// </summary>
        /// <param name="row">Number of row (lowest is 0).</param>
        /// <param name="column">Number of column (lowest is 0).</param>
        /// <exception cref="ArgumentException">Throws if any argument is out of range.</exception>
        /// <returns>Matrix element.</returns>
        public abstract T this[int row, int column] { get; set; }
        
        /// <summary>
        /// Equals protocol.
        /// </summary>
        /// <param name="other">Other matrix.</param>
        /// <returns>TRUE: if matrix are equals.</returns>
        public bool Equals(Matrix<T> other) 
            => this.Equals(other, EqualityComparer<T>.Default);

        /// <summary>
        /// Equals protocol.
        /// </summary>
        /// <param name="other">Other matrix.</param>
        /// <param name="comparer">Comparer to elements.</param>
        /// <returns>TRUE: if matrix are equals.</returns>
        public bool Equals(Matrix<T> other, EqualityComparer<T> comparer)
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

            if (this.GetType() != other.GetType())
            {
                return false;
            }

            return this.ContentEquals(other, comparer);
        }

        public abstract bool ContentEquals(Matrix<T> other, EqualityComparer<T> comparer);

        /// <summary>
        /// Create clone of current matrix.
        /// </summary>
        /// <returns>Clone of current matrix.</returns>
        public abstract Matrix<T> Clone();
        
        object ICloneable.Clone()
            => this.Clone();

        public abstract IEnumerator<T> GetEnumerator();
        
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

            return this.Equals((Matrix<T>)obj);
        }

        public override int GetHashCode()
        {
            return this.Rank;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        protected void OnModifyElement(ModifyElementInfo info)
        {
            this.ModifyElement?.Invoke(this, info);
        }
    }
}
