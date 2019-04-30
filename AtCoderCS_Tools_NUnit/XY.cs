using System;

namespace Tools
{
	public class XY : IComparable<XY>, IFormattable {
		public readonly int X;
		public readonly int Y;

		public XY() {}
		public XY(int x, int y) {
			X = x;
			Y = y;
		}
		public XY(int[] ary) {
			X = ary[0];
			Y = ary[1];
		}

		public int CompareTo(XY other) {
			var dx = this.X - other.X;
			if (0 < dx)
				return 1;
			else if (dx < 0)
				return -1;
			else {
				var dy = this.Y - other.Y;
				if (0 < dy)
					return 1;
				else if (dy < 0)
					return -1;
				else
					return 0;
			}
		}

		public override string ToString() {
			return ToString(null, null);
		}
		// format等の引数は一切無視
		public string ToString(string format, IFormatProvider formatProvider) {
			return $"({X}, {Y})";
		}
	}
}
