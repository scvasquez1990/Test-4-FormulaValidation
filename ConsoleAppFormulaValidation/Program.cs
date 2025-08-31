using FormulaValidation.Service;
using FormulaValidation.Contracts;
using FormulaValidation.IO;
using FormulaValidation.Validation;

namespace MyApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            IFormulaReader reader = new TextFileReader(@"C:\Users\scvas\Desktop\Modularis\Technical Test 4 - Formulas\Bad formed formulas.txt");
            IFormulaValidator validator = new BracketFormulaValidator();
            var _validatorService = new FormulaProcessor(reader,validator);

            foreach (var result in _validatorService.ValidateFormulas()) 
            { 
                Console.WriteLine($"{result.formula} - {result.isValid}");
            }
        }
    }
}