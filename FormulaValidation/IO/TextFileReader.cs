using FormulaValidation.Contracts;
using System.Text;

namespace FormulaValidation.IO
{
    public class TextFileReader : IFormulaReader
    {
        private readonly int _bufferSize;
        private readonly char[] _delimiters;
        private readonly string _filePath;

        public TextFileReader(string filepath, int bufferSize = 64 * 1024, char[]? delimiters = null)
        {
            _bufferSize = bufferSize;
            _delimiters = delimiters ?? new[] { ',', '\n', '\r' };
            _filePath = filepath;
        }
        public IEnumerable<string> Read()
        {
            if (string.IsNullOrWhiteSpace(_filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(_filePath));
            }

            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException("File not found.", _filePath);
            }

            using var reader = new StreamReader(_filePath, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: _bufferSize);

            var formulaBuilder = new StringBuilder(capacity: 256);
            var charBuffer = new char[_bufferSize];
            int charsRead;

            while ((charsRead = reader.Read(charBuffer, 0, charBuffer.Length)) > 0)
            {
                int tokenStartIndex = 0;

                for (int i = 0; i < charsRead; i++)
                {
                    char currentChar = charBuffer[i];
                    bool isDelimiter = _delimiters.Contains(currentChar);

                    if (isDelimiter)
                    {
                        if (i > tokenStartIndex)
                        {
                            formulaBuilder.Append(charBuffer, tokenStartIndex, i - tokenStartIndex);
                        }
                            
                        string formula = formulaBuilder.ToString().Trim();
                        formulaBuilder.Clear();

                        if (formula.Length > 0)
                        {
                            yield return formula;
                        }
                            
                        tokenStartIndex = i + 1;
                    }
                }

                if (tokenStartIndex < charsRead)
                {
                    formulaBuilder.Append(charBuffer, tokenStartIndex, charsRead - tokenStartIndex);
                }
                    
            }

            string lastFormula = formulaBuilder.ToString().Trim();
            if (lastFormula.Length > 0)
            {
                yield return lastFormula;
            }
                
        }
    }
}
