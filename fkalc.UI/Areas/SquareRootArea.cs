//
// SquareRootArea.cs
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
	public class SquareRootArea : Area
	{
		public ContentControl Child { get; private set; }

		public SquareRootArea ()
		{
			Child = new ContentControl ()
			{
				Content = new TextArea (),
				Margin = new Thickness(12, 3 ,2, 0)
			};

			var grid = new Grid ();
			grid.Children.Add (Child);
			grid.Children.Add (new SquareRootPath ());

			Content = grid;

			BindingOperations.SetBinding (this, "DataContext.Child", Child.GetProperty ("Content"), new TokenAreaConverter ());
		}

		private class SquareRootPath : UserControl
		{
			public SquareRootPath ()
			{
				VerticalAlignment = VerticalAlignment.Stretch;
				HorizontalAlignment = HorizontalAlignment.Stretch;
			}

			protected override void OnRender (DrawingContext dc)
			{
				base.OnRender (dc);

				var figure = new PathFigure ()
				{
					StartPoint = new Point (0, 0.3 * Height)
				};

				figure.Segments.Add (new LineSegment () { Point = new Point (3, 0.3 * Height) });
				figure.Segments.Add (new LineSegment () { Point = new Point (6, Height) });
				figure.Segments.Add (new LineSegment () { Point = new Point (10, 0) });
				figure.Segments.Add (new LineSegment () { Point = new Point (Width + 2, 0) });
				
				var geometry = new PathGeometry ();
				geometry.Figures.Add (figure);

				dc.DrawGeometry (null, new Pen (Colors.Black, 2), geometry);
			}
		}
	}
}

