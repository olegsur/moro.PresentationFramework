// 
// Canvas.cs
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
using System.Collections.Generic;
using Cairo;
using System.Linq;
using System.Collections.Specialized;
using moro.Framework.Data;

namespace moro.Framework
{
	public class Canvas : Panel, IAttachedPropertiesContainer
	{		
		private List<CanvasChild> children = new List<CanvasChild> ();
		
		public Canvas ()
		{		
			HorizontalAlignment = HorizontalAlignment.Stretch;
			VerticalAlignment = VerticalAlignment.Stretch;
			
			Children.CollectionChanged += HandleCollectionChanged;
		}
			
		public double GetLeft (UIElement element)
		{
			var child = children.FirstOrDefault (c => c.Content == element);
			
			if (child == null)
				return 0;
			
			return child.X;
		}
		
		public void SetLeft (double left, UIElement element)
		{
			var child = children.FirstOrDefault (c => c.Content == element);
			
			if (child == null)
				return;
			
			child.X = left;
		}
		
		public double GetTop (UIElement element)
		{
			var child = children.FirstOrDefault (c => c.Content == element);
			
			if (child == null)
				return 0;
			
			return child.Y;
		}
		
		public void SetTop (double top, UIElement element)
		{
			var child = children.FirstOrDefault (c => c.Content == element);
			
			if (child == null)
				return;
			
			child.Y = top;
		}

		private void HandleCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add) {
				var index = e.NewStartingIndex;

				foreach (var uielement in e.NewItems.Cast<UIElement>()) {
					var child = new CanvasChild (uielement);
			
					AddVisualChild (child);
					children.Insert (index, child);

					RaiseAddedItem (uielement);

					index++;
				}
			}

			if (e.Action == NotifyCollectionChangedAction.Remove) {
				var index = e.OldStartingIndex;
				foreach (var uielement in e.OldItems.Cast<UIElement>()) {
					var child = children [index];
			
					RemoveVisualChild (child);			
					children.Remove (child);

					RaiseRemovedItem (uielement);

					index++;
				}
			}

			if (e.Action == NotifyCollectionChangedAction.Replace) {
				var index = e.NewStartingIndex;

				foreach (var uielement in e.NewItems.Cast<UIElement>()) {
					var child = new CanvasChild (uielement);
			
					RemoveVisualChild (children [index]);
					RaiseRemovedItem (children [index].Content);

					AddVisualChild (child);
					children [index] = child;

					RaiseAddedItem (uielement);

					index++;
				}
			}

			if (e.Action == NotifyCollectionChangedAction.Reset) {
				foreach (var child in children) {
					RaiseRemovedItem (child.Content);
				}

				children.Clear ();
			}
		}
				
		protected override Size MeasureOverride (Size availableSize)
		{
			foreach (var child in children.Where(c => c.Visibility != Visibility.Collapsed)) {
				child.Measure (availableSize);
			}			
			
			return new Size (0, 0);
		}
		
		protected override void ArrangeOverride (Size finalSize)
		{
			foreach (var child in children.Where(c => c.Visibility != Visibility.Collapsed)) {				
				child.Arrange (new Rect (new Point (child.X, child.Y), child.DesiredSize));
			}
		}

		public override Visual GetVisualChild (int index)
		{
			return children [index];
		}

		public event EventHandler<ItemEventArgs> AddedItem;
		public event EventHandler<ItemEventArgs> RemovedItem;

		public IDependencyProperty GetProperty (object item, string propertyName)
		{
			var child = children.FirstOrDefault (c => c.Content == item);
						
			return child != null ? child.GetProperty (propertyName) : null;
		}

		private void RaiseAddedItem (object item)
		{
			if (AddedItem != null)
				AddedItem (this, new ItemEventArgs (item));
		}

		private void RaiseRemovedItem (object item)
		{
			if (RemovedItem != null)
				RemovedItem (this, new ItemEventArgs (item));
		}
	}
}