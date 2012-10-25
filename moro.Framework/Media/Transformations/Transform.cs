//
// Transform.cs
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
using System.Linq;

namespace moro.Framework
{
	public abstract class Transform
	{
		public abstract Matrix Value { get; protected set; }

		public Transform ()
		{
		}

		public Point TransformPoint (Point point)
		{
			return Value.Transform (point);
		}

		public Transform Inverse {
			get {
				return new MatrixTransform (Value.Inverse ());
			}
		}

		public Rect TransformBounds (Rect rect)
		{
			var inverse = this;
			var points = new [] {
				inverse.TransformPoint (rect.TopLeft),
				inverse.TransformPoint (rect.TopRight),
				inverse.TransformPoint (rect.BottomLeft) ,
				inverse.TransformPoint (rect.BottomRight)
			};

			var x1 = points.Min (p => p.X);
			var y1 = points.Min (p => p.Y);

			var x2 = points.Max (p => p.X);
			var y2 = points.Max (p => p.Y);

			return new Rect (x1, y1, x2 - x1, y2 - y1);
		}
	}
}

