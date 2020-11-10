using GoRestClient.Converters;
using GoRestClient.Models.Enums;
using NUnit.Framework;
using System;
using System.Windows;

namespace GoRestClient.Tests.Converters
{
    [TestFixture]
    public class StatusToBoolConverterTests
    {
        private StatusToBoolConverter _unitUnderTest;

        [SetUp]
        public void SetUp()
        {
            _unitUnderTest = new StatusToBoolConverter();
        }

        static dynamic[] convertAsTestCaseShouldReturnExpectedValueTestCase =
        {
            new dynamic[] { Status.Active, true  },
            new dynamic[] { Status.Inactive, false },
            new dynamic[] { "invalid", DependencyProperty.UnsetValue }
        };

        [Test, TestCaseSource(nameof(convertAsTestCaseShouldReturnExpectedValueTestCase))]
        public void Convert_AsTestCase_ShouldReturnExpectedValue(object value, object expected)
        {
            // Arrange
            Type targetType = typeof(bool);

            // Act
            var actual = _unitUnderTest.Convert(
                value,
                targetType,
                null,
                null);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        static dynamic[] convertBackAsTestCaseShouldReturnExpectedValueTestCase =
        {
            new dynamic[] { true, Status.Active },
            new dynamic[] { false, Status.Inactive },
            new dynamic[] { "invalid", DependencyProperty.UnsetValue }
        };


        [Test, TestCaseSource(nameof(convertBackAsTestCaseShouldReturnExpectedValueTestCase))]
        public void ConvertBack_AsTestCase_ShouldReturnExpectedValue(object value, object expected)
        {
            // Arrange
            Type targetType = typeof(Status);

            // Act
            var actual = _unitUnderTest.ConvertBack(
                value,
                targetType,
                null,
                null);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
