using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace Quirk.Graphics
{
    public class Camera
    {
        public Vector3 Position
        {
            get { return _Position; }
            set
            {
                _Position = value;
                RecalculateMatrix();
            }
        }
        private Vector3 _Position = Vector3.Zero;

        public Angle Angles
        {
            get { return _Angles; }
            set
            {
                _Angles = value;
                RecalculateMatrix();
            }
        }
        private Angle _Angles = Angle.Zero;

        /// <summary>
        /// Unit vector representing which way is 'up'. By default, this is Z (0, 0, 1).
        /// </summary>
        public Vector3 Up = new Vector3(0.0f, 0.0f, 1.0f);

        /// <summary>
        /// Gets the Perspective Matrix needed to transform vertices.
        /// </summary>
        public Matrix4 PerspectiveMatrix { get; private set; }

        private void RecalculateMatrix()
        {
            float distance = 5000;
            Vector3 direction = Angles.Forward();

            float bbb = direction.Length;

            Vector3 falseTarget = Position + (direction * distance);

            Vector3 moddedUp = Vector3.TransformVector(Up, Matrix4.CreateRotationY(Angles.Roll));

            PerspectiveMatrix = Matrix4.LookAt(Position, falseTarget, moddedUp);

            int y = 0;
        }
    }
}
