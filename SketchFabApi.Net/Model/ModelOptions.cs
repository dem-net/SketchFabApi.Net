using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace SketchFab
{
    public class ModelOptions
    {
        public ShadingType Shading { get; set; } = ShadingType.lit;
        /// <summary>
        /// Defines the background used. Either a color, a background (uid), an environment (uid) or transparent. eg: {"color": "#ffffff"}, {"environment": "uid"}, {"image": "uid"} or {"transparent": 1}
        /// <see cref="SkecthFabEnvironment"/>
        /// </summary>
        public ModelOptionsBackground Background { get; set; } = SkecthFabEnvironment.Studio;
        public string Orientation { get; set; }// = "{\"axis\": [1, 1, 0], \"angle\": 34}";



    }
    public static class SkecthFabEnvironment
    {
        public static ModelOptionsBackground Kirby_Cove => new ModelOptionsBackground() { Environment = "8653449395004fd58820874bfff93ce7" }; // too much light
        public static ModelOptionsBackground Footprint_Court => new ModelOptionsBackground() { Environment = "d348dfd2a8104ab0b8528f885d645eb3" }; // ok
        public static ModelOptionsBackground Pine_Tree_Arch => new ModelOptionsBackground() { Environment = "e2aa1ab3582c4feab7371baf1e4cd734" }; // ok
        public static ModelOptionsBackground Tokyo_Big_Sight => new ModelOptionsBackground() { Environment = "2a016b232e444ef3a6ba323c51aa5063" }; // OK
        public static ModelOptionsBackground Tropical_Ruins => new ModelOptionsBackground() { Environment = "5335c7c5c2434866ac8a442157f24f5e" }; // OK
        public static ModelOptionsBackground Milky_Way => new ModelOptionsBackground() { Environment = "102d22e28ca34190a8470402ccdc35d3" };// too dark
        public static ModelOptionsBackground Queen_Mary_Chimney => new ModelOptionsBackground() { Environment = "cd5e5b1607d844cdb928e96ff9c36b5c" }; // too dark
        public static ModelOptionsBackground Studio => new ModelOptionsBackground() { Environment = "df380da788ee444885722735039b0c09" }; // too much light
        public static ModelOptionsBackground Industrial_Room => new ModelOptionsBackground() { Environment = "9190e8da70694ef3b9d1d0c01541917e" }; // OK zenithal (no nice shadows)
        public static ModelOptionsBackground Road_in_Dordogne => new ModelOptionsBackground() { Environment = "4eda3e78ed214dc8852c439dc64b9c9d" }; // too much light
        public static ModelOptionsBackground Road_in_Tenerife_Mountain => new ModelOptionsBackground() { Environment = "749a0594343e4ff9a2875cb411d0ad1a" };// too much light
        public static ModelOptionsBackground Gdansk_Shipyard_Buildings => new ModelOptionsBackground() { Environment = "3e2ad3e1f1ea47679f045a7eb0e6af49" };// too much light
        public static ModelOptionsBackground Glazed_Patio_by_Restaurant => new ModelOptionsBackground() { Environment = "2fa15e0b1cee45a5b02605a288934e1b" };// too dark
        public static ModelOptionsBackground Urban_Exploring_Interior => new ModelOptionsBackground() { Environment = "c281dff366844cbc8b33179337037f42" }; // too dark
        public static ModelOptionsBackground Gareoult => new ModelOptionsBackground() { Environment = "e871dd2920334a0f9c3107a00da3c24a" };// too much light
        public static ModelOptionsBackground Muir_Wood => new ModelOptionsBackground() { Environment = "41192cc664484a0fa565da3361d10c9c" };// too much light
        public static ModelOptionsBackground Treasure_Island => new ModelOptionsBackground() { Environment = "5e7120378b00431ea18151e97a8366ec" }; // too much light
        public static ModelOptionsBackground Trinitatis_Church => new ModelOptionsBackground() { Environment = "e00dc642058b4176a4aaa449ea8ad5f8" };// too much light


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