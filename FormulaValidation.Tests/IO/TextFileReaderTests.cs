using FormulaValidation.IO;

namespace FormulaValidation.Tests.IO
{
    public class TextFileReaderTests : IDisposable
    {
        private readonly string _tempFile;

        public TextFileReaderTests()
        {
            _tempFile = Path.Combine(Path.GetTempPath(), $"formulas_{Guid.NewGuid():N}.txt");
        }

        public void Dispose()
        {
            try 
            {
                if (File.Exists(_tempFile))
                { 
                    File.Delete(_tempFile); 
                }  
            } 
            catch 
            { 
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Read_WhenPathIsNullOrWhitespace_ThrowsException(string badPath)
        {
            // Arrange
            var reader = new TextFileReader(badPath);

            // Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => reader.Read().ToList());
            Assert.Equal("File path cannot be null or empty. (Parameter '_filePath')", ex.Message);
        }

        [Fact]
        public void Read_WhenFileIsMissing_ThrowsException()
        {
            // Arrange
            var missingPath = Path.Combine(Path.GetTempPath(), $"missing_{Guid.NewGuid():N}.txt");
            var reader = new TextFileReader(missingPath);

            // Act + Assert
            Assert.Throws<FileNotFoundException>(() => reader.Read().ToList());
        }

        [Fact]
        public void Read_DifferentDelimitersInTheFile_AllFormulasInTheArray()
        {
            // Arrange
            var content = "(), [], {}\n({[]}),([)]  ,  {[]}\n";
            File.WriteAllText(_tempFile, content);

            var reader = new TextFileReader(_tempFile);

            // Act
            var formulas = reader.Read().ToList();

            // Assert
            Assert.Equal(new[]
            {
                "()", "[]", "{}", "({[]})", "([)]", "{[]}"
            }, formulas);
        }

        [Fact]
        public void Read_WhenFileDoesNotEndWithDelimiter_AllFormulasInTheArray()
        {
            // Arrange
            var content = "(),[]{}";
            File.WriteAllText(_tempFile, content);

            var reader = new TextFileReader(_tempFile);

            // Act
            var formulas = reader.Read().ToList();

            // Assert
            
            Assert.Equal("()", formulas[0]);
            Assert.Equal("[]{}", formulas[1]);
        }

        [Fact]
        public void Read_HandlesSmallBuffer_AllFormulasInTheArray()
        {
            // Arrange
            var content = "(),[],{}\n({[]}),([)])";
            File.WriteAllText(_tempFile, content);

            var smallBuffer = 8; 
            var reader = new TextFileReader(_tempFile, smallBuffer);

            // Act
            var formulas = reader.Read().ToList();

            // Assert
            Assert.Equal(new[] { "()", "[]", "{}", "({[]})", "([)])" }, formulas);
        }  
    }

}
