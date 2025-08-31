using FormulaValidation.Validation;

namespace FormulaValidation.Tests.Validation
{
    public class BracketFormulaValidatorTests
    {
        private readonly BracketFormulaValidator _bracketFormulaValidator = new();

        [Theory]
        [InlineData("()")]
        [InlineData("[]")]
        [InlineData("{}")]
        [InlineData("()[]{}")]
        [InlineData("({[]})")]
        [InlineData("{[()()]}")]
        [InlineData("{[]}{()}")]
        public void IsValid_ForValidSequences_ReturnsTrue(string input)
        {
            // Act
            var result = _bracketFormulaValidator.IsValid(input);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("(")]
        [InlineData("([)]")]
        [InlineData("}{")]
        [InlineData("())(")]
        [InlineData("((((")]
        [InlineData("{[()]}]")] // extra closer
        public void IsValid_ForInvalidSequences_ReturnsFalse(string input)
        {
            // Act
            var result = _bracketFormulaValidator.IsValid(input);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ForNonBracketCharacters_ReturnsFalse()
        {
            // Arrange
            var input = "abc";

            // Act
            var result = _bracketFormulaValidator.IsValid(input);
            
            //Assert
            Assert.False(result);
        }

    }
}
