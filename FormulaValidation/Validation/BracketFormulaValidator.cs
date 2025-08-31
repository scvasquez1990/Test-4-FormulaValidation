using FormulaValidation.Contracts;

namespace FormulaValidation.Validation
{
    public class BracketFormulaValidator : IFormulaValidator
    {
        private readonly Dictionary<char, char> CloserToOpener = new()
        {
            [')'] = '(',
            [']'] = '[',
            ['}'] = '{'
        };

        public bool IsValid(string formula)
        {
            if (string.IsNullOrEmpty(formula))
            {
                return false;
            }    
                
            var openers = new Stack<char>();

            foreach (var ch in formula)
            {
                if (CloserToOpener.ContainsValue(ch))
                {
                    openers.Push(ch);
                }
                else if (CloserToOpener.ContainsKey(ch))
                {
                    if (openers.Count == 0)
                    {
                        return false;
                    }                
                    
                    var expected = CloserToOpener[ch];
                    
                    if (openers.Pop() != expected)
                    {
                        return false;
                    }         
                }
                else
                {
                    return false;
                }
            }

            return openers.Count == 0;
        }
    }
}
