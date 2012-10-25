//
// Matrix.cs
//
// Author:
//       Oleg Sur <oleg.sur@gmail.com>
//
// Copyright (c) 2012 Oleg Sur
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;

namespace moro.Framework
{
	public struct Matrix
	{
		private static Matrix identity = new Matrix (1, 0, 0, 1, 0, 0);
		public static Matrix Identity { get { return identity; } }

		public double M11 { get; private set; }
		public double M12 { get; private set; }
		public double M21 { get; private set; }
		public double M22 { get; private set; }
		public double OffsetX { get; private set; }
		public double OffsetY { get; private set; }

		public Matrix (double m11, double m12, double m21, double m22, double offsetX, double offsetY) : this()
		{
			M11 = m11;
			M12 = m12;
			M21 = m21;
			M22 = m22;
			OffsetX = offsetX;
			OffsetY = offsetY;
		}

		public Point Transform (Point point)
		{
			return new Point (M11 * point.X + M12 * point.Y + OffsetX, M21 * point.X + M22 * point.Y + OffsetY);
		}

		public Matrix Inverse ()
		{
			var determinant = Determinant ();

			var a11 = M22 / determinant;
			var a12 = - M12 / determinant;
			var a21 = - M21 / determinant;
			var a22 = M11 / determinant;

			var offsetX = -(a11 * OffsetX + a12 * OffsetY);
			var offsetY = -(a21 * OffsetX + a22 * OffsetY);

			return new Matrix (a11, a12, a21, a22, offsetX, offsetY);
		}

		private double Determinant ()
		{
			return M11 * M22 - M12 * M21;
		}

		public static Matrix operator * (Matrix m1, Matrix m2)
		{
			var a11 = m1.M11 * m2.M11 + m1.M12 * m2.M21;
			var a12 = m1.M11 * m2.M12 + m1.M12 * m2.M22;

			var a21 = m1.M21 * m2.M11 + m1.M22 * m2.M21;
			var a22 = m1.M21 * m2.M12 + m1.M22 * m2.M22;

			var offsetX = m1.M11 * m2.OffsetX + m1.M12 * m2.OffsetY + m1.OffsetX;
			var offsetY = m1.M21 * m2.OffsetX + m1.M22 * m2.OffsetY + m1.OffsetY;

			return new Matrix (a11, a12, a21, a22, offsetX, offsetY);
		}
	}
}

