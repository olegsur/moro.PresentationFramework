//
// AttachedPropertyExpression.cs
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
using moro.Framework.Data;

namespace moro.Framework.Data
{
	public class AttachedPropertyExpression : BindingExpression
	{
		private string PropertyName { get; set; }
		private object Item { get; set; }
		private IDependencyProperty Source { get; set; }

		public AttachedPropertyExpression (BindingExpression expression, object item, string propertyName)
		{
			PropertyName = propertyName;
			Item = item;

			Source = expression.Property;

			if (Source != null) {
				var container = Source.Value as IAttachedPropertiesContainer;

				if (container != null) {
					Property = container.GetProperty (item, propertyName);

					container.AddedItem += HandleAddedItem;
					container.RemovedItem += HandleRemovedItem;
				}
				Source.DependencyPropertyValueChanged += HandlePropertyValueChanged;
			}

			expression.GetProperty ("Property").DependencyPropertyValueChanged += HandlePropertyChanged;
		}

		private void HandlePropertyChanged (object sender, DPropertyValueChangedEventArgs e)
		{		
			if (Source != null) {
				Source.DependencyPropertyValueChanged -= HandlePropertyValueChanged;

				var container = Source.Value as IAttachedPropertiesContainer;

				if (container != null) {
					container.AddedItem -= HandleAddedItem;
					container.RemovedItem -= HandleRemovedItem;
				}
			}

			Source = e.NewValue as IDependencyProperty;

			if (Source != null) {
				var container = Source.Value as IAttachedPropertiesContainer;

				if (container != null) {
					Property = container.GetProperty (Item, PropertyName);

					container.AddedItem += HandleAddedItem;
					container.RemovedItem += HandleRemovedItem;
				}
				Source.DependencyPropertyValueChanged += HandlePropertyValueChanged;
			} else {
				Property = null;
			}
		}

		private void HandlePropertyValueChanged (object sender, DPropertyValueChangedEventArgs e)
		{
			if (e.OldValue is IAttachedPropertiesContainer) {
				var oldContainer = e.OldValue as IAttachedPropertiesContainer;

				if (oldContainer != null) {	
					oldContainer.AddedItem += HandleAddedItem;
					oldContainer.RemovedItem += HandleRemovedItem;
				}
			}

			var container = e.NewValue as IAttachedPropertiesContainer;

			if (container != null) {
				Property = container.GetProperty (Item, PropertyName);

				container.AddedItem += HandleAddedItem;
				container.RemovedItem += HandleRemovedItem;
			} else {
				Property = null;
			}
		}

		private void HandleAddedItem (object sender, ItemEventArgs e)
		{
			if (e.Item != Item)
				return;

			var container = Source.Value as IAttachedPropertiesContainer;

			Property = container.GetProperty (Item, PropertyName);
		}

		private void HandleRemovedItem (object sender, ItemEventArgs e)
		{
			if (e.Item != Item)
				return;

			Property = null;
		}
	}
}

