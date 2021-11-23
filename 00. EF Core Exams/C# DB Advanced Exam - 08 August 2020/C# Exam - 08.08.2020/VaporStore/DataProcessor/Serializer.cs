namespace VaporStore.DataProcessor
{
	using System;
    using System.Text;
    using Data;

	public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
			var result = new StringBuilder();

			return result.ToString().TrimEnd();
		}

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{
			var result = new StringBuilder();

			return result.ToString().TrimEnd();
		}
	}
}