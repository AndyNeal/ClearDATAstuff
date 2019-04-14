using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace Validator.Repositories
{
    public interface IValidationRepository
    {
        bool ValidateRegex(string input);
        bool ValidateLoop(string input);
        bool ValidateAnd(string input);
        bool ValidateLinq(string input);
    }
    public class ValidationRepository : IValidationRepository
    {
        readonly IConfiguration _configuration;
        static string pattern;
        static string validCharacters;

        public ValidationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            pattern = _configuration["ValidationRegex"];
            validCharacters = _configuration["ValidCharacters"];
        }

        public bool ValidateRegex(string input)
        {
            return Regex.Match(input, pattern, RegexOptions.IgnoreCase).Success;
        }

        public bool ValidateLoop(string input)
        {
            input = input.ToLowerInvariant();
            foreach (var item in validCharacters.ToCharArray())
            {
                var found = input.IndexOf(item);
                if (found == -1)
                    return false;
            }
            return true;
        }
        public bool ValidateAnd(string input)
        {
            input = input.ToLowerInvariant();
            if (input.Contains('a')
                && input.Contains('b')
                && input.Contains('c')
                && input.Contains('d')
                && input.Contains('e')
                && input.Contains('f')
                && input.Contains('g')
                && input.Contains('h')
                && input.Contains('i')
                && input.Contains('j')
                && input.Contains('k')
                && input.Contains('l')
                && input.Contains('m')
                && input.Contains('n')
                && input.Contains('o')
                && input.Contains('p')
                && input.Contains('q')
                && input.Contains('r')
                && input.Contains('s')
                && input.Contains('t')
                && input.Contains('u')
                && input.Contains('v')
                && input.Contains('w')
                && input.Contains('x')
                && input.Contains('y')
                && input.Contains('z'))
                return true;
            else
                return false;
        }
        public bool ValidateLinq(string input)
        {
            if (validCharacters.ToCharArray().Except(input.ToLowerInvariant().ToCharArray()).Count() > 0)
                return false;
            else
                return true;
        }

    }
}
