using Newtonsoft.Json;
using System.Collections.Generic;

namespace DEM.Net.Extension.SketchFab
{
    public class UploadModelRequest
    {

        /// <summary>
        /// SketchFab uuid
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
        /// Sets a password [pro only]
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }



        /// <summary>
        /// Sets a license (slug). This makes the model downloadable to others. All downloadable models must have isInspectable enabled.
        /// See https://api.sketchfab.com/v3/licenses
        /// 
        /// 
        /// </summary>
        [JsonProperty("license")]
        public string License { get; set; } = "by-nc-sa";


        [JsonIgnore]
        public List<string> Tags { get; set; } = new List<string> { "elevationapi" };

        [JsonProperty("tags")]
        public string TagsJson => string.Join("\n", Tags);

        [JsonIgnore]
        public List<string> Categories { get; set; } = new List<string> { "cultural-heritage-history", "places-travel" };

        [JsonProperty("categories")]
        public string CategoriesJson => string.Join("\n", Categories);

        /// <summary>
        /// Sets published after it is processed
        /// </summary>
        [JsonProperty("isPublished")]
        public bool IsPublished { get; set; }



        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("options")]
        public ModelOptions Options { get; set; } = new ModelOptions();

        [JsonIgnore]
        public string FilePath { get; set; }


    }
}