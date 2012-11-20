// 
// WidgetKeyboardInputProvider.cs
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
using Gtk;

namespace moro.Framework
{
	public class WidgetKeyboardInputProvider : IKeyboardInputProvider
	{
		public event EventHandler<KeyEventArgs> KeyPressEvent;
		public event EventHandler<KeyEventArgs> KeyReleaseEvent;
		
		public WidgetKeyboardInputProvider (Widget widget)
		{
			widget.KeyPressEvent += HandleKeyPressEvent;
			widget.KeyReleaseEvent += HandleKeyReleaseEvent;
		}

		[GLib.ConnectBefore] 
		private void HandleKeyPressEvent (object o, KeyPressEventArgs args)
		{
			var key = Convert (args.Event.HardwareKeycode);
			RaiseKeyPressEvent (new KeyEventArgs (key));
		}

		[GLib.ConnectBefore] 
		private void HandleKeyReleaseEvent (object o, KeyReleaseEventArgs args)
		{
			var key = Convert (args.Event.HardwareKeycode);
			RaiseKeyReleaseEvent (new KeyEventArgs (key));
		}

		private void RaiseKeyPressEvent (KeyEventArgs args)
		{
			if (KeyPressEvent != null) {
				KeyPressEvent (this, args);
			}
		}

		private void RaiseKeyReleaseEvent (KeyEventArgs args)
		{
			if (KeyReleaseEvent != null) {
				KeyReleaseEvent (this, args);
			}
		}

		private Key Convert (ushort keyCode)
		{
			switch (keyCode) {
			case 10:
				return Key.D1;
			case 11:
				return Key.D2;
			case 12:
				return Key.D3;
			case 13:
				return Key.D4;
			case 14:
				return Key.D5;
			case 15:
				return Key.D6;
			case 16:
				return Key.D7;
			case 17:
				return Key.D8;
			case 18:
				return Key.D9;
			case 19:
				return Key.D0;
			case 20:
				return Key.OemMinus;
			case 21:
				return Key.OemPlus;
			case 22:
				return Key.Back;
			case 23:
				return Key.Tab;
			case 24:
				return Key.Q;
			case 25:
				return Key.W;
			case 26:
				return Key.E;
			case 27:
				return Key.R;
			case 28:
				return Key.T;
			case 29:
				return Key.Y;
			case 30:
				return Key.U;
			case 31:
				return Key.I;
			case 32:
				return Key.O;
			case 33:
				return Key.P;
			case 34:
				return Key.OemOpenBrackets;
			case 35:
				return Key.OemCloseBrackets;
			case 36:
				return Key.Enter;
			case 37:
				return Key.LeftCtrl;
			case 38:
				return Key.A;
			case 39:
				return Key.S;
			case 40:
				return Key.D;
			case 41:
				return Key.F;
			case 42:
				return Key.G;
			case 43:
				return Key.H;
			case 44:
				return Key.J;
			case 45:
				return Key.K;
			case 46:
				return Key.L;
			case 47:
				return Key.OemSemicolon;
			case 48:
				return Key.OemQuotes;
			case 50:
				return Key.LeftShift;
			case 51:
				return Key.OemBackslash;
			case 52:
				return Key.Z;
			case 53:
				return Key.X;
			case 54:
				return Key.C;
			case 55:
				return Key.V;
			case 56:
				return Key.B;
			case 57:
				return Key.N;
			case 58:
				return Key.M;
			case 59:
				return Key.OemComma;
			case 60:
				return Key.OemPeriod;
			case 61:
				return Key.OemQuestion;
			case 62:
				return Key.RightShift;
			case 63:
				return Key.Multiply;
			case 64:
				return Key.LeftAlt;
			case 65:
				return Key.Space;
			case 77:
				return Key.NumLock;
			case 79:
				return Key.NumPad7;
			case 80:
				return Key.NumPad8;
			case 81:
				return Key.NumPad9;
			case 82:
				return Key.Subtract;
			case 83:
				return Key.NumPad4;
			case 84:
				return Key.NumPad5;
			case 85:
				return Key.NumPad6;
			case 86:
				return Key.Add;
			case 87:
				return Key.NumPad1;
			case 88:
				return Key.NumPad2;
			case 89:
				return Key.NumPad3;
			case 90:
				return Key.NumPad0;
			case 105:
				return Key.RightCtrl;
			case 106:
				return Key.Divide;
			case 108:
				return Key.RightAlt;			
			case 110:
				return Key.Home;			
			case 111:
				return Key.Up;
			case 112:
				return Key.PageUp;
			case 113:
				return Key.Left;
			case 114:
				return Key.Right;
			case 115:
				return Key.End;
			case 116:
				return Key.Down;			
			case 117:
				return Key.PageDown;
			case 118:
				return Key.Insert;
			case 119:
				return Key.Delete;
			case 133:
				return Key.LeftWindows;
			}

			return Key.None;
		}
	}
}

