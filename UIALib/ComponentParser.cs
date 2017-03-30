using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LanguageExt;

namespace UIALib
{
    public class ParsingError
    {
        public string file { get; set; }
        public string message { get; set; }
    }

    class ComponentParser
    {
        /// <summary>
        /// Function that returns a list of the components in a file or a 'ParsinError' if
        /// any error.
        /// </summary>
        /// <param name="file">
        /// The file to be parsed.
        /// </param>
        /// <returns>
        /// Either a list of the components or a ParsingError.
        /// </returns>
        public static Either<List<Component>,ParsingError> parseJSON(string filepath)
        {
            throw new NotImplementedException();
        }
    }
}
