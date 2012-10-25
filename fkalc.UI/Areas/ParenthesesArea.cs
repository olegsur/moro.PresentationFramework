//
// ParenthesesArea.cs
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
	public class ParenthesesArea : Area
	{
		public ContentControl Child { get; private set; }

		public ParenthesesArea ()
		{
			Child = new ContentControl ()
			{
				Content = new TextArea (),
				VerticalAlignment = VerticalAlignment.Center
			};

			var stackPanel = new StackPanel ()
			{
				Orientation = Orientation.Horizontal
			};

			stackPanel.Children.Add (new OpenParentheses ());
			stackPanel.Children.Add (Child);

			var closeParentheses = new CloseParentheses ();
			BindingOperations.SetBinding (this, "DataContext.ShowCloseParentheses", closeParentheses.GetProperty ("Visibility"), new BooleanToVisibilityConverter ());

			stackPanel.Children.Add (closeParentheses);

			Content = stackPanel;

			BindingOperations.SetBinding (this, "DataContext.Child", Child.GetProperty ("Content"), new TokenAreaConverter ());		
		}

		private class Parentheses : UserControl
		{
			public Parentheses ()
			{
				WidthRequest = 6;
				VerticalAlignment = VerticalAlignment.Stretch;
				Margin = new Thickness (2, 4, 0, 4);
			}

			protected void DrawArc (DrawingContext dc, Point start, Point end, Size size, SweepDirection sweepDirection)
			{
				var figure = new PathFigure ();
				figure.StartPoint = start;

				figure.Segments.Add (new ArcSegment () 
				{ 
					Point = end, Size = size, SweepDirection = sweepDirection
				}
				);
				
				var geometry = new PathGeometry ();
				geometry.Figures.Add (figure);

				dc.DrawGeometry (null, new Pen (Colors.Black, 2), geometry);
			}
		}

		private class OpenParentheses : Parentheses
		{
			protected override void OnRender (DrawingContext dc)
			{
				base.OnRender (dc);
				DrawArc (dc, new Point (5, -3), new Point (5, Height + 3), new Size (10, Height), SweepDirection.Counterclockwise);
			}
		}

		private class CloseParentheses : Parentheses
		{
			protected override void OnRender (DrawingContext dc)
			{
				base.OnRender (dc);
				DrawArc (dc, new Point (0, -3), new Point (0, Height + 3), new Size (10, Height), SweepDirection.Clockwise);
			}
		}
	}
}

