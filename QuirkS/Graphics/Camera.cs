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
        public Vector3 UpVector = new Vector3(0.0f, 0.0f, 1.0f);

        /// <summary>
        /// Gets the Perspective Matrix needed to transform vertices.
        /// </summary>
        public Matrix4 PerspectiveMatrix { get; private set; }

        /// <summary>
        /// Gets the World transformation matrix.
        /// </summary>
        public Matrix4 WorldMatrix { get; private set; }

        private void RecalculateMatrix()
        {
            float distance = 5000;
            Vector3 direction = Angles.Forward();

            Vector3 falseTarget = Position + (direction * distance);

            // Rotate "up" vector by the camera's roll angle.
            Vector3 rotatedUp = Vector3.TransformVector(UpVector, Matrix4.CreateRotationY(Angles.RollRads));

            PerspectiveMatrix = Matrix4.LookAt(Position, falseTarget, rotatedUp);
            WorldMatrix = Matrix4.Translation(-Position);
        }
    }
}
