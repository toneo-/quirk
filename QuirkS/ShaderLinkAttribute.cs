using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace Quirk
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class ShaderLinkAttribute : Attribute
    {
        public readonly string LinkedName;
        public readonly Type ComponentType;
        public readonly int ComponentSize;
        public readonly int ComponentCount;
        public readonly bool Normalised;

        public ShaderLinkAttribute(string LinkedName, Type ComponentType, int ComponentCount, bool Normalised = false)
        {
            this.LinkedName = LinkedName;
            this.ComponentType = ComponentType;
            this.ComponentSize = Marshal.SizeOf(ComponentType);
            this.ComponentCount = ComponentCount;
            this.Normalised = Normalised;
        }
    }
}
