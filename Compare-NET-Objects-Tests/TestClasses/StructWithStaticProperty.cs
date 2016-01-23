namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
	public struct StructWithStaticProperty
	{
		private readonly int _x;
		private readonly int _y;
		
		public StructWithStaticProperty(int x, int y)
		{
			_x = x;
			_y = y;
		}

		private static StructWithStaticProperty _origin = new StructWithStaticProperty(0, 0);
		
		public static StructWithStaticProperty Origin
		{
			get { return _origin; }
			set { _origin = value; }
		}
	}
}

