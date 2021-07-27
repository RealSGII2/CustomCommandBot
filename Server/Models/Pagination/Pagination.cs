using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomCommandBot.Server.Models.Pagination
{
    public class Pagination<T> : List<T>
    {
        public string HeaderName { get; } = "X-Pagination";

        public PaginationOptions Options { get; init; } = new();
        public string HeaderContent
            => JsonConvert.SerializeObject(Options);

        public Pagination(IEnumerable<T> items, PaginationOptions options)
        {
            Options = options;
            Options.TotalCount = (int)Math.Ceiling(options.TotalPages / (double)options.PageSize);

            AddRange(items);
        }
    }

    public static class PaginationExtentions
    {
        public static Pagination<T> ToPagedList<T>(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new Pagination<T>(items, new PaginationOptions
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPages = items.Count
            });
        }
    }
}
