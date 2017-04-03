using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using LanguageExt;
using static LanguageExt.Prelude;
using UIALib;
using static UIALib.CompParser;
using System.IO;

namespace UIALib_UnitTest
{
    [TestFixture]
    public class ParsingTests
    {
        public static List<CompSpec> tComponents =
            new List<CompSpec>
            {
                new CompSpec { name = "Microsoft-Word", type = "Application-Window" },
                new CompSpec { name = "View"
                               , type = "Application-Menu"
                               , watch = new List<LEventSpec> {
                                   new LEventSpec { type = "Component-Instantiation"
                                                  , source = "Microsoft-Word" }
                                 }
                               },
                new CompSpec { name = "Column-width"
                               , type = "Menu"
                               , watch = new List<LEventSpec> {
                                   new LEventSpec { type = "Menu-Expansion"
                                                  , source = "View" }
                                 }
                               },
                new CompSpec { name = "Narrow"
                               , type = "Menu-Element"
                               , watch = new List<LEventSpec> {
                                   new LEventSpec { type = "Menu-Expansion"
                                                  , source = "Column-width" }
                                 }
                               , emit = new List<EEventSpec> {
                                   new EEventSpec { type = "Invoked-Element" }
                                 }
                               },
                new CompSpec { name = "Logger"
                               , type = "Runtime-Logger"
                               , watch = new List<LEventSpec> {
                                   new LEventSpec { type = "*" }
                                 }
                               }
            };

        [Test]
        public void fileParsing()
        {
            var filepath =
               Path.Combine(TestContext.CurrentContext.TestDirectory
                           , "data/tests/parsing-components.json");

            var res = parseJSON(filepath);

            res.Match(
                Left: (components) =>
                {
                    var normalcheck = components.Equals(tComponents);
                    Assert.AreEqual(components, tComponents);
                },
                Right: (error) =>
                {
                    Assert.Fail(error.message);
                }
            );
        }
    }
}
