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
		
		public WidgetKeyboardInputProvider (Widget widget)
		{
			widget.KeyPressEvent += HandleKeyPressEvent;
		}

		[GLib.ConnectBefore] 
		private void HandleKeyPressEvent (object o, KeyPressEventArgs e)
		{
			var key = Convert (e.Event.Key);
			RaiseKeyPressEventHandler (new KeyEventArgs (key));
		}

		private void RaiseKeyPressEventHandler (KeyEventArgs args)
		{
			if (KeyPressEvent != null) {
				KeyPressEvent (this, args);
			}
		}

		private Key Convert (Gdk.Key k)
		{
			switch (k) {
			case Gdk.Key.Cancel:
				return Key.Cancel;
			case Gdk.Key.BackSpace:
				return Key.Back;
			case Gdk.Key.Tab:
				return Key.Tab;
			case Gdk.Key.Linefeed:
				return Key.LineFeed;
			case Gdk.Key.Clear:
				return Key.Clear;
			case Gdk.Key.Return:
				return Key.Return;
			case Gdk.Key.KP_Enter:
				return Key.Enter;
			case Gdk.Key.Pause:
				return Key.Pause;
			case Gdk.Key.Caps_Lock:
				return Key.CapsLock;
			case Gdk.Key.kana_MO:
				return Key.KanaMode;
			case Gdk.Key.Hangul:
				return Key.HangulMode;
			case Gdk.Key.Kanji:
				return Key.KanjiMode;
			case Gdk.Key.Escape:
				return Key.Escape;
			case Gdk.Key.space:
				return Key.Space;		
			case Gdk.Key.Page_Up:
				return Key.PageUp;
			case Gdk.Key.Page_Down:
				return Key.PageDown;
			case Gdk.Key.KP_End:
				return Key.End;
			case Gdk.Key.KP_Home:
				return Key.Home;
			case Gdk.Key.Left:
				return Key.Left;
			case Gdk.Key.Up:
				return Key.Up;
			case Gdk.Key.Right:
				return Key.Right;
			case Gdk.Key.Down:
				return Key.Down;
			case Gdk.Key.Select:
				return Key.Select;
			case Gdk.Key.Print:
				return Key.Print;
			case Gdk.Key.Execute:
				return Key.Execute;
			case Gdk.Key.Key_3270_PrintScreen:
				return Key.PrintScreen;
			case Gdk.Key.Insert:
				return Key.Insert;
			case Gdk.Key.Delete:
				return Key.Delete;
			case Gdk.Key.Help:
				return Key.Help;
			case Gdk.Key.Key_0:
				return Key.D0;
			case Gdk.Key.Key_1:
				return Key.D1;
			case Gdk.Key.Key_2:
				return Key.D2;
			case Gdk.Key.Key_3:
				return Key.D3;
			case Gdk.Key.Key_4:
				return Key.D4;
			case Gdk.Key.Key_5:
				return Key.D5;
			case Gdk.Key.Key_6:
				return Key.D6;
			case Gdk.Key.Key_7:
				return Key.D7;
			case Gdk.Key.Key_8:
				return Key.D8;
			case Gdk.Key.Key_9:
				return Key.D9;
			case Gdk.Key.A:
				return Key.A;
			case Gdk.Key.B:
				return Key.B;
			case Gdk.Key.C:
				return Key.C;
			case Gdk.Key.D:
				return Key.D;
			case Gdk.Key.E:
				return Key.E;
			case Gdk.Key.F:
				return Key.F;
			case Gdk.Key.G:
				return Key.G;
			case Gdk.Key.H:
				return Key.H;
			case Gdk.Key.I:
				return Key.I;
			case Gdk.Key.J:
				return Key.J;
			case Gdk.Key.K:
				return Key.K;
			case Gdk.Key.L:
				return Key.L;
			case Gdk.Key.M:
				return Key.M;
			case Gdk.Key.N:
				return Key.N;
			case Gdk.Key.O:
				return Key.O;
			case Gdk.Key.P:
				return Key.P;
			case Gdk.Key.Q:
				return Key.Q;
			case Gdk.Key.R:
				return Key.R;
			case Gdk.Key.S:
				return Key.S;
			case Gdk.Key.T:
				return Key.T;
			case Gdk.Key.U:
				return Key.U;
			case Gdk.Key.V:
				return Key.V;
			case Gdk.Key.W:
				return Key.W;
			case Gdk.Key.X:
				return Key.X;
			case Gdk.Key.Y:
				return Key.Y;
			case Gdk.Key.Z:
				return Key.Z;
			case Gdk.Key.multiply:
				return Key.Multiply;
			case Gdk.Key.plus:
				return Key.Add;	
			case Gdk.Key.KP_Separator:
				return Key.Separator;
			case Gdk.Key.minus:
				return Key.Subtract;
			case Gdk.Key.KP_Decimal:
				return Key.Decimal;
			case Gdk.Key.KP_Divide:
				return Key.Divide;
			case Gdk.Key.F1:
				return Key.F1;
			case Gdk.Key.F2:
				return Key.F2;
			case Gdk.Key.F3:
				return Key.F3;
			case Gdk.Key.F4:
				return Key.F4;
			case Gdk.Key.F5:
				return Key.F5;
			case Gdk.Key.F6:
				return Key.F6;
			case Gdk.Key.F7:
				return Key.F7;
			case Gdk.Key.F8:
				return Key.F8;
			case Gdk.Key.F9:
				return Key.F9;
			case Gdk.Key.F10:
				return Key.F10;
			case Gdk.Key.F11:
				return Key.F11;
			case Gdk.Key.F12:
				return Key.F12;
			case Gdk.Key.F13:
				return Key.F13;
			case Gdk.Key.F14:
				return Key.F14;
			case Gdk.Key.F15:
				return Key.F15;
			case Gdk.Key.F16:
				return Key.F16;
			case Gdk.Key.F17:
				return Key.F17;
			case Gdk.Key.F18:
				return Key.F18;
			case Gdk.Key.F19:
				return Key.F19;
			case Gdk.Key.F20:
				return Key.F20;
			case Gdk.Key.F21:
				return Key.F21;
			case Gdk.Key.F22:
				return Key.F22;
			case Gdk.Key.F23:
				return Key.F23;
			case Gdk.Key.F24:
				return Key.F24;
			case Gdk.Key.Num_Lock:
				return Key.NumLock;
			case Gdk.Key.Scroll_Lock:
				return Key.Scroll;
			case Gdk.Key.Shift_L:
				return Key.LeftShift;
			case Gdk.Key.Shift_R:
				return Key.RightShift;
			case Gdk.Key.Control_L:
				return Key.LeftCtrl;
			case Gdk.Key.Control_R:
				return Key.RightCtrl;
			case Gdk.Key.Alt_L:
				return Key.LeftAlt;
			case Gdk.Key.Alt_R:
				return Key.RightAlt;
			case Gdk.Key.Key_3270_EraseEOF:			
				return Key.EraseEof;
			case Gdk.Key.Key_3270_Play:
				return Key.Play;	
			case Gdk.Key.comma:
				return Key.OemComma;
			case Gdk.Key.colon:
				return Key.Oem1;
			default:
				return Key.None;
			}
		}
	}
}

