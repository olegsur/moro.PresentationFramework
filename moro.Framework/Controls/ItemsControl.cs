//
// ItemsControl.cs
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
using System.Collections;
using System.Linq;
using System.Collections.Specialized;
using System.Collections.Generic;
using moro.Framework.Data;

namespace moro.Framework
{
	public class ItemsControl : Control
	{
		private readonly DependencyProperty<Panel> itemsPanel;
		private readonly DependencyProperty<IEnumerable> itemsSource;
		
		public Panel ItemsPanel { 
			get { return itemsPanel.Value; }
			set { itemsPanel.Value = value; }
		}
		
		public IEnumerable ItemsSource { 
			get { return itemsSource.Value;} 
			set { itemsSource.Value = value; }
		}		

		public DataTemplate ItemTemplate { get; set; }

		private List<ItemVisual> items = new List<ItemVisual> ();

		public ItemsControl ()
		{
			itemsPanel = BuildProperty<Panel> ("ItemsPanel");
			itemsPanel.DependencyPropertyValueChanged += HandleItemsPanelChanged;
			
			ItemsPanel = new StackPanel ();
			itemsSource = BuildProperty<IEnumerable> ("ItemsSource");

			itemsSource.DependencyPropertyValueChanged += ItemsSourceChanged;

			ItemTemplate = new DataTemplate (o => o is UIElement ? o as UIElement : new TextBlock () {Text = o.ToString ()});
			
			StyleHelper.ApplyStyle (this, typeof(ItemsControl));
		}

		private void HandleItemsPanelChanged (object sender, DPropertyValueChangedEventArgs<Panel> e)
		{
			if (e.OldValue != null)
				RemoveVisualChild (e.OldValue);
				
			if (e.NewValue != null)
				AddVisualChild (e.NewValue);
		}						

		private void ItemsSourceChanged (object sender, DPropertyValueChangedEventArgs<IEnumerable> e)
		{
			if (ItemsPanel == null)
				return;
			
			ItemsPanel.Children.Clear ();

			foreach (var o in e.NewValue) {
				var child = ItemTemplate.LoadContent (o);
				ItemsPanel.Children.Add (child);

				items.Add (new ItemVisual () { Item = o, Visual = child });
			}

			if (e.NewValue is INotifyCollectionChanged) {
				var collection = e.NewValue as INotifyCollectionChanged;

				collection.CollectionChanged += HandleCollectionChanged;
			}
		}

		private void HandleCollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			if (ItemsPanel == null)
				return;
			
			switch (e.Action) {
			case NotifyCollectionChangedAction.Add:
				var index = e.NewStartingIndex;

				foreach (var o in e.NewItems) {
					var child = ItemTemplate.LoadContent (o);
					ItemsPanel.Children.Insert (index, child);

					items.Insert (index, new ItemVisual () { Item = o, Visual = child });
					index++;
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				foreach (var o in e.OldItems) {
					var item = items.First (i => i.Item == o);

					ItemsPanel.Children.Remove (item.Visual);
					items.Remove (item);
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				var i = e.NewStartingIndex;
				foreach (var o in e.NewItems) {
					var child = ItemTemplate.LoadContent (o);

					ItemsPanel.Children[i] = child;

					items[i] = new ItemVisual () { Item = o, Visual = child };
					i++;
				}
				break;
			case NotifyCollectionChangedAction.Reset:
				ItemsPanel.Children.Clear ();
				break;
			default:
				break;
			}

		}
				
		protected override int GetVisualChildrenCountCore ()
		{
			return 1;
		}

		protected override Visual GetVisualChildCore (int index)
		{
			return ItemsPanel;
		}

		private class ItemVisual
		{
			public object Item { get; set; }
			public UIElement Visual { get; set; }
		}

	}
}

