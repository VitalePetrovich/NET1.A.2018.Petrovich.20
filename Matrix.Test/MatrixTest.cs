using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Matrix.Extension;

namespace Matrix.Test
{
    using Matrix.Base;

    [TestFixture]
    public class MatrixTest
    {
        [Test]
        public void AddTest_ValidInput_ValidResult_1()
        {
            var first = new SqMatrix<int>(Enumerable.Range(1, 9), 3);
            var second = new SqMatrix<int>(Enumerable.Range(1, 9).Reverse(), 3);

            var expected = new SymmMatrix<int>(Enumerable.Repeat(10, 6), 3);

            var actual = first.Add(second);

            Assert.IsTrue(expected.Equals(actual));
        }

        [Test]
        public void AddTest_ValidInput_ValidResult_2()
        {
            var first = new SqMatrix<int>(Enumerable.Range(2, 16), 4);
            var second = new SqMatrix<int>(Enumerable.Repeat(5, 16), 4);

            var expected = new SqMatrix<int>(Enumerable.Range(7, 16), 4);

            var actual = first.Add(second);

            Assert.IsTrue(expected.Equals(actual));
        }

        [Test]
        public void AddTest_ValidInput_ValidResult_3()
        {
            var first = new DiagMatrix<int>(Enumerable.Range(1, 4), 4);
            var second = new DiagMatrix<int>(Enumerable.Repeat(7, 4), 4);

            var expected = new DiagMatrix<int>(Enumerable.Range(8, 4), 4);

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
    }
}
