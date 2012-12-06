//
// PropertyExpression.cs
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

namespace moro.Framework.Data
{
	public class PropertyExpression : PathExpression
	{
		private string PropertyName { get; set; }
		private IDependencyProperty Source { get; set; }

		public PropertyExpression (IDependencyProperty property)
		{
			Property = property;
		}

		public PropertyExpression (PathExpression expression, string propertyName)
		{
			PropertyName = propertyName;

			Source = expression.Property;

			if (Source != null) {
				if (Source.Value != null)
					Property = (Source.Value as DependencyObject).GetProperty (propertyName);
				Source.DependencyPropertyValueChanged += HandlePropertyValueChanged;
			}

			expression.GetProperty ("Property").DependencyPropertyValueChanged += HandlePropertyChanged;
		}

		private void HandlePropertyChanged (object sender, DPropertyValueChangedEventArgs e)
		{		
			if (Source != null)
				Source.DependencyPropertyValueChanged -= HandlePropertyValueChanged;

			Source = e.NewValue as IDependencyProperty;

			if (Source != null) {
				if (Source.Value != null)
					Property = (Source.Value as DependencyObject).GetProperty (PropertyName);
				Source.DependencyPropertyValueChanged += HandlePropertyValueChanged;
			}
		}

		private void HandlePropertyValueChanged (object sender, DPropertyValueChangedEventArgs e)
		{
			Property = (e.NewValue as DependencyObject).GetProperty (PropertyName);
		}
	}
}

