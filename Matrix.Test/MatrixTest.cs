using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    using NUnit.Framework;

namespace Matrix.Test
{
    [TestFixture]
    public class MatrixTest
    {
        [Test]
        public void AddTest_ValidInput_ValidResult_1()
        {
            var first = new SqMatrix<int>(Enumerable.Range(1, 9), 3);
            var second = new SqMatrix<int>(Enumerable.Range(1, 9).Reverse(), 3);

            var expected = new SqMatrix<int>(Enumerable.Repeat(10, 9), 3);

            var actual = first.Add(second);

            Assert.IsTrue(expected.Equals(actual));
        }

        [Test]
        public void AddTest_ValidInput_ValidResult_2()
        {
            var first = new SqMatrix<int>(Enumerable.Range(2, 16), 4);
            var second = new SqMatrix<int>(Enumerable.Range(5, 16).Reverse(), 4);

            var expected = new SqMatrix<int>(Enumerable.Repeat(22, 16), 4);

            var actual = first.Add(second);

            Assert.IsTrue(expected.Equals(actual));
        }

        [Test]
        public void AddTest_WithNullElement()
        {
            var first = new SqMatrix<int>(Enumerable.Range(2, 16), 4);

            Assert.Throws<ArgumentNullException>(() => first.Add(null));
        }

        [Test]
        public void SqMatrixTest_AccessByOutOfRangeIndex()
        {
            var first = new SqMatrix<int>(Enumerable.Range(2, 16), 4);

            Assert.Throws<ArgumentException>(() => first[-1, 12] = 0);
        }

        [Test]
        public void SqMatrixTest_RowsTest()
        {
            var first = new SqMatrix<int>(Enumerable.Range(1, 9), 3);

            IEnumerable<int[]> actual = first.Rows;

            IEnumerable<int[]> expected = new[] {
                                                    new[] { 1, 2, 3 },
                                                    new[] { 4, 5, 6 },
                                                    new[] { 7, 8, 9 }
                                                };
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SqMatrixTest_ColumnsTest()
        {
            var first = new SqMatrix<int>(Enumerable.Range(1, 9), 3);

            IEnumerable<int[]> actual = first.Columns;

            IEnumerable<int[]> expected = new[] {
                                                    new[] { 1, 4, 7 },
                                                    new[] { 2, 5, 8 },
                                                    new[] { 3, 6, 9 }
                                                };

            Assert.AreEqual(expected, actual);
        }
    }
}
