﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quirk.Graphics
{
    public interface IGenericBuffer
    {
        void Bind();
        void Unbind();
        void Destroy();
        void WriteData(IntPtr Data, int Length);
        //void WriteData(T[] iboData);
    }
}
