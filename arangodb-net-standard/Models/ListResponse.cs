using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace ArangoDBNetStandard.Models
{
    public abstract class ListResponse<TItem> : ResponseBase, IReadOnlyList<TItem>
    {
        protected readonly IList<TItem> Results;

        protected ListResponse(IEnumerable<TItem> items, ApiResponse responseDetails) : base(responseDetails)
        {
            Results = new List<TItem>(items ?? new List<TItem>());
        }

        protected ListResponse([NotNull] ApiResponse responseDetails) : base(responseDetails)
        {
            Results = new List<TItem>();
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return Results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => Results.Count;

        public TItem this[int index] => Results[index];
    }
}