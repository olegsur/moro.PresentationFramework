//
// RoutedEvent.cs
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
using System.Collections.Generic;

namespace moro.Framework
{
	public abstract class RoutedEvent<TEventArgs> where TEventArgs: EventArgs
	{
		public event EventHandler<TEventArgs> Event;

		public void RaiseEvent (Visual visual, TEventArgs eventArgs)
		{
			if (Event == null)
				return;

			var handlers = Event.GetInvocationList ();

			if (!handlers.Any ())
				return;

			foreach (var target in GetRoute(visual)) {
				var handler = handlers.FirstOrDefault (h => h.Target == target);

				if (handler != null)
					handler.DynamicInvoke (this, eventArgs);
			}
		}

		protected abstract IEnumerable<Visual> GetRoute (Visual visual);
	}

	public class BubblingEvent<TEventArgs> : RoutedEvent<TEventArgs> where TEventArgs: EventArgs
	{	
		protected override IEnumerable<Visual> GetRoute (Visual visual)
		{
			return VisualTreeHelper.GetVisualBranch (visual);
		}	
	}

	public class TunnelingEvent<TEventArgs> : RoutedEvent<TEventArgs> where TEventArgs: EventArgs
	{	
		protected override IEnumerable<Visual> GetRoute (Visual visual)
		{
			return VisualTreeHelper.GetVisualBranch (visual).Reverse ();
		}	
	}

	public class DirectEvent<TEventArgs> : RoutedEvent<TEventArgs> where TEventArgs: EventArgs
	{	
		protected override IEnumerable<Visual> GetRoute (Visual visual)
		{
			yield return visual;
		}	
	}
}

