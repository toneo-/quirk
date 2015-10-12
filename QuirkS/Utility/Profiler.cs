using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quirk.Utility
{
    public class Profiler
    {
        private static Dictionary<string, DateTime> Sections = new Dictionary<string, DateTime>();

        public static void Start(string SectionName)
        {
            Sections[SectionName] = DateTime.Now;
        }

        public static double End(string SectionName)
        {
            if (!Sections.ContainsKey(SectionName))
                throw new ArgumentException("Section '" + SectionName + "' hasn't been started yet.");

            DateTime startTime = Sections[SectionName];

            // Time elapsed
            return (DateTime.Now - startTime).TotalSeconds;
        }

        public static double TimeDelegate(Action Method)
        {
            DateTime startTime = DateTime.Now;

            Method.Invoke();

            return (DateTime.Now - startTime).TotalSeconds;
        }
    }
}
