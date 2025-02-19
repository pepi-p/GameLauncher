using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Launcher.Utilities
{
    public static class JsonUtility
    {
        public static Result<T> Deserialize<T>(string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) return Result<T>.Failure("ファイルが存在しません");

                var json = File.ReadAllText(filePath);

                if (json is null) return Result<T>.Failure("jsonファイルが空です");

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var result = JsonSerializer.Deserialize<T>(json, options);

                if (result is null) return Result<T>.Failure("データのデシリアライズに失敗しました");

                return Result<T>.Success(result);
            }
            catch (JsonException ex)
            {
                return Result<T>.Failure($"JSONのフォーマットが無効です: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<T>.Failure($"JsonUtility.Deserialize(): {ex.Message}");
            }
        }

        public static Result<string> Serialize<T>(T instance)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                string json = JsonSerializer.Serialize(instance, options);

                if (json is null) return Result<string>.Failure("データのシリアライズに失敗しました");

                return Result<string>.Success(json);
            }
            catch (Exception ex)
            {
                return Result<string>.Failure($"JsonUtility.Serialize(): {ex.Message}");
            }
        }

        /// <summary>
        /// クラスで受け取り，pathで指定された場所にjsonファイルを書き出す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">クラスのインスタンス</param>
        /// <param name="path">jsonファイルの保存場所</param>
        /// <returns></returns>
        public static Result DumpFile<T>(T instance, string path)
        {
            try
            {
                var jsonResult = Serialize(instance);
                if (!jsonResult.IsSuccess) return Result.Failure(jsonResult.ErrorMessage);

                if (File.Exists(path)) File.Delete(path);
                File.WriteAllText(path, jsonResult.Value);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"JsonUtility.DumpFile(): {ex.Message}");
            }
        }
    }
}
