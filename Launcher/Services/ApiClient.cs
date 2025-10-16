using Launcher.Interfaces;
using Launcher.Models;
using Launcher.Models.Dtos;
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

        public async Task<Result<GameMetadata>> GetGameMetadata(int id)
        {
            try
            {
                var response = await _client.GetAsync(_serverAddress + $"/api/metadata/get?id={id}");
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();

                // jsonをアッパーキャメルケースで読む
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var data = JsonSerializer.Deserialize<ReadData>(body, options);
                var metadata = DotsConverter.ReadDataToMetadata(data);

                return Result<GameMetadata>.Success(metadata);
            }
            catch (HttpRequestException e)
            {
                _logger.Log(LogLevel.Error, $"ApiClientService.GetGameMetadata(): {e.Message}");
                return Result<GameMetadata>.Failure($"ApiClientService.GetGameMetadata(): {e.Message}");
            }
            catch (InvalidOperationException e)
            {
                _logger.Log(LogLevel.Error, $"ApiClientService.GetGameMetadata(): {e.Message}");
                return Result<GameMetadata>.Failure($"ApiClientService.GetGameMetadata(): {e.Message}");
            }
        }

        public async Task<Result<List<GameMetadata>>> GetGameMetadataAll()
        {
            try
            {
                var response = await _client.GetAsync(_serverAddress + "/api/metadata/getall");
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();

                // jsonをアッパーキャメルケースで読む
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var datas = JsonSerializer.Deserialize<List<ReadData>>(body, options);

                List<GameMetadata> metadatas = new();

                foreach (var data in datas)
                {
                    metadatas.Add(DotsConverter.ReadDataToMetadata(data));
                }

                return Result<List<GameMetadata>>.Success(metadatas);
            }
            catch (HttpRequestException e)
            {
                _logger.Log(LogLevel.Error, $"ApiClientService.GetGameMetadataAll(): {e.Message}");
                return Result<List<GameMetadata>>.Failure($"ApiClientService.GetGameMetadataAll(): {e.Message}");
            }
            catch (InvalidOperationException e)
            {
                _logger.Log(LogLevel.Error, $"ApiClientService.GetGameMetadataAll(): {e.Message}");
                return Result<List<GameMetadata>>.Failure($"ApiClientService.GetGameMetadataAll(): {e.Message}");
            }
        }

        public async Task<Result<Stream>> GetGameZip(int id)
        {
            try
            {
                // var response = await _client.GetAsync(_serverAddress + "/api/download/game/" + id, HttpCompletionOption.ResponseHeadersRead);
                var response = await _client.GetAsync(_serverAddress + $"/api/metadata/download/game?id={id}");
                var stream = await response.Content.ReadAsStreamAsync();
                return Result<Stream>.Success(stream);
            }
            catch (HttpRequestException e)
            {
                _logger.Log(LogLevel.Error, $"ApiClientService.GetGameZip(): {e.Message}");
                return Result<Stream>.Failure($"ApiClientService.GetGameZip(): {e.Message}");
            }
        }

        public async Task<Result<Stream>> GetGameImage(int id)
        {
            try
            {
                var response = await _client.GetAsync(_serverAddress + $"/api/metadata/download/image?id={id}");
                var stream = await response.Content.ReadAsStreamAsync();
                return Result<Stream>.Success(stream);
            }
            catch (HttpRequestException e)
            {
                _logger.Log(LogLevel.Error, $"ApiClientService.GetGameImage(): {e.Message}");
                return Result<Stream>.Failure($"ApiClientService.GetGameImage(): {e.Message}");
            }
        }
    }
}
