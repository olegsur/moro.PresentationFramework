//
// ObservableCollection.cs
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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace moro.Framework
{
	public class ObservableCollection<T> : Collection<T>, INotifyCollectionChanged
	{
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public ObservableCollection () : base()
		{
		}

		public ObservableCollection (IList<T> list) : base(list)
		{
		}

		protected override void InsertItem (int index, T item)
		{
			base.InsertItem (index, item);

			RaiseCollectionChanged (NotifyCollectionChangedAction.Add, item, index);
		}

		protected override void RemoveItem (int index)
		{
			T item = base [index];

			base.RemoveItem (index);

			RaiseCollectionChanged (NotifyCollectionChangedAction.Remove, item, index);
		}

		protected override void SetItem (int index, T item)
		{
			T oldItem = base [index];

			base.SetItem (index, item);

			RaiseCollectionChanged (NotifyCollectionChangedAction.Replace, oldItem, item, index);
		}

		protected override void ClearItems ()
		{
			base.ClearItems ();

			RaiseCollectionChanged (NotifyCollectionChangedAction.Reset);
		}

		private void RaiseCollectionChanged (NotifyCollectionChangedAction action)
		{
			if (CollectionChanged != null)
				CollectionChanged (this, new NotifyCollectionChangedEventArgs (action));
		}

		private void RaiseCollectionChanged (NotifyCollectionChangedAction action, T item, int index)
		{
			if (CollectionChanged != null)
				CollectionChanged (this, new NotifyCollectionChangedEventArgs (action, item, index));
		}

		private void RaiseCollectionChanged (NotifyCollectionChangedAction action, T oldItem, T newItem, int index)
		{
			if (CollectionChanged != null)
				CollectionChanged (this, new NotifyCollectionChangedEventArgs (action, newItem, oldItem, index));
		}
	}
}

