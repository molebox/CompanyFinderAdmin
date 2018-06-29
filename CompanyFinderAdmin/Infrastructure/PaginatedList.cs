﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.Infrastructure
{
    /// <summary>
    /// Class to servoce pagination
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginatedList<T> : List<T>
    {
        /// <summary>
        /// The pages index
        /// </summary>
        public int PageIndex { get; private set; }
        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages { get; private set; }
        /// <summary>
        /// Paginated list for sorting the page size
        /// </summary>
        /// <param name="items"></param>
        /// <param name="count"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }
        /// <summary>
        /// Previous page
        /// </summary>
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }
        /// <summary>
        /// Next page
        /// </summary>
        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
        /// <summary>
        /// The CreateAsync method in this code takes page size and page number and applies the appropriate Skip and Take statements to the IQueryable
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
