﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quirk.Graphics
{
    public interface IIndexBuffer<T> : IGenericBuffer where T : struct
    {
        void WriteData(T[] Data);
    }
}
