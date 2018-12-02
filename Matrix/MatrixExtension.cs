using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
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
        public static SqMatrix<T> Add<T>(this SqMatrix<T> first, SqMatrix<T> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            var temp = first.Clone();

            for (int row = 0; row < temp.Rank; row++)
            {
                for (int column = 0; column < temp.Rank; column++)
                {
                    temp[row, column] = (dynamic)temp[row, column] + second[row, column];
                }
            }
            
            return temp;
        }
        
        //private static T Add<T>(this T first, T other)
        //{
        //    // ???
        //    return (T)(typeof(T).GetMethod("op_Addition")?.Invoke(null, new object[] { first, other }));
        //}
    }
}
