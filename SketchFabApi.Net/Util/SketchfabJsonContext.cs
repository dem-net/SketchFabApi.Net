using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sketchfab;

/// <summary>
/// Source-generated System.Text.Json metadata for the Sketchfab API payloads:
/// camelCase, case-insensitive reads, enums as names, nulls omitted on write.
/// </summary>
[JsonSourceGenerationOptions(JsonSerializerDefaults.Web,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    UseStringEnumConverter = true)]
[JsonSerializable(typeof(Account))]
[JsonSerializable(typeof(Model))]
[JsonSerializable(typeof(PagedResult<Model>))]
[JsonSerializable(typeof(PagedResult<Collection>))]
[JsonSerializable(typeof(ModelOptions))]
[JsonSerializable(typeof(UploadResult))]
internal sealed partial class SketchfabJsonContext : JsonSerializerContext;

/// <summary>
/// Body of a successful model upload.
/// </summary>
internal sealed record UploadResult(string? uid, string? uri);
