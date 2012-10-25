//
// Rect.cs
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
	public struct Rect
	{
		public double X { get; private set; }
		public double Y { get; private set; }
		public double Width { get; private set; }
		public double Height { get; private set; }

		public Size Size { get; private set; }

		public Point TopLeft { get; private set; }
		public Point TopRight { get; private set; }
		public Point BottomLeft { get; private set; }
		public Point BottomRight { get; private set; }

		public Rect (double x, double y, double width, double height) : this()
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;

			Size = new Size (Width, Height);

			TopLeft = new Point (x, y);
			TopRight = new Point (x + width, y);
			BottomLeft = new Point (x, y + height);
			BottomRight = new Point (x + width, y + height);
		}

		public Rect (Size size) : this (0,0, size.Width, size.Height)
		{
		}

		public Rect (Point location, Size size) : this (location.X, location.Y, size.Width, size.Height)
		{
		}
	}
}

