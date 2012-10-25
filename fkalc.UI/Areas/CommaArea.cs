//
// CommaArea.cs
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
using moro.Framework;

namespace fkalc.UI
{
	public class CommaArea : Area
	{
		public CommaArea ()
		{
			var figure = new PathFigure ();
			figure.StartPoint = new Point (4, 2);

			figure.Segments.Add (new ArcSegment () 
			{ 
				Point = new Point (2, 2), Size = new Size (1, 1), RotationAngle = Math.PI, IsLargeArc = true, SweepDirection = SweepDirection.Clockwise
			}
			);

			figure.Segments.Add (new ArcSegment () 
			{ 
				Point = new Point (4, 2), Size = new Size (1, 1), RotationAngle = Math.PI, IsLargeArc = true, SweepDirection = SweepDirection.Clockwise
			}
			);

			figure.Segments.Add (new ArcSegment () 
			{ 
				Point = new Point (2, 6), Size = new Size (12, 12), RotationAngle = Math.PI, IsLargeArc = false, SweepDirection = SweepDirection.Clockwise
			}
			);
				
			var geometry = new PathGeometry ();
			geometry.Figures.Add (figure);
			var path = new Path ();
			path.Data = geometry;

			Content = path;

			HeightRequest = 3;
			WidthRequest = 3;

			VerticalAlignment = VerticalAlignment.Bottom;
			Margin = new Thickness (2, 0, 2, 0);
		}
	}
}

