namespace EventsAPI.DTOs
{
    public class PaginatedResult<T> where T : class
    {
        /// <summary>
        /// Номер страницы
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Размер страницы
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Количество элементов в текущем ответе
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Всего найдено
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Элементы
        /// </summary>
        public IEnumerable<T> Items { get; set; }
    }
}
