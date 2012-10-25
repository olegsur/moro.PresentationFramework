//
// ExponentiationToken.cs
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
using fkalc.UI.Common.MathRegion.Tokens;

namespace fkalc.UI.ViewModels.MathRegion.Tokens
{
	public class ExponentiationToken: ContainerToken, IExponentiationToken
	{
		private readonly DependencyProperty<Token> _base;
		private readonly DependencyProperty<Token> power;

		public Token Base { 
			get { return _base.Value; } 
			set{ _base.Value = value; }
		}

		public Token Power { 
			get { return power.Value; } 
			set{ power.Value = value; }
		}

		public ExponentiationToken ()
		{
			_base = BuildProperty<Token> ("Base");
			_base.DependencyPropertyValueChanged += HandleValueChanged;

			power = BuildProperty<Token> ("Power");
			power.DependencyPropertyValueChanged += HandleValueChanged;

			Base = new TextToken ();
			Power = new TextToken ();					
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
			if (oldToken == Base)
				Base = newToken;

			if (oldToken == Power)
				Power = newToken;
		}

		IToken IExponentiationToken.Base {
			get {
				return Base;
			}
		}

		IToken IExponentiationToken.Power {
			get {
				return Power;
			}
		}
	}
}

