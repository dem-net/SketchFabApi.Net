using System.Collections.Generic;

namespace DEM.Net.Extension.SketchFab.Export
{
    public class UploadModelRequest
    {
        public string Token { get; set; }
        public string FilePath { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Sets private [pro only]
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Enables 2D view in model inspector. All downloadable models must have isInspectable enabled.
        /// </summary>
        public bool IsInspectable { get; set; }

        /// <summary>
        /// Sets a password [pro only]
        /// </summary>
        public string Password { get; set; }


        /// <summary>
        /// Sets a license (slug). This makes the model downloadable to others. All downloadable models must have isInspectable enabled.
        /// See https://api.sketchfab.com/v3/licenses
        /// 
        /// 
        /// </summary>
        public string License { get; set; } = "by-nc-sa";

        public List<string> Tags { get; set; } = new List<string> { "elevationapi" };
        public List<string> Categories { get; set; } = new List<string> { "cultural-heritage-history", "places-travel" };

        /// <summary>
        /// Sets published after it is processed
        /// </summary>
        public bool IsPublished { get; set; }

        public string Description { get; set; }

        public string Options { get; set; }
    }
}