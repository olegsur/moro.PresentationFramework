//
// HBoxToken.cs
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
using moro.Framework;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using fkalc.UI.Common.MathRegion.Tokens;

namespace fkalc.UI.ViewModels.MathRegion.Tokens
{
	public class HBoxToken : ContainerToken, IHBoxToken
	{
		private readonly DependencyProperty<ObservableCollection<Token>> tokens;

		public ObservableCollection<Token> Tokens { get { return tokens.Value; } }

		public HBoxToken ()
		{
			tokens = BuildProperty<ObservableCollection<Token>> ("Tokens");
			tokens.Value = new ObservableCollection<Token> ();

			Tokens.CollectionChanged += HandleCollectionChanged;
		}


		public void Add (Token token)
		{
			Tokens.Add (token);
		}

		public override void Replace (Token oldToken, Token newToken)
		{
			var index = Tokens.IndexOf (oldToken);
			if (index == -1)
				return;

			Tokens.RemoveAt (index);
			Tokens.Insert (index, newToken);
		}

		private void HandleCollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action) {
			case NotifyCollectionChangedAction.Add:
				foreach (var token in e.NewItems.Cast<Token>()) {
					token.Parent = this;
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				foreach (var token in e.OldItems.Cast<Token>()) {
					token.Parent = null;
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				foreach (var token in e.OldItems.Cast<Token>()) {
					token.Parent = null;
				}
				foreach (var token in e.NewItems.Cast<Token>()) {
					token.Parent = this;
				}
				break;
			case NotifyCollectionChangedAction.Reset:
				foreach (var token in e.OldItems.Cast<Token>()) {
					token.Parent = this;
				}
				break;
			default:
				break;
			}

		}
	
		IEnumerable<IToken> IHBoxToken.Tokens {
			get {
				return Tokens.Cast<IToken> ().ToArray ();
			}
		}

	}
}

