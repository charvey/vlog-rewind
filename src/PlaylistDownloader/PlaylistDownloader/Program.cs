using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PlaylistDownloader
{
	class Program
	{
		static void Main(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.AddUserSecrets(Assembly.GetEntryAssembly())
				.Build();

			var service = new YouTubeService(new BaseClientService.Initializer() { ApiKey = configuration["YouTubeApiKey"] });

			var playlistId = service.GetUploadPlaylist("CaseyNeistat");

			var items = service.GetAllItems(playlistId);

			File.WriteAllText($"{playlistId}.json", JsonConvert.SerializeObject(items.Select(i => new
			{
				title = i.Snippet.Title,
				videoId = i.Snippet.ResourceId.VideoId,
				publishedAt = i.Snippet.PublishedAt
			})));

			Console.WriteLine($"Downloaded {playlistId}");
		}
	}
}
