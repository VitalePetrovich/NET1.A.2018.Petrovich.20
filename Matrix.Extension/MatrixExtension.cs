namespace Matrix.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Remoting.Messaging;

    using Matrix;
    using Matrix.Base;

    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class MatrixExtension
    {
        /// <summary>
        /// Add one matrix to another.
        /// </summary>
        /// <typeparam name="T">Type of matrix elements.</typeparam>
        /// <param name="first">First matrix.</param>
        /// <param name="second">Second matrix.</param>
        /// <exception cref="ArgumentNullException">Trows if one of matrix is null.</exception>
        /// <returns>Resulted matrix.</returns>
        public static Matrix<T> Add<T>(this Matrix<T> first, Matrix<T> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            if (first.Rank != second.Rank)
            {
                throw new ArgumentException($"Wrong dimensions of matrix.");
            }

            Matrix<T> temp = new SqMatrix<T>(new T[] { }, first.Rank);

            for (int row = 0; row < temp.Rank; row++)
            {
                for (int column = 0; column < temp.Rank; column++)
                {
                    temp[row, column] = (dynamic)first[row, column] + second[row, column];
                }
            }
            
            return ConvertToMatrixType(temp);
        }

        private static Matrix<T> ConvertToMatrixType<T>(Matrix<T> matrix)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;

            bool isDiag = !matrix
                              .Select((value, index) => new { value, row = index / matrix.Rank, col = index % matrix.Rank })
                              .Any(anon => anon.row != anon.col && !comparer.Equals(anon.value, default(T)));

            if (isDiag)
            {
                var elements = matrix.Where((e, index) => index / matrix.Rank == index % matrix.Rank);

                return new DiagMatrix<T>(elements, matrix.Rank);
            }

            bool isSymm = matrix
                            .Select((value, index) => new { value, row = index / matrix.Rank, col = index % matrix.Rank })
                            .All(anon => comparer.Equals(anon.value, matrix[anon.col, anon.row]));

            if (isSymm)
            {
                var elements = matrix
                    .Select((value, index) => new { value, row = index / matrix.Rank, col = index % matrix.Rank })
                    .Where(anon => anon.row <= anon.col).Select(anon => anon.value);

                return new SymmMatrix<T>(elements, matrix.Rank);
            }

            return matrix;
        }
    }
}
