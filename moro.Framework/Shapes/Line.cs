// 
// Line.cs
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
	public class Line : Shape
	{
		public double? X1 { get; set; }		
		public double? Y1 { get; set; }
		
		public double? X2 { get; set; }
		public double? Y2 { get; set; }
		
		
		public Line ()
		{
		}
		
		protected override Size MeasureOverride (Size availableSize)
		{
			var width = X1.HasValue && X2.HasValue ? Math.Abs (X1.Value - X2.Value) : 0;
			var height = Y1.HasValue && Y2.HasValue ? Math.Abs (Y1.Value - Y2.Value) : 0;
			
			return new Size (width, height);
		}				
		
		protected override void OnRender (DrawingContext dc)
		{
			var anialias = dc.Antialias;
			
			dc.Antialias = SnapsToDevicePixels ? Antialias.None : anialias;

			dc.DrawLine (new Pen (Stroke, StrokeThickness), 
			             new Point (StrokeThickness / 2, StrokeThickness / 2), 
			             new Point (Width - StrokeThickness / 2, Height - StrokeThickness / 2));

			dc.Antialias = anialias;
		}
	}
}

