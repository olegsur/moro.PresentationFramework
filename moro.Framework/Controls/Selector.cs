//
// Selector.cs
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
using System.Collections.Specialized;
using System.Linq;
using moro.Framework.Data;

namespace moro.Framework
{
	public abstract class Selector : ItemsControl
	{
		private DependencyProperty<UIElement> selectedItem;

		public UIElement SelectedItem { 
			get { return selectedItem.Value; }
			set { selectedItem.Value = value; }
		}

		public Selector ()
		{
			selectedItem = BuildProperty<UIElement> ("SelectedItem");
			selectedItem.DependencyPropertyValueChanged += HandleSelectedItemChanged;

			Items.CollectionChanged += HandleItemsChanged;
		}

		private void HandleSelectedItemChanged (object sender, DPropertyValueChangedEventArgs<UIElement> e)
		{
			if (e.OldValue != null) {
				var isSelected = e.OldValue.GetProperty ("IsSelected");
				if (isSelected != null)
					isSelected.Value = false;
			}

			if (e.NewValue != null) {
				var isSelected = e.NewValue.GetProperty ("IsSelected");
				if (isSelected != null)
					isSelected.Value = true;
			}
		}

		private void HandleItemsChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action) {
			case NotifyCollectionChangedAction.Add:
				if (SelectedItem == null)
					SelectedItem = e.NewItems.Cast<ItemView> ().First ().Visual;
				break;
			default:
				break;
			}
		}
	}
}

