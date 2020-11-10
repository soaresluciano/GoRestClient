using GoRestClient.Converters;
using NUnit.Framework;
using System;
using System.Windows;

namespace GoRestClient.Tests.Converters
{
    [TestFixture]
    public class IdToVisibilityConverterTests
    {

        IdToVisibilityConverter _unitUnderTest;

        [SetUp]
        public void SetUp()
        {

            _unitUnderTest = new IdToVisibilityConverter();
        }

        [TestCase("invalid", Visibility.Collapsed)]
        [TestCase(0, Visibility.Collapsed)]
        [TestCase(10, Visibility.Visible)]
        public void Convert_WhenNotUsingSwitcherParameter_ShouldExpectedBehavior(object value, Visibility expected)
        {
            // Arrange
            Type targetType = typeof(uint);
            object switcherParameter = null;

            // Act
            var actual = (Visibility)_unitUnderTest.Convert(
                value,
                targetType,
                switcherParameter,
                null);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase("invalid", Visibility.Visible)]
        [TestCase(0, Visibility.Visible)]
        [TestCase(10, Visibility.Collapsed)]
        public void Convert_WhenUsingSwitcherParameter_ShouldExpectedBehavior(object value, Visibility expected)
        {
            // Arrange
            Type targetType = typeof(uint);
            object switcherParameter = "true";

            // Act
            var actual = (Visibility)_unitUnderTest.Convert(
                value,
                targetType,
                switcherParameter,
                null);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
