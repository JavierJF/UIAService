using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using LanguageExt;
using UIALib.Types;

namespace UIALib.Components
{
    public class Logger : WComp<object>
    {
        public string name => "Logger";
        public string type => "Log";
        public Dictionary<string, string> props
            => new Dictionary<string, string>{};

        /// <summary>
        /// Necessity for debugging ERROR here
        /// </summary>
        public List<string> watchedComps =>
            new List<string> { "AppWindowDetector" };

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            CompLogger.log(this, "Fatal logging Error");
        }

        public void OnNext(object value)
        {
            var msg = value as Event<string>;

            if (msg == null)
            {
                CompLogger.log(this
                              , msg.srcName
                              + msg.srcId
                              + "Type matching error");
            }
            else
            {
                CompLogger.log(this, msg.payload);
            }
        }
    }
}
