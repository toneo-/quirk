﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quirk.Graphics
{
    public interface IMesh
    {
        void Bind();
        void Unbind();
        void Destroy();

        void Draw();
    }
}
