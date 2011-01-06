using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XEngine {
    interface ICamera {

        Matrix View { get; }

        Matrix InverseView { get; }

        Matrix Projection { get; }

        Vector3 Position { get; set; }

        Vector3 LookAt { get; set; }

        Vector3 Up { get; set; }

        Vector3 Right { get; }

        Vector3 LookDirection { get; }
    }
}
