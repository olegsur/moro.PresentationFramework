// 
// HBoxArea.cs
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
using System.Linq;
using System.Collections.Generic;

namespace fkalc.UI
{
	public class HBoxArea : Area
	{
		private HStackPanel panel = new HStackPanel ();  
		private ItemsControl itemsControl;

		public HBoxArea ()
		{
			itemsControl = new ItemsControl ()
			{
				ItemsPanel = panel
			};

			Content = itemsControl;

			itemsControl.ItemTemplate = new DataTemplate (Factory);

			BindingOperations.SetBinding (this, "DataContext.Tokens", itemsControl.GetProperty ("ItemsSource"));
		}

		private UIElement Factory (object token)
		{
			var result = new TokenAreaConverter ().Convert (token) as Area;
			//result.Margin = new Thickness (2, 0, 0, 0);

			return result;
		}

		private class HStackPanel : StackPanel
		{
			public HStackPanel ()
			{
				Orientation = Orientation.Horizontal;
			}
			
			protected override Size MeasureOverride (Size availableSize)
			{
				foreach (var child in InternalChildren.Where(c => c.Visibility != Visibility.Collapsed)) {
					child.Measure (availableSize);
				}
				
				var width = InternalChildren.Any () ? InternalChildren.Sum (c => c.DesiredSize.Width + c.Margin.Left + c.Margin.Right) : 0;
				var heightTop = InternalChildren.Any () ? InternalChildren.Max (c => BaseLineCalculator.GetBaseLine (GetArea (c)) + c.Margin.Top) : 0;			
				var heightBottom = InternalChildren.Any () ? InternalChildren.Max (c => c.DesiredSize.Height - BaseLineCalculator.GetBaseLine (GetArea (c)) + c.Margin.Bottom) : 0; 
						
				return new Size (width, heightTop + heightBottom);
			}
			
			protected override void ArrangeOverride (Size finalSize)
			{
				if (!InternalChildren.Any (c => c.Visibility != Visibility.Collapsed))
					return;

				var x = 0d;
				var y = 0d;

				var baseLine = InternalChildren.Where (c => c.Visibility != Visibility.Collapsed)
					.Max (c => BaseLineCalculator.GetBaseLine (GetArea (c)) + c.Margin.Top);

				foreach (var child in InternalChildren.Where(c => c.Visibility != Visibility.Collapsed)) {	
					var width = child.DesiredSize.Width;
					var height = child.DesiredSize.Height;

					x += child.Margin.Left;	

					switch (child.VerticalAlignment) {
					
					case VerticalAlignment.Bottom:
						y = Height - height;
						break;					
					default:
						y = baseLine - BaseLineCalculator.GetBaseLine (GetArea (child));
						break;
					}

					child.Arrange (new Rect (new Point (x, y), new Size (width, height)));
				
					x += child.Width + child.Margin.Right;
				}
			}

			private Area GetArea (UIElement uielement)
			{
				return AreaHelper.GetArea (uielement);
			}
		}
	}
}