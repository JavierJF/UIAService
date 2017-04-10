using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIALib.Types
{

    internal interface IImageFilter<out I, out O>
    {
        I Input { get; }

        O Process();
    }

    class Experimental
    {
        public static void testing()
        {
            List<IImageFilter<object, object>> filters =
                new List<IImageFilter<object, object>>();

            ImageFilter filter = new ImageFilter("Something");
            filters.Add(filter);

            ImageFilter type = filters[0] as ImageFilter;
        }
    }

    public class ImageFilter : IImageFilter<string, string>
    {
        public string Input { get; private set; }

        public ImageFilter(string input)
        {
            Input = input;
        }

        public string Process()
        {
            return Input.ToUpper();
        }
    }
}
