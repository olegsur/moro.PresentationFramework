//
// DrawingContext.cs
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
	public abstract class DrawingContext
	{
		public abstract Antialias Antialias { get; set; }

		public DrawingContext ()
		{
		}

		public abstract void PushTransform (Transform transform);
		public abstract void Pop ();

		public abstract void DrawLine (Pen pen, Point point0, Point point1);
		public abstract void DrawEllipse (Brush brush, Pen pen, Point center, double radiusX, double radiusY);
		public abstract void DrawText (FormattedText formattedText, Point origin);
		public abstract void DrawRectangle (Brush brush, Pen pen, Rect rectangle);
		public abstract void DrawRoundedRectangle (Brush brush, Pen pen, Rect rectangle, double radiusX, double radiusY);
		public abstract void DrawGeometry (Brush brush, Pen pen, Geometry geometry);	
	}
}
