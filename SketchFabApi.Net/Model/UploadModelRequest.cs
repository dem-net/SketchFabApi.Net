using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sketchfab
{
    public class UploadModelRequest
    {

        [JsonIgnore]
        public string Source { get; set; }

        /// <summary>
        /// Set to Bearer when token was obtained using OAuth2 flow
        /// Set to Token if it is your token (API Key) copied from your Sketchfab profile
        /// </summary>
        [JsonIgnore]
        public TokenType TokenType { get; set; } = TokenType.Token;


        /// <summary>
        /// Sketchfab uuid
        /// </summary>
        [JsonProperty("uid")]
        public string ModelId { get; set; }


        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Sets private [pro only]
        /// </summary>

        [JsonProperty("private")]
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Enables 2D view in model inspector. All downloadable models must have isInspectable enabled.
        /// </summary>
        [JsonProperty("isInspectable")]
        public bool IsInspectable { get; set; }

        /// <summary>
        /// Sets published after it is processed
        /// </summary>
        [JsonProperty("isPublished")]
        public bool IsPublished { get; set; }

        /// <summary>
        /// Sets a password [pro only]
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }



        /// <summary>
        /// Sets a license (slug). This makes the model downloadable to others. All downloadable models must have isInspectable enabled.
        /// See https://api.Sketchfab.com/v3/licenses
        /// 
        /// 
        /// </summary>
        [JsonProperty("license")]
        public string License { get; set; } = "by-nc-sa";


        [JsonIgnore]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonProperty("tags")]
        public string TagsJson => string.Join("\n", Tags);

        [JsonIgnore]
        public List<string> Categories { get; set; } = new List<string>();

        [JsonProperty("categories")]
        public string CategoriesJson => string.Join("\n", Categories);

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("options")]
        public ModelOptions Options { get; set; } = new ModelOptions();

        [JsonIgnore]
        public string FilePath { get; set; }


    }

    public class UpdateModelRequest
    {

        /// <summary>
        /// Sketchfab uuid
        /// </summary>
        [JsonProperty("uid")]
        public string ModelId { get; set; }


        [JsonIgnore]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonProperty("tags")]
        public string TagsJson => string.Join("\n", Tags);

        [JsonIgnore]
        public List<string> Categories { get; set; } = new List<string>();

        [JsonProperty("categories")]
        public string CategoriesJson => string.Join("\n", Categories);

       

    }
}