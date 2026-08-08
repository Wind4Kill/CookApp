
namespace CookApp.Model.FiltrationClasses
{
    public record Filter
    {
        public FiltrationOrder OrderType { get; init; }

        public FiltrationFilter FiltrationType { get; set; }

        public string? FiltrationData { get; set; }

        public int Page { get; set; }

        public Filter(string orderType, string filtrationType, string? filtrationData, int? page)
        {
            OrderType = Enum.TryParse<FiltrationOrder>(orderType, true, out FiltrationOrder resultOrderType) ? resultOrderType : FiltrationOrder.Default;

            FiltrationType = Enum.TryParse<FiltrationFilter>(filtrationType, true, out FiltrationFilter resultFilterType) ? resultFilterType : FiltrationFilter.Default;

            FiltrationData = filtrationData;

            Page = page is null ? 1 : page.Value;
        }
    }
}