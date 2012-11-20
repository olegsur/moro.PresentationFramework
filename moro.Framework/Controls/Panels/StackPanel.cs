// 
// StackPanel.cs
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
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace moro.Framework
{
	public class StackPanel : Panel
	{	
		private List<StackPanelChild> children = new List<StackPanelChild> ();
		protected IEnumerable<StackPanelChild> InternalChildren { get { return children; } }
		
		public Orientation Orientation { get; set; }
		
		public StackPanel ()
		{
			base.Children.CollectionChanged += HandleCollectionChanged;

			Orientation = Orientation.Vertical;
		}		

		private void HandleCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add) {
				var index = e.NewStartingIndex;

				foreach (var uielement in e.NewItems.Cast<UIElement>()) {
					var child = new StackPanelChild (uielement);
			
					AddVisualChild (child);
					children.Insert (index, child);

					index++;
				}
			}

			if (e.Action == NotifyCollectionChangedAction.Remove) {
				var index = e.OldStartingIndex;
				foreach (var uielement in e.OldItems.Cast<UIElement>()) {
					var child = children [index];
			
					RemoveVisualChild (child);			
					children.Remove (child);

					index++;
				}
			}

			if (e.Action == NotifyCollectionChangedAction.Replace) {
				var index = e.NewStartingIndex;

				foreach (var uielement in e.NewItems.Cast<UIElement>()) {
					var child = new StackPanelChild (uielement);
			
					RemoveVisualChild (children [index]);

					AddVisualChild (child);
					children [index] = child;

					index++;
				}
			}

			if (e.Action == NotifyCollectionChangedAction.Reset) {
				children.Clear ();
			}
		}
						
		protected override Size MeasureOverride (Size availableSize)
		{
			var collection = children.Where (c => c.Visibility != Visibility.Collapsed);

			if (!collection.Any ())
				return new Size ();

			foreach (var child in collection) {
				child.Measure (availableSize);
			}
			
			var width = 0d;
			var height = 0d;			
						
			if (Orientation == Orientation.Horizontal) {
				width = collection.Sum (c => c.DesiredSize.Width + c.Margin.Left + c.Margin.Right);

				height = collection.Where (c => c.DesiredSize.Height > 0).Max (c => c.DesiredSize.Height + c.Margin.Top + c.Margin.Bottom);
				height += collection.Any (c => c.DesiredSize.Height == 0) ? children.Where (c => c.DesiredSize.Height == 0).Max (c => c.Margin.Top + c.Margin.Bottom) : 0;
			} else {
				width = collection.Where (c => c.DesiredSize.Width > 0).Max (c => c.DesiredSize.Width + c.Margin.Left + c.Margin.Right);
				width += collection.Any (c => c.DesiredSize.Width == 0) ? children.Where (c => c.DesiredSize.Width == 0).Max (c => c.Margin.Left + c.Margin.Right) : 0;

				height = collection.Sum (c => c.DesiredSize.Height + c.Margin.Top + c.Margin.Bottom);			
			}
		
			return new Size (width, height);
		}
		
		protected override void ArrangeOverride (Size finalSize)
		{
			var x = 0d;
			var y = 0d;
			
			foreach (var child in children.Where(c => c.Visibility != Visibility.Collapsed)) {	
				var width = child.DesiredSize.Width;
				var height = child.DesiredSize.Height;
				
				if (Orientation == Orientation.Vertical && child.HorizontalAlignment == HorizontalAlignment.Stretch) {
					width = finalSize.Width - child.Margin.Left - child.Margin.Right;
				}

				if (Orientation == Orientation.Horizontal && child.VerticalAlignment == VerticalAlignment.Stretch) {
					height = finalSize.Height - child.Margin.Top - child.Margin.Bottom;
				}

				if (Orientation == Orientation.Horizontal) {
					x += child.Margin.Left;	

					switch (child.VerticalAlignment) {
					case VerticalAlignment.Top:
					case VerticalAlignment.Stretch:
						y = child.Margin.Top;
						break;
					case VerticalAlignment.Bottom:
						y = Height - child.Margin.Bottom - height;
						break;
					case VerticalAlignment.Center:
					default:
						y = (Height - height) / 2;
						break;
					}

				} else {
					y += child.Margin.Top;

					switch (child.HorizontalAlignment) {
					case HorizontalAlignment.Left:
					case HorizontalAlignment.Stretch:
						x = child.Margin.Left;
						break;
					case HorizontalAlignment.Right:
						x = Width - child.Margin.Right - width;
						break;
					case HorizontalAlignment.Center:
						x = (Width - width) / 2;
						break;
					}
				}

				child.Arrange (new Rect (new Point (x, y), new Size (width, height)));
				
				if (Orientation == Orientation.Horizontal)				
					x += child.Width + child.Margin.Right;
				else
					y += child.Height + child.Margin.Bottom;
			}
		}
		
		public override Visual GetVisualChild (int index)
		{
			return children [index];
		}
	}
}