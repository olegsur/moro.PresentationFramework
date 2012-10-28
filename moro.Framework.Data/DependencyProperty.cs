//
// DependencyProperty.cs
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
	public class DependencyProperty<T> : IDependencyProperty
	{
		EventHandler<DPropertyValueChangedEventArgs> dependencyPropertyValueChangedHandler;

		event EventHandler<DPropertyValueChangedEventArgs> IDependencyProperty.DependencyPropertyValueChanged {
			add{ dependencyPropertyValueChangedHandler += value; }
			remove{ dependencyPropertyValueChangedHandler -= value; }
		}

		public event EventHandler<DPropertyValueChangedEventArgs<T>> DependencyPropertyValueChanged;

		private T value;

		public DependencyProperty ()
		{
		}

		public T Value {
			get {
				return value;
			}
			set {
				if (this.value == null && value == null)
					return;
				if (this.value != null && this.value.Equals (value))
					return;

				var oldValue = this.value;
				this.value = value;
				RaiseDependencyPropertyValueChanged (oldValue, value);
			}
		}

		private void RaiseDependencyPropertyValueChanged (T oldValue, T newValue)
		{
			if (DependencyPropertyValueChanged != null) {
				DependencyPropertyValueChanged (this, new DPropertyValueChangedEventArgs<T> (oldValue, newValue));
			}
			if (dependencyPropertyValueChangedHandler != null) {
				dependencyPropertyValueChangedHandler (this, new DPropertyValueChangedEventArgs (oldValue, newValue));
			}
		}

		object IDependencyProperty.Value {
			get { return Value;	}
			set {
				if (value is T)
					Value = (T)value;
			}
		}
	}
}

