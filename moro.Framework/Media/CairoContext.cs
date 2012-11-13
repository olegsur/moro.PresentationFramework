//
// CairoContext.cs
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
	public class CairoContext : DrawingContext
	{
		private Cairo.Context cr;

		public CairoContext (Cairo.Context cr)
		{
			this.cr = cr;
		}

		public override void DrawLine (Pen pen, Point point0, Point point2)
		{
			cr.Color = new Cairo.Color (pen.Color.R, pen.Color.G, pen.Color.B, pen.Color.Alfa);
			cr.LineWidth = pen.Thickness;

			cr.MoveTo (point0.X, point0.Y);
			cr.LineTo (point2.X, point2.Y);

			cr.Stroke ();
		}

		public override void DrawEllipse (Brush brush, Pen pen, Point center, double radiusX, double radiusY)
		{
			cr.Save ();		

			cr.Translate (center.X, center.Y);
			cr.Scale (1, radiusY / radiusX);

			if (brush != null) {
				if (brush is SolidColorBrush) {
					var b = brush as SolidColorBrush;
					cr.Color = new Cairo.Color (b.Color.R, b.Color.G, b.Color.B, b.Color.Alfa);	
					cr.Arc (0, 0, radiusX, 0, 2 * Math.PI);			
			
					cr.Fill ();
				}
			}

			if (pen != null) {
				cr.Color = new Cairo.Color (pen.Color.R, pen.Color.G, pen.Color.B, pen.Color.Alfa);	
				cr.LineWidth = pen.Thickness;
				cr.Arc (0, 0, radiusX - pen.Thickness / 2, 0, 2 * Math.PI);				
			
				cr.Stroke ();
			}
			
			cr.Restore ();
		}

		public override void DrawText (FormattedText formattedText, Point origin)
		{
			cr.Color = new Cairo.Color (formattedText.Foreground.R, formattedText.Foreground.G, formattedText.Foreground.B, formattedText.Foreground.Alfa);	
			cr.MoveTo (origin.X, origin.Y + formattedText.Height);
			
			cr.SelectFontFace (formattedText.FontFamily, Cairo.FontSlant.Normal, Cairo.FontWeight.Normal);
			cr.SetFontSize (formattedText.FontSize);
			
			cr.ShowText (formattedText.Text);
		}

		public override void DrawRectangle (Brush brush, Pen pen, Rect rectangle)
		{
			if (brush != null) {
				if (brush is SolidColorBrush) {
					var b = brush as SolidColorBrush;
					cr.Color = new Cairo.Color (b.Color.R, b.Color.G, b.Color.B, b.Color.Alfa);	
					cr.Rectangle (rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);			
			
					cr.Fill ();
				} else if (brush is LinearGradientBrush) {
					var b = brush as LinearGradientBrush;
					
					var pattern = new Cairo.LinearGradient (b.StartPoint.X, b.StartPoint.Y, b.EndPoint.X, b.EndPoint.Y);
					foreach (var stop in b.GradientStops) {
						pattern.AddColorStop (stop.Offset, new Cairo.Color (stop.Color.R, stop.Color.G, stop.Color.B, stop.Color.Alfa));						
					}
					
					cr.Pattern = pattern;
					cr.Rectangle (rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);			
					cr.Fill ();
				}
			}

			if (pen != null) {
				cr.Color = new Cairo.Color (pen.Color.R, pen.Color.G, pen.Color.B, pen.Color.Alfa);	
				cr.LineWidth = pen.Thickness;
				cr.Rectangle (rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);			
			
				cr.Stroke ();
			}			
		}

		public override void DrawRoundedRectangle (Brush brush, Pen pen, Rect rectangle, double radiusX, double radiusY)
		{
			var figure = new PathFigure ();
			figure.StartPoint = new Point (rectangle.X + radiusX, rectangle.Y);
			figure.Segments.Add (new LineSegment () { Point = new Point(rectangle.X + rectangle.Width - radiusX, rectangle.Y) });
			figure.Segments.Add (new ArcSegment () 
			    {
					Point = new Point(rectangle.X + rectangle.Width, rectangle.Y + radiusY),
					Size = new Size(radiusX, radiusY),
					SweepDirection = SweepDirection.Clockwise,
					IsLargeArc = false
				}
			);
			figure.Segments.Add (new LineSegment () { Point = new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height - radiusY) });
			figure.Segments.Add (new ArcSegment () 
			    {
					Point = new Point(rectangle.X + rectangle.Width - radiusX, rectangle.Y + rectangle.Height),
					Size = new Size(radiusX, radiusY),
					SweepDirection = SweepDirection.Clockwise,
					IsLargeArc = false
				}
			);
			figure.Segments.Add (new LineSegment () { Point = new Point(rectangle.X + radiusX, rectangle.Y + rectangle.Height) });
			figure.Segments.Add (new ArcSegment () 
			    {
					Point = new Point(rectangle.X, rectangle.Y + rectangle.Height - radiusY),
					Size = new Size(radiusX, radiusY),
					SweepDirection = SweepDirection.Clockwise,
					IsLargeArc = false
				}
			);
			figure.Segments.Add (new LineSegment () { Point = new Point(rectangle.X, rectangle.Y + radiusY) });
			figure.Segments.Add (new ArcSegment () 
			    {
					Point = new Point(rectangle.X + radiusX, rectangle.Y),
					Size = new Size(radiusX, radiusY),
					SweepDirection = SweepDirection.Clockwise,
					IsLargeArc = false
				}
			);
			
			var path = new PathGeometry ();
			path.Figures.Add (figure);			
		}
		
		public override void DrawGeometry (Brush brush, Pen pen, Geometry geometry)
		{
			if (brush != null) {
				if (brush is SolidColorBrush) {
					var b = brush as SolidColorBrush;
					cr.Color = new Cairo.Color (b.Color.R, b.Color.G, b.Color.B, b.Color.Alfa);	
					
					DrawGeometry (geometry);	
			
					cr.Fill ();
				} else if (brush is LinearGradientBrush) {
					var b = brush as LinearGradientBrush;
					
					var pattern = new Cairo.LinearGradient (b.StartPoint.X, b.StartPoint.Y, b.EndPoint.X, b.EndPoint.Y);
					foreach (var stop in b.GradientStops) {
						pattern.AddColorStop (stop.Offset, new Cairo.Color (stop.Color.R, stop.Color.G, stop.Color.B, stop.Color.Alfa));						
					}
					
					cr.Pattern = pattern;
					DrawGeometry (geometry);	
					
					cr.Fill ();
				}
			}
			
			if (pen != null) {
				cr.Color = new Cairo.Color (pen.Color.R, pen.Color.G, pen.Color.B, pen.Color.Alfa);	
				cr.LineWidth = pen.Thickness;

				DrawGeometry (geometry);	
			
				cr.Stroke ();
			}
		}

		private void DrawGeometry (Geometry geometry)
		{
			if (geometry is PathGeometry) {
				var path = geometry as PathGeometry;

				foreach (var figure in path.Figures) {
					DrawFigure (figure);
				}
			}
		}

		private void DrawFigure (PathFigure figure)
		{
			cr.MoveTo (figure.StartPoint.X, figure.StartPoint.Y);

			foreach (var segment in figure.Segments) {
				DrawSegment (segment);
			}
			
			if (figure.IsClosed)
				cr.ClosePath ();
		}

		private void DrawSegment (PathSegment segment)
		{
			if (segment is LineSegment) {
				DrawLineSegment (segment as LineSegment);
			}

			if (segment is ArcSegment) {
				DrawArcSegment (segment as ArcSegment);
			}
		}

		private void DrawLineSegment (LineSegment segment)
		{
			cr.LineTo (segment.Point.X, segment.Point.Y);
		}

		private void DrawArcSegment (ArcSegment segment)
		{
			DrawArcSegment (cr.CurrentPoint.X, cr.CurrentPoint.Y, segment.Point.X, segment.Point.Y, 
			               segment.Size.Width, segment.Size.Height, 
			               segment.RotationAngle, segment.IsLargeArc, segment.SweepDirection);
		}

		private void DrawArcSegment (double xm1, double ym1, double xm2, double ym2, double xr, double yr, double alpha, bool isLargeArc, SweepDirection direction)
		{
			if (xr <= 0.000000001 || yr <= 0.000000001)
				return;

			var x1 = xm1 * Math.Cos (-alpha) - ym1 * Math.Sin (-alpha);
			var y1 = xm1 * Math.Sin (-alpha) + ym1 * Math.Cos (-alpha);

			var x2 = xm2 * Math.Cos (-alpha) - ym2 * Math.Sin (-alpha);
			var y2 = xm2 * Math.Sin (-alpha) + ym2 * Math.Cos (-alpha);

			var r = 0.0;

			if (xr > yr) {
				y1 = y1 * xr / yr;
				y2 = y2 * xr / yr;

				r = xr;

			} else {
				x1 = x1 * yr / xr;
				x2 = x2 * yr / xr;

				r = yr;
			}

			if (4 * r * r < (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2))
				return;

			var xc1 = 0.0;
			var xc2 = 0.0;
			var yc1 = 0.0;
			var yc2 = 0.0;

			if (Math.Abs (y1 - y2) > 0.000000001) {
				var A = (x1 - x2) / (y2 - y1);
				var B = (x2 * x2 - x1 * x1 + y2 * y2 - y1 * y1) / (2 * (y2 - y1));

				var a = A * A + 1;
				var b = -2 * x1 + 2 * A * B - 2 * A * y1;
				var c = x1 * x1 + B * B - 2 * B * y1 + y1 * y1 - r * r;


				xc1 = (-b + Math.Sqrt (b * b - 4 * a * c)) / (2 * a);
				yc1 = A * xc1 + B;

				xc2 = (-b - Math.Sqrt (b * b - 4 * a * c)) / (2 * a);
				yc2 = A * xc2 + B;
			} else {
				xc1 = (x1 + x2) / 2;
				yc1 = y1 + Math.Sqrt (r * r - (xc1 - x1) * (xc1 - x1));

				xc2 = (x1 + x2) / 2;
				yc2 = y1 - Math.Sqrt (r * r - (xc2 - x1) * (xc2 - x1));
				
			}

			var angle1 = Math.Abs (y1 - yc1) / r > 1 ? Math.PI / 2 : Math.Asin (Math.Abs (y1 - yc1) / r);
			if ((x1 < xc1) && (y1 >= yc1))
				angle1 = Math.PI - angle1;
			if (y1 < yc1)
			if (x1 < xc1)
				angle1 = Math.PI + angle1;
			else 
				angle1 = - angle1;
			
			var angle2 = Math.Abs (y2 - yc1) / r > 1 ? Math.PI / 2 : Math.Asin (Math.Abs (y2 - yc1) / r);
			if ((x2 < xc1) && (y2 >= yc1))
				angle2 = Math.PI - angle2;
			if (y2 < yc1) {
				if (x2 < xc1)
					angle2 = Math.PI + angle2;
				else 
					angle2 = - angle2;
			}
			
			var alfa1 = Math.Abs (y1 - yc2) / r > 1 ? Math.PI / 2 : Math.Asin (Math.Abs (y1 - yc2) / r);
			
			if ((x1 < xc2) && (y1 >= yc2))
				alfa1 = Math.PI - alfa1;
			if (y1 < yc2) {
				if (x1 < xc2)
					alfa1 = Math.PI + alfa1;
				else 
					alfa1 = - alfa1;
			}

			var alfa2 = Math.Abs (y2 - yc2) / r > 1 ? Math.PI / 2 : Math.Asin (Math.Abs (y2 - yc2) / r);
			if ((x2 < xc2) && (y2 >= yc2))
				alfa2 = Math.PI - alfa2;
			if (y2 < yc2) {
				if (x2 < xc2)
					alfa2 = Math.PI + alfa2;
				else 
					alfa2 = - alfa2;
			}
			
			cr.Save ();

			cr.Rotate (alpha);

			if (xr > yr) 			
				cr.Scale (1, yr / xr);
			else
				cr.Scale (xr / yr, 1);

			if (direction == SweepDirection.Clockwise) {	
				if (isLargeArc) {
					if ((y1 < y2) || ((Math.Abs (y1 - y2) < 0.00000001) && (x1 > x2))) 
						cr.Arc (xc1, yc1, r, angle1, angle2);
					else 
						cr.Arc (xc2, yc2, r, alfa1, alfa2);
						
				} else {			
					if ((y1 > y2) || ((Math.Abs (y1 - y2) < 0.00000001) && (x1 < x2))) 
						cr.Arc (xc1, yc1, r, angle1, angle2);
					else 
						cr.Arc (xc2, yc2, r, alfa1, alfa2);									
				}

			} else {
				
				if (isLargeArc) {
					if ((y1 > y2) || ((Math.Abs (y1 - y2) < 0.00000001) && (x1 < x2))) 
						cr.ArcNegative (xc1, yc1, r, angle1, angle2);
					else 
						cr.ArcNegative (xc2, yc2, r, alfa1, alfa2);
						
				} else {					
				
					if ((y1 < y2) || ((Math.Abs (y1 - y2) < 0.00000001) && (x1 > x2))) 
						cr.ArcNegative (xc1, yc1, r, angle1, angle2);
					else 
						cr.ArcNegative (xc2, yc2, r, alfa1, alfa2);
									
				}
			}

			cr.Restore ();
		}

		public override void PushTransform (Transform transform)
		{
			cr.Save ();

			var m = transform.Value;
			cr.Transform (new Cairo.Matrix (m.M11, m.M21, m.M12, m.M22, m.OffsetX, m.OffsetY));
		}

		public override void Pop ()
		{
			cr.Restore ();
		}

		public override Antialias Antialias {
			get {
				switch (cr.Antialias) {
				case Cairo.Antialias.Default:
					return Antialias.Default;
				case Cairo.Antialias.None:
					return Antialias.None;
				case Cairo.Antialias.Gray:
					return Antialias.Gray;
				case Cairo.Antialias.Subpixel:
					return Antialias.Subpixel;
				default:
					return Antialias.Default;
				}
			}
			set {
				switch (value) {
				case Antialias.Default:
					cr.Antialias = Cairo.Antialias.Default;
					break;
				case Antialias.None:
					cr.Antialias = Cairo.Antialias.None;
					break;
				case Antialias.Gray:
					cr.Antialias = Cairo.Antialias.Gray;
					break;
				case Antialias.Subpixel:
					cr.Antialias = Cairo.Antialias.Subpixel;
					break;
				default:
					cr.Antialias = Cairo.Antialias.Default;
					break;
				}
			}
		}


	}
}

