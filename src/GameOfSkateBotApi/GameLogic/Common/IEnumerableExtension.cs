namespace GameOfSkateBotApi.GameLogic.Common
{
	public static class IEnumerableExtension
	{
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> values)
		{
			var array = values.ToArray();
			Random.Shared.Shuffle(array);
			return array;
		}
	}
}