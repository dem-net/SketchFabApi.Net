using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sketchfab;

public class UploadModelRequest
{

    [JsonIgnore]
    public string? Source { get; set; }

    /// <summary>
    /// Set to Bearer when token was obtained using OAuth2 flow
    /// Set to Token if it is your token (API Key) copied from your Sketchfab profile
    /// </summary>
    [JsonIgnore]
    public TokenType TokenType { get; set; } = TokenType.Token;


    /// <summary>
    /// Sketchfab uuid
    /// </summary>
    [JsonPropertyName("uid")]
    public string? ModelId { get; set; }


    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Sets private [pro only]
    /// </summary>

    [JsonPropertyName("private")]
    public bool IsPrivate { get; set; }

    /// <summary>
    /// Enables 2D view in model inspector. All downloadable models must have isInspectable enabled.
    /// </summary>
    [JsonPropertyName("isInspectable")]
    public bool IsInspectable { get; set; }

    /// <summary>
    /// Sets published after it is processed
    /// </summary>
    [JsonPropertyName("isPublished")]
    public bool IsPublished { get; set; }

    /// <summary>
    /// Sets a password [pro only]
    /// </summary>
    [JsonPropertyName("password")]
    public string? Password { get; set; }



    /// <summary>
    /// Sets a license (slug). This makes the model downloadable to others. All downloadable models must have isInspectable enabled.
    /// See https://api.Sketchfab.com/v3/licenses
    /// 
    /// 
    /// </summary>
    [JsonPropertyName("license")]
    public string License { get; set; } = "by-nc-sa";


    [JsonIgnore]
    public List<string> Tags { get; set; } = [];

    [JsonPropertyName("tags")]
    public string TagsJson => string.Join("\n", Tags);

    [JsonIgnore]
    public List<string> Categories { get; set; } = [];

    [JsonPropertyName("categories")]
    public string CategoriesJson => string.Join("\n", Categories);

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("options")]
    public ModelOptions Options { get; set; } = new ModelOptions();

    [JsonIgnore]
    public string? FilePath { get; set; }


}

public class UpdateModelRequest
{

    /// <summary>
    /// Sketchfab uuid
    /// </summary>
    [JsonPropertyName("uid")]
    public string? ModelId { get; set; }


    [JsonIgnore]
    public List<string> Tags { get; set; } = [];

    [JsonPropertyName("tags")]
    public string TagsJson => string.Join("\n", Tags);

    [JsonIgnore]
    public List<string> Categories { get; set; } = [];

    [JsonPropertyName("categories")]
    public string CategoriesJson => string.Join("\n", Categories);

   

}