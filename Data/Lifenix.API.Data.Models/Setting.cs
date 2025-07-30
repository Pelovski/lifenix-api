namespace Lifenix.API.Data.Models
{
    using Lifenix.API.Data.Common.Models;

    public class Setting : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
