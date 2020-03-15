using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace DEM.Net.Extension.SketchFab.Export
{
    public class ModelOptions
    {
        public ShadingType Shading { get; set; } = ShadingType.lit;
        /// <summary>
        /// Defines the background used. Either a color, a background (uid), an environment (uid) or transparent. eg: {"color": "#ffffff"}, {"environment": "uid"}, {"image": "uid"} or {"transparent": 1}
        /// <see cref="SkecthFabEnvironment"/>
        /// </summary>
        public ModelOptionsBackground Background { get; set; }
        public string Orientation { get; set; }// = "{\"axis\": [1, 1, 0], \"angle\": 34}";



    }
    public static class SkecthFabEnvironment
    {
        public static ModelOptionsBackground Kirby_Cove => new ModelOptionsBackground() { Environment = "8653449395004fd58820874bfff93ce7" };
        public static ModelOptionsBackground Footprint_Court => new ModelOptionsBackground() { Environment = "d348dfd2a8104ab0b8528f885d645eb3" };

    }
    public class ModelOptionsBackground
    {
        public string Environment { get; set; }
        public string Image { get; set; }
        public string Color { get; set; }
        //public bool Transparent { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ShadingType
    {
        lit,
        shadeless
    }
}