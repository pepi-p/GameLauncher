using Launcher.Interfaces;
using Launcher.Models;
using Launcher.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Launcher.Services
{
    public class ApiClient : IApiClient
    {
        private readonly ILogger _logger;
        private string _serverAddress;
        private readonly HttpClient _client;

        public ApiClient(IServerAddressProvider addressProvider, ILogger logger)
        {
            _logger = logger;
            _serverAddress = addressProvider.GetServerAddress();
            _client = new HttpClient();
        }

        public async Task<Result> HealthCheck()
        {
            try
            {
                var response = await _client.GetAsync(_serverAddress + "/api/health");
                response.EnsureSuccessStatusCode();

                string body = await response.Content.ReadAsStringAsync();

                return Result.Success();
            }
            catch (HttpRequestException e)
            {
                return Result.Failure($"Health check failed : {e.Message}");
            }
            catch (TaskCanceledException e)
            {
                return Result.Failure($"Health check timed out: {e.Message}");
            }
        }

        public async Task<Result<GameMetadata>> GetGameMetadata(int id)
        {
            try
            {
                var response = await _client.GetAsync(_serverAddress + $"/api/search?id={id}");
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();

                // jsonをアッパーキャメルケースで読む
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var data = JsonSerializer.Deserialize<GameMetadata>(body, options);

                return Result<GameMetadata>.Success(data);
            }
            catch (HttpRequestException e)
            {
                return Result<GameMetadata>.Failure($"ApiClientService.GetGameMetadata(): {e.Message}");
            }
            catch (InvalidOperationException e)
            {
                return Result<GameMetadata>.Failure($"ApiClientService.GetGameMetadata(): {e.Message}");
            }
        }

        public async Task<Result<List<GameMetadata>>> GetGameMetadataAll()
        {
            try
            {
                var response = await _client.GetAsync(_serverAddress + "/api/search");
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();

                // jsonをアッパーキャメルケースで読む
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var datas = JsonSerializer.Deserialize<List<GameMetadata>>(body, options);

                return Result<List<GameMetadata>>.Success(datas);
            }
            catch (HttpRequestException e)
            {
                return Result<List<GameMetadata>>.Failure($"ApiClientService.GetGameMetadataAll(): {e.Message}");
            }
            catch (InvalidOperationException e)
            {
                return Result<List<GameMetadata>>.Failure($"ApiClientService.GetGameMetadataAll(): {e.Message}");
            }
        }

        public async Task<Result<Stream>> GetGameZip(int id)
        {
            try
            {
                var response = await _client.GetAsync(_serverAddress + "/api/download/game/" + id, HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();
                return Result<Stream>.Success(stream);
            }
            catch (HttpRequestException e)
            {
                return Result<Stream>.Failure($"ApiClientService.GetGameZip(): {e.Message}");
            }
        }

        public async Task<Result<Stream>> GetGameImage(int id)
        {
            try
            {
                var response = await _client.GetAsync(_serverAddress + "/api/download/image/" + id, HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();
                return Result<Stream>.Success(stream);
            }
            catch (HttpRequestException e)
            {
                return Result<Stream>.Failure($"ApiClientService.GetGameImage(): {e.Message}");
            }
        }
    }
}
