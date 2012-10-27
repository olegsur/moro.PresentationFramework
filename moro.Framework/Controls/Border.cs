// 
// Border.cs
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
using moro.Framework.Data;

namespace moro.Framework
{
	public class Border : Decorator
	{ 
		private DependencyProperty<Thickness> padding;		
		private DependencyProperty<Brush> background;
		private DependencyProperty<Color> borderColor;
		private DependencyProperty<double> borderThickness;	
		private DependencyProperty<CornerRadius> cornerRadius;
		
		public Thickness Padding { 
			get { return padding.Value;}
			set { padding.Value = value;}
		}
		
		public Brush Background { 
			get { return background.Value;}
			set { background.Value = value;}
		}
		
		public Color BorderColor { 
			get { return borderColor.Value;}
			set { borderColor.Value = value;}
		}
		
		public double BorderThickness { 
			get { return borderThickness.Value;}
			set { borderThickness.Value = value;}
		}
		
		public CornerRadius CornerRadius { 
			get { return cornerRadius.Value;}
			set { cornerRadius.Value = value;}
		}
		
		public Border ()
		{
			padding = BuildProperty<Thickness> ("Padding");
			background = BuildProperty<Brush> ("Background");
			borderColor = BuildProperty<Color> ("BorderColor");
			borderThickness = BuildProperty<double> ("BorderThickness");
			cornerRadius = BuildProperty<CornerRadius> ("CornerRadius");
					
			BorderColor = Colors.Black;
			BorderThickness = 1;
			Padding = new Thickness (5);
		}

		protected override Size MeasureOverride (Size availableSize)
		{
			if (Child == null)
				return new Size (0, 0);
			
			Child.Measure (availableSize);
			
			var width = Child.DesiredSize.Width + Padding.Left + Padding.Right + BorderThickness;
			var height = Child.DesiredSize.Height + Padding.Top + Padding.Bottom + BorderThickness;
			
			return new Size (width, height);
		}

		protected override void ArrangeOverride (Size finalSize)
		{
			if (Child == null)
				return;

			Child.Arrange (new Rect (Padding.Left + BorderThickness / 2, 
			                         Padding.Top + BorderThickness / 2,
			                         finalSize.Width - Padding.Left - Padding.Right - BorderThickness / 2, 
			                         finalSize.Height - Padding.Top - Padding.Bottom - BorderThickness / 2));
		}

		protected override void OnRender (DrawingContext dc)
		{		
			if (Child == null)
				return;
			
			var x = BorderThickness / 2;
			var y = BorderThickness / 2;
			var width = Width - BorderThickness;
			var height = Height - BorderThickness;

			var figure = new PathFigure ();
			figure.StartPoint = new Point (x + CornerRadius.TopLeft, y);
			figure.IsClosed = true;
			
			figure.Segments.Add (new LineSegment () { Point = new Point(x + width - CornerRadius.TopRight, y)});
			if (CornerRadius.TopRight > 0) {
				figure.Segments.Add (new ArcSegment () 
			    	{
						Point = new Point(x + width, y + CornerRadius.TopRight),
						Size = new Size(CornerRadius.TopRight, CornerRadius.TopRight),
						SweepDirection = SweepDirection.Clockwise,
						IsLargeArc = false
					}
				);
			}
			figure.Segments.Add (new LineSegment () { Point = new Point(x + width, y + height - CornerRadius.BottomRight) });
			if (CornerRadius.BottomRight > 0) {
				figure.Segments.Add (new ArcSegment () 
			    	{
						Point = new Point(x + width - CornerRadius.BottomRight, y + height),
						Size = new Size(CornerRadius.BottomRight, CornerRadius.BottomRight),
						SweepDirection = SweepDirection.Clockwise,
						IsLargeArc = false
					}
				);
			}
			figure.Segments.Add (new LineSegment () { Point = new Point(x + CornerRadius.BottomLeft, y + height) });
			if (CornerRadius.BottomLeft > 0) {
				figure.Segments.Add (new ArcSegment () 
				    {
						Point = new Point(x, y + height - CornerRadius.BottomLeft),
						Size = new Size(CornerRadius.BottomLeft, CornerRadius.BottomLeft),
						SweepDirection = SweepDirection.Clockwise,
						IsLargeArc = false
					}
				);
			}
			figure.Segments.Add (new LineSegment () { Point = new Point(x, y + CornerRadius.TopLeft) });
			if (CornerRadius.TopLeft > 0) {				
				figure.Segments.Add (new ArcSegment () 
			    	{
						Point = new Point(x + CornerRadius.TopLeft, y),
						Size = new Size(CornerRadius.TopLeft, CornerRadius.TopLeft),
						SweepDirection = SweepDirection.Clockwise,
						IsLargeArc = false
					}
				);
			}
			
			var path = new PathGeometry ();
			path.Figures.Add (figure);				
			
			//todo: resove it
			var brush = Background;
			
			if (brush is LinearGradientBrush) {
				var b = brush as LinearGradientBrush;
				var newBrush = new LinearGradientBrush ()
				{
					StartPoint = new Point (b.StartPoint.X * width, b.StartPoint.Y * height),
					EndPoint = new Point (b.EndPoint.X * width, b.EndPoint.Y * height)
				};
				
				newBrush.GradientStops.AddRange (b.GradientStops);
				
				brush = newBrush;
			}
			
			dc.DrawGeometry (brush, new Pen (BorderColor, BorderThickness), path);
		}
	}
}

