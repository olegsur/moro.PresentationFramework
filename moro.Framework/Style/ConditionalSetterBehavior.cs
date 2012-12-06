//
// ConditionalSetterBehavior.cs
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

namespace moro.Framework
{
	public class ConditionalSetterBehavior : ISetterBehavior
	{
		private SetterBehavior Behavior { get; set; }
		private SetterCondition Condition { get; set; }
		private bool isSetterApplied;

		public ConditionalSetterBehavior (SetterCondition condition, SetterBehavior behavior)
		{
			Behavior = behavior;
			Condition = condition;
			Condition.GetProperty ("IsMatch").DependencyPropertyValueChanged += HandleIsMatchChanged;
		}

		public void Apply ()
		{
			if (Condition.IsMatch) {
				Behavior.Apply ();
				isSetterApplied = true;
			}
		}

		public void Remove ()
		{
			if (isSetterApplied) {
				Behavior.Remove ();
				isSetterApplied = false;
			}
		}

		private void HandleIsMatchChanged (object sender, moro.Framework.Data.DPropertyValueChangedEventArgs e)
		{
			if (Condition.IsMatch)
				Apply ();
			else
				Remove ();
		}

	}
}

