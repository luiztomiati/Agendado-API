namespace Agendado.Shared
{
    public class ResultadoPagincao<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int QtdPage { get; set; }
    }
}
