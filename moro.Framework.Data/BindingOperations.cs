//
// BindingOperations.cs
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
using moro.Framework.Data;

namespace moro.Framework.Data
{
	public static class BindingOperations
	{
		public static void SetBinding (IDependencyProperty source, IDependencyProperty target, IValueConverter converter = null)
		{
			new Binding (new PropertyExpression (source), new PropertyExpression (target), converter ?? new EmptyConverter ());
		}

		public static void SetBinding (DependencyObject source, string path, IDependencyProperty target, IValueConverter converter = null)
		{
			var paths = path.Split ('.');

			PathExpression expression = new SelfExpression (source);

			foreach (var p in paths) {
				expression = new PropertyExpression (expression, p);
			}

			new Binding (expression, new PropertyExpression (target), converter ?? new EmptyConverter ());
		}

		public static void SetBinding (IDependencyProperty source, IAttachedPropertiesContainer container, object item, string propertyName, IValueConverter converter = null)
		{
			var self = new SelfExpression (container);

			new Binding (new PropertyExpression (source), new AttachedPropertyExpression (self, item, propertyName), converter ?? new EmptyConverter ());
		}
	}
}

