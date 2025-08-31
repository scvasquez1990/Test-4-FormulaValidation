using FormulaValidation.Contracts;

namespace FormulaValidation.Service
{
    public class FormulaProcessor
    {
        private readonly IFormulaReader _reader;
        private readonly IFormulaValidator _validator;

        public FormulaProcessor(IFormulaReader reader, IFormulaValidator validator)
        {
            _reader = reader;
            _validator = validator;
        }

        public IEnumerable<(string formula, bool isValid)>ValidateFormulas() 
        { 
            foreach (var formula in _reader.Read())
            {
                yield return (formula, _validator.IsValid(formula));
            }
        }
    }
}
