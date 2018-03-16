namespace framework
{
	public static class Game
	{
		private static ObjectPool objectPool;

		public static ObjectPool ObjectPool
		{
			get
			{
				return objectPool ?? (objectPool = new ObjectPool());
			}
		}

		public static void Close()
		{
			objectPool = null;
		}
	}
}