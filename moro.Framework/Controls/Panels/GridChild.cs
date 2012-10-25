//
// GridChild.cs
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
	public class GridChild : ContentControl
	{
		public uint Row { get; set; }
		public uint Column { get ; set; }

		public GridChild (UIElement child)
		{
			Content = child;

			BindingOperations.SetBinding (child.GetProperty ("Visibility"), GetProperty ("Visibility"));
			
			HorizontalAlignment = HorizontalAlignment.Stretch;
			VerticalAlignment = VerticalAlignment.Stretch;
		}	
		
		protected override Size MeasureOverride (Size availableSize)
		{
			Content.Measure (availableSize);
			
			var margin = Content.GetProperty ("Margin") != null ? 
				(Thickness)Content.GetProperty ("Margin").Value : new Thickness(0);
			
			return new Size(Content.DesiredSize.Width + margin.Left + margin.Right, Content.DesiredSize.Height + margin.Bottom + margin.Top);
		}
		
		protected override void ArrangeOverride (Size finalSize)
		{			
			var horizontalAligment = Content.GetProperty ("HorizontalAlignment") != null ?
				(HorizontalAlignment)Content.GetProperty ("HorizontalAlignment").Value : HorizontalAlignment.Stretch;
			
			var verticalAligment = Content.GetProperty ("VerticalAlignment") != null ?
				(VerticalAlignment)Content.GetProperty ("VerticalAlignment").Value : VerticalAlignment.Stretch;
			
			var margin = Content.GetProperty ("Margin") != null ? 
				(Thickness)Content.GetProperty ("Margin").Value : new Thickness (0);
			
			var x = margin.Left;
			var y = margin.Top;
			var width = Content.DesiredSize.Width;
			var height = Content.DesiredSize.Height;
			
			switch (horizontalAligment) {
			case HorizontalAlignment.Right:
				x = Width - width;
				break;
			case HorizontalAlignment.Center:
				x = (Width - width) / 2;
				break;
			case HorizontalAlignment.Stretch:
				width = finalSize.Width;
				break;			
			}			
			
			switch (verticalAligment){
			case VerticalAlignment.Bottom:
				y = Height - height;
				break;
			case VerticalAlignment.Center:
				x = (Height - height) / 2;
				break;
			case VerticalAlignment.Stretch:
				height = finalSize.Height;
				break;		
			}
				
			Content.Arrange (new Rect (x, y, width - margin.Left - margin.Right, height - margin.Top - margin.Bottom));
		}
	}
}

