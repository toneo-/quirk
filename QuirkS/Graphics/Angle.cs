using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace Quirk.Graphics
{
    public struct Angle
    {
        public float Pitch, Yaw, Roll;

        /// <summary>
        /// Gets or sets the Pitch component of this angle in radians.
        /// </summary>
        public float PitchRads {
            get
            {
                return (float)(Pitch * Deg2Rad);
            }

            set
            {
                Pitch = (float)(value * Rad2Deg);
            }
        }

        /// <summary>
        /// Gets or sets the Yaw component of this angle in radians.
        /// </summary>
        public float YawRads
        {
            get
            {
                return (float)(Yaw * Deg2Rad);
            }

            set
            {
                Yaw = (float)(value * Rad2Deg);
            }
        }

        /// <summary>
        /// Gets or sets the Roll component of this angle in radians.
        /// </summary>
        public float RollRads
        {
            get
            {
                return (float)(Roll * Deg2Rad);
            }

            set
            {
                Roll = (float)(value * Rad2Deg);
            }
        }

        public static readonly Angle Zero = new Angle(0, 0, 0);

        private const double Deg2Rad = (Math.PI / 180);
        private const double Rad2Deg = (180 / Math.PI);

        public Angle(float Pitch, float Yaw, float Roll)
        {
            this.Pitch = Pitch;
            this.Yaw = Yaw;
            this.Roll = Roll;
        }

        public Vector3 Forward()
        {
            Vector3 forwardVec = new Vector3(
                    (float)(Math.Sin(YawRads) * Math.Cos(PitchRads)),
                    (float)(Math.Cos(YawRads) * Math.Cos(PitchRads)),
                    (float)(Math.Sin(PitchRads))
                );

            return forwardVec;
        }

        public Vector3 Right()
        {
            // Must be rotated by 90 degrees yaw (z-axis) and then 'pitched up' by however much roll the camera has.

            Vector3 forwardVec = this.Forward();
            Vector3 rightVec = Vector3.TransformVector(forwardVec, Matrix4.CreateRotationZ((float)(-90 * Deg2Rad)) * Matrix4.CreateRotationY(RollRads));

            return rightVec;
        }

        public Vector3 Up()
        {
            throw new NotImplementedException();
        }
    }
}
