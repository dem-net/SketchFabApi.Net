using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DEM.Net.Extension.SketchFab.Export
{
    public class ModelOptions
    {
        public ShadingType Shading { get; set; }

        public string Background { get; set; } = "4eda3e78ed214dc8852c439dc64b9c9d"; // Road in Dordogne https://docs.sketchfab.com/data-api/v3/index.html#!/environment/get_v3_me_environments

        public string Orientation { get; set; } // = "{\"axis\": [1, 1, 0], \"angle\": 34}";
    }


    public enum ShadingType
    {
        Lit,
        Shadeless
    }
}