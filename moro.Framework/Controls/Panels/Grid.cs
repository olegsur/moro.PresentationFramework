//
// Grid.cs
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
using System.Collections.Specialized;
using System.Linq;

namespace moro.Framework
{
	public class Grid : Panel
	{
		private List<GridChild> children = new List<GridChild> ();
		public List<RowDefinition> RowDefinitions { get; private set; }
		public List<ColumnDefinition> ColumnDefinitions { get; private set; }

		public Grid ()
		{
			RowDefinitions = new List<RowDefinition> ();
			ColumnDefinitions = new List<ColumnDefinition> ();
			Children.CollectionChanged += HandleCollectionChanged;			
		}

		public void SetRow (uint row, UIElement element)
		{
			var child = children.FirstOrDefault (c => c.Content == element);
			
			if (child == null)
				return;
			
			child.Row = row;
		}

		public void SetColumn (uint column, UIElement element)
		{
			var child = children.FirstOrDefault (c => c.Content == element);
			
			if (child == null)
				return;
			
			child.Column = column;
		}

		private void HandleCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add) {
				var index = e.NewStartingIndex;

				foreach (var uielement in e.NewItems.Cast<UIElement>()) {
					var child = new GridChild (uielement);
			
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
					var child = new GridChild (uielement);
			
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
			var col = children.Where (c => c.Visibility != Visibility.Collapsed);

			foreach (var child in col) {
				child.Measure (availableSize);
			}

			var height = col.Any () ? col.GroupBy (c => c.Row).Sum (row => row.Max (c => c.DesiredSize.Height + c.Margin.Top + c.Margin.Bottom)) : 0;
			var width = col.Any () ? col.GroupBy (c => c.Column).Sum (column => column.Max (c => c.DesiredSize.Width + c.Margin.Left + c.Margin.Right)) : 0;
			
			return new Size (width, height);
		}

		protected override void ArrangeOverride (Size finalSize)
		{
			var x = 0d;
			var y = 0d;

			var col = children.Where (c => c.Visibility != Visibility.Collapsed);

			var heights = col.GroupBy (c => c.Row).OrderBy (g => g.Key)
				.ToDictionary (row => row.Key, row => row.Max (c => c.DesiredSize.Height + c.Margin.Top + c.Margin.Bottom));

			var widths = col.GroupBy (c => c.Column).OrderBy (g => g.Key)
				.ToDictionary (
				column => column.Key,
				column => column.Max (c => c.DesiredSize.Width + c.Margin.Left + c.Margin.Right)
			); 
			
			var rows = heights.Keys.Where (k => k < RowDefinitions.Count).ToDictionary (k => k, k => RowDefinitions [(int)k]);
			
			if (rows.Values.Any (r => r.Height.GridUnitType == GridUnitType.Star)) {
				var fixedHeight = 0d;
				
				foreach (var height in heights) {
					if (!rows.ContainsKey (height.Key) || rows [height.Key].Height.GridUnitType != GridUnitType.Star)
						fixedHeight += height.Value;
				}
				
				var star = (finalSize.Height - fixedHeight) / rows.Values.Where (r => r.Height.GridUnitType == GridUnitType.Star).Sum (r => r.Height.Value);
				
				//TODO: solve the case when heights[row.Key] > row.Value.Height.Value * star!
				foreach (var row in rows.Where(r => r.Value.Height.GridUnitType == GridUnitType.Star)) {
					heights [row.Key] = row.Value.Height.Value * star;
				}
			}
			
			var columns = widths.Keys.Where (k => k < ColumnDefinitions.Count).ToDictionary (
				k => k,
				k => ColumnDefinitions [(int)k]
			);
			
			if (columns.Values.Any (r => r.Width.GridUnitType == GridUnitType.Star)) {
				var fixedWidth = 0d;
				
				foreach (var widht in widths) {
					if (!columns.ContainsKey (widht.Key) || columns [widht.Key].Width.GridUnitType != GridUnitType.Star)
						fixedWidth += widht.Value;
				}
				
				var star = (finalSize.Width - fixedWidth) / columns.Values.Where (r => r.Width.GridUnitType == GridUnitType.Star).Sum (r => r.Width.Value);
				
				//TODO: solve the case when widths[column.Key > column.Value.Width.Value * starr!
				foreach (var column in columns.Where(r => r.Value.Width.GridUnitType == GridUnitType.Star)) {
					widths[column.Key] = column.Value.Width.Value * star;
				}
			}

			foreach (var height in heights) {				
				x = 0;
				foreach (var width in widths) {
					foreach (var child in col.Where(c => c.Row == height.Key && c.Column == width.Key)) {
						var w = child.DesiredSize.Width;
						var h = child.DesiredSize.Height;

						if (child.HorizontalAlignment == HorizontalAlignment.Stretch) {
							w = width.Value - child.Margin.Left - child.Margin.Right;
						}

						if (child.VerticalAlignment == VerticalAlignment.Stretch) {
							h = height.Value - child.Margin.Top - child.Margin.Bottom;
						}

						child.Arrange (new Rect (new Point (x + child.Margin.Left, y + child.Margin.Top), new Size (w, h)));
					}
					x += width.Value;
				}
				y += height.Value;
			}
		}

		public override Visual GetVisualChild (int index)
		{
			return children [index];
		}
	}
}

