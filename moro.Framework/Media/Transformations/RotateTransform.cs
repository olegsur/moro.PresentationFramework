//
// RotateTransform.cs
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
	public class RotateTransform : Transform
	{
		public double Angle { get; private set; }
		public double CenterX { get; private set; }
		public double CenterY { get; private set; }
		public override Matrix Value { get; protected set; }

		public RotateTransform (double angle) : this (angle, 0 ,0)
		{
		}

		public RotateTransform (double angle, double centerX, double centerY)
		{
			Angle = angle;
			CenterX = centerX;
			CenterY = centerY;

			var cos = Math.Cos (angle);
			var sin = Math.Sin (angle);

			var x = -cos * centerX + sin * centerY + centerX;
			var y = -sin * centerX - cos * centerY + centerY;

			Value = new Matrix (cos, -sin, sin, cos, x, y);
		}
	}
}

