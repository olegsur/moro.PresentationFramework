//
// FractionToken.cs
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
using fkalc.UI.Common.MathRegion.Tokens;

namespace fkalc.UI.ViewModels.MathRegion.Tokens
{
	public class FractionToken : ContainerToken, IFractionToken
	{
		private readonly DependencyProperty<Token> dividend;
		private readonly DependencyProperty<Token> divisor;

		public Token Dividend { 
			get { return dividend.Value; } 
			set{ dividend.Value = value; }
		}

		public Token Divisor { 
			get { return divisor.Value; } 
			set{ divisor.Value = value; }
		}

		public FractionToken ()
		{
			dividend = BuildProperty<Token> ("Dividend");
			dividend.DependencyPropertyValueChanged += HandleValueChanged;

			divisor = BuildProperty<Token> ("Divisor");
			divisor.DependencyPropertyValueChanged += HandleValueChanged;

			Dividend = new TextToken ();
			Divisor = new TextToken ();					
		}

		private void HandleValueChanged (object sender, DPropertyValueChangedEventArgs<Token> e)
		{
			if (e.OldValue != null)
				e.OldValue.Parent = null;

			if (e.NewValue != null)
				e.NewValue.Parent = this;
		}
				
		public override void Replace (Token oldToken, Token newToken)
		{
			if (oldToken == Dividend)
				Dividend = newToken;

			if (oldToken == Divisor)
				Divisor = newToken;
		}

		IToken IFractionToken.Dividend {
			get {
				return Dividend;
			}
		}

		IToken IFractionToken.Divisor {
			get {
				return Divisor;
			}
		}
	}
}

