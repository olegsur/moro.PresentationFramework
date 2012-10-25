//
// TokenAreaConverter.cs
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
using fkalc.UI.ViewModels.MathRegion.Tokens;

namespace fkalc.UI
{
	public class TokenAreaConverter : IValueConverter
	{
		public TokenAreaConverter ()
		{
		}

		public object Convert (object value)
		{
			Area result = null;

			if (value is TextToken) 
				result = new TextArea ();				

			if (value is PlusToken)
				result = new PlusArea ();

			if (value is MinusToken)
				result = new MinusArea ();

			if (value is MultiplicationToken)
				result = new MultiplicationArea ();

			if (value is FractionToken)
				result = new FractionArea ();

			if (value is HBoxToken)
				result = new HBoxArea ();

			if (value is ResultToken)
				result = new ResultArea ();

			if (value is AssignmentToken)
				result = new AssignmentArea ();

			if (value is ParenthesesToken)
				result = new ParenthesesArea ();

			if (value is CommaToken)
				result = new CommaArea ();

			if (value is ExponentiationToken)
				result = new ExponentiationArea ();

			if (value is SquareRootToken)
				result = new SquareRootArea ();

			if (value is AbsoluteToken)
				result = new AbsoluteArea ();

			result.DataContext = value;

			return result;
		}

		public object ConvertBack (object value)
		{
			throw new System.NotImplementedException ();
		}
	}
}

