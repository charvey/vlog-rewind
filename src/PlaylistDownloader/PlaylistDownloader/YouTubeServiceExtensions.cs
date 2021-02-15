using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Collections.Generic;
using System.Linq;

namespace PlaylistDownloader
{
	static class YouTubeServiceExtensions
	{
		public static string GetUploadPlaylist(this YouTubeService service, string username)
		{
			var request=service.Channels.List("contentDetails");
			request.ForUsername = username;
			return request.Execute().Items.Single().ContentDetails.RelatedPlaylists.Uploads;
		}

		public static IEnumerable<PlaylistItem> GetAllItems(this YouTubeService service, string playlistId)
		{
			string nextToken = null;
			do
			{
				var request = service.PlaylistItems.List("snippet");
				request.PlaylistId = playlistId;
				request.MaxResults = 50;
				request.PageToken = nextToken;

				var response = request.Execute();
				foreach (var item in response.Items)
					yield return item;

				nextToken = response.NextPageToken;
			} while (nextToken != null);
		}
	}
}
