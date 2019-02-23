using EDennis.AspNetCore.Base.Testing;

namespace EDennis.Samples.Colors.InternalApi.Models {
    public static partial class ColorDbContextDataFactory {
        public static Color[] ColorRecordsFromRetriever { get; set; }
            = DataRetriever.Retrieve<Color>("ColorDb", "Colors");
    }
}