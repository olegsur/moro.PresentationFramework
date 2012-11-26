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

		private ObservableCollection<ItemView> items = new ObservableCollection<ItemView> ();
		public ObservableCollection<ItemView> Items { get { return items; } }

		public ItemsControl ()
		{
			itemsPanel = BuildProperty<Panel> ("ItemsPanel");
			itemsPanel.DependencyPropertyValueChanged += HandleItemsPanelChanged;			
			ItemsPanel = new StackPanel ();

			itemsSource = BuildProperty<IEnumerable> ("ItemsSource");
			itemsSource.DependencyPropertyValueChanged += ItemsSourceChanged;

			ItemTemplate = new DataTemplate (o => o is UIElement ? o as UIElement : new TextBlock () {Text = o.ToString ()});

			items.CollectionChanged += HandleItemsChanged;
			
			StyleHelper.ApplyStyle (this, typeof(ItemsControl));
		}

		private void HandleItemsPanelChanged (object sender, DPropertyValueChangedEventArgs<Panel> e)
		{
			if (e.OldValue != null) {
				e.OldValue.Children.Clear ();

				RemoveVisualChild (e.OldValue);
			}
				
			if (e.NewValue != null) {
				AddVisualChild (e.NewValue);

				foreach (var item in items) {
					e.NewValue.Children.Add (item.Visual);
				}
			}
		}						

		private void ItemsSourceChanged (object sender, DPropertyValueChangedEventArgs<IEnumerable> e)
		{	
			if (e.OldValue is INotifyCollectionChanged)
				(e.NewValue as INotifyCollectionChanged).CollectionChanged -= HandleItemSourceCollectionChanged;
			
			foreach (var o in e.NewValue) {
				var child = ItemTemplate.LoadContent (o);

				items.Add (new ItemView () { Item = o, Visual = child });
			}

			if (e.NewValue is INotifyCollectionChanged) 
				(e.NewValue as INotifyCollectionChanged).CollectionChanged += HandleItemSourceCollectionChanged;
		}

		private void HandleItemSourceCollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action) {
			case NotifyCollectionChangedAction.Add:
				var index = e.NewStartingIndex;

				foreach (var o in e.NewItems) {
					var child = ItemTemplate.LoadContent (o);

					items.Insert (index, new ItemView () { Item = o, Visual = child });
					index++;
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				foreach (var o in e.OldItems) {
					var item = items.First (i => i.Item == o);

					items.Remove (item);
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				var index2 = e.NewStartingIndex;
				foreach (var o in e.NewItems) {
					var child = ItemTemplate.LoadContent (o);

					items [index2] = new ItemView () { Item = o, Visual = child };
					index2++;
				}
				break;
			case NotifyCollectionChangedAction.Reset:
				items.Clear ();
				break;			
			}
		}

		private void HandleItemsChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			if (ItemsPanel == null)
				return;

			switch (e.Action) {
			case NotifyCollectionChangedAction.Add:
				var index = e.NewStartingIndex;
				
				foreach (var itemView in e.NewItems.Cast<ItemView>()) {
					ItemsPanel.Children.Insert (index, itemView.Visual);
					index++;
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				foreach (var o in e.OldItems) {
					var item = items.First (i => i.Item == o);
					
					ItemsPanel.Children.Remove (item.Visual);
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				var index2 = e.NewStartingIndex;
				foreach (var o in e.NewItems.Cast<ItemView>()) {										
					ItemsPanel.Children [index2] = o.Visual;
				}
				break;
			case NotifyCollectionChangedAction.Reset:
				ItemsPanel.Children.Clear ();
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
	}

	public class ItemView
	{
		public object Item { get; set; }
		public UIElement Visual { get; set; }
	}
}

