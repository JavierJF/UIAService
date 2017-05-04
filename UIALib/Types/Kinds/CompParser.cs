/*
 * The R&D leading to these results received funding from the
 * Department of Education - Grant H421A150005 (GPII-APCP). However,
 * these results do not necessarily represent the policy of the
 * Department of Education, and you should not assume endorsement by the
 * Federal Government.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LanguageExt;
using System.IO;

namespace UIALib
{
    /// <summary>
    /// File parsing error.
    /// </summary>
    public class ParsingError
    {
        public string file { get; set; }
        public string message { get; set; }
    }

    public class CompParser
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
        public static Either<List<CompSpec>,ParsingError> parseJSON(string filepath)
        {
            try
            {
                string fileText = File.ReadAllText(filepath);
                List<string> errors = new List<string>();

                JObject jFile = JObject.Parse(fileText);

                JToken compToken = jFile["Components"];

                if (compToken == null)
                {
                    return
                        new ParsingError
                            { file = filepath
                            , message = "No 'Components' definition in file" };
                }

                var jComponents = compToken.Children().ToList<JToken>();
                List<CompSpec> components = new List<CompSpec>();
                string mError = null;

                var serializer = new JsonSerializer();
                serializer.Error +=
                    new EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs>(
                        (objt, args) => {
                            mError = args.ErrorContext.Error.Message;
                        }
                    );
                serializer.NullValueHandling = NullValueHandling.Ignore;

                foreach (JToken token in jComponents)
                {
                    var newComponent = token.ToObject<CompSpec>(serializer);
                    components.Add(newComponent);

                    if (mError != null)
                    {
                        break;
                    }
                }

                if (errors.Any())
                {
                    return new ParsingError { file = filepath
                                            , message = "Errors: " + errors };
                }
                else
                {
                    return components;
                }
            }
            catch (Exception ex)
            {
                return new ParsingError { file = filepath
                                        , message = "Message: " + ex.Message
                                                    + "\r\nTrace:" +  ex.StackTrace };
            }
        }
    }
}
