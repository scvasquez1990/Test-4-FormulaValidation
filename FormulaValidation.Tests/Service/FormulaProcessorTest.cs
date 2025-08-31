using FormulaValidation.Contracts;
using Moq;
using FormulaValidation.Service;

namespace FormulaValidation.Tests.Service
{
    public class FormulaServiceValidatorTests
    {
        [Theory]
        // valid
        [InlineData("()", true)]
        [InlineData("[]", true)]
        [InlineData("{}", true)]
        [InlineData("()[]{}", true)]
        [InlineData("({[]})", true)]
        [InlineData("{[()()]}", true)]
        [InlineData("{[]}{()}", true)]
        // invalid
        [InlineData("(", false)]
        [InlineData(")", false)]
        [InlineData("(]", false)]
        [InlineData("([)]", false)]
        [InlineData("(()", false)]
        [InlineData("())", false)]
        [InlineData("}{", false)]
        public void ValidateFormulas_ProcesorWithValidAndInvalidSequence(string formula, bool expected)
        {
            // Arrange: each run returns a single formula so we can assert per-case
            var readerMock = new Mock<IFormulaReader>();
            readerMock.Setup(r => r.Read()).Returns(new[] { formula });

            var validatorMock = new Mock<IFormulaValidator>();
            validatorMock.Setup(v => v.IsValid(formula)).Returns(expected);

            var processor = new FormulaProcessor(readerMock.Object, validatorMock.Object);

            // Act
            var results = processor.ValidateFormulas().ToList();

            // Assert
            Assert.Single(results);
            Assert.Equal((formula, expected), results[0]);
        }
    }
}
