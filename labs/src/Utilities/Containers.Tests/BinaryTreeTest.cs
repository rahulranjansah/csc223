using System;
using Xunit;
using homework;

namespace homework.__tests__
{
    public class BinaryDigitTreeTests
    {
        /// <summary>
        /// Build a BinaryDigitTree by incrementing from 0 up to the target value.
        /// This avoids accessing protected TreeNode<T>.
        /// </summary>
        private BinaryTree<int>.BinaryDigitTree BuildTreeFromInt(int value)
        {
            var tree = new BinaryTree<int>.BinaryDigitTree();
            for (int i = 0; i < value; i++)
            {
                tree.Increment();
            }
            return tree;
        }

        [Theory(DisplayName = "CalculateBase10 returns correct decimal value")]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(15)]
        [InlineData(32)]
        public void CalculateBase10_Works(int number)
        {
            var tree = BuildTreeFromInt(number);
            Assert.Equal(number, tree.CalculateBase10());
        }

        [Theory(DisplayName = "DivideBy2 halves the number correctly")]
        [InlineData(2, 1)]
        [InlineData(3, 1)]  // integer division truncates
        [InlineData(4, 2)]
        [InlineData(9, 4)]
        [InlineData(15, 7)]
        public void DivideBy2_Works(int input, int expected)
        {
            var tree = BuildTreeFromInt(input);
            tree.DivideBy2();
            Assert.Equal(expected, tree.CalculateBase10());
        }

        [Theory(DisplayName = "DivideByPowerOf2 shifts right by multiple steps")]
        [InlineData(8, 1, 4)]
        [InlineData(8, 2, 2)]
        [InlineData(8, 3, 1)]
        [InlineData(9, 2, 2)] // 1001 -> 10
        [InlineData(32, 5, 1)]
        public void DivideByPowerOf2_Works(int input, int power, int expected)
        {
            var tree = BuildTreeFromInt(input);
            tree.DivideByPowerOf2(power);
            Assert.Equal(expected, tree.CalculateBase10());
        }

        [Fact(DisplayName = "Increment increases value by 1")]
        public void Increment_Works()
        {
            var tree = BuildTreeFromInt(7); // 111 (7)
            tree.Increment();
            Assert.Equal(8, tree.CalculateBase10()); // 1000
        }

        [Fact(DisplayName = "Increment carries correctly through multiple 1s")]
        public void Increment_WithCarry()
        {
            var tree = BuildTreeFromInt(3); // 11
            tree.Increment();
            Assert.Equal(4, tree.CalculateBase10()); // 100
        }

        [Fact(DisplayName = "Increment from zero sets tree to 1")]
        public void Increment_FromZero()
        {
            var tree = new BinaryTree<int>.BinaryDigitTree();
            tree.Increment();
            Assert.Equal(1, tree.CalculateBase10());
        }

        [Fact(DisplayName = "Increment throws overflow when Root is null")]
        public void Increment_Overflow()
        {
            var tree = new BinaryTree<int>.BinaryDigitTree();
            // simulate invalid state: never incremented, so Root = null
            Assert.Throws<OverflowException>(() => tree.Increment());
        }
    }
}
