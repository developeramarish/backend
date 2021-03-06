﻿using ShareBook.Domain.Common;
using ShareBook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShareBook.Domain
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public string Slug { get; set; }

        public byte[] ImageBytes { get; set; }

        public string ImageSlug { get; set; }

        public FreightOption FreightOption { get; set; }

        public Guid? UserId { get; set; }

        public Guid CategoryId { get; set; }

        public User User { get; set; }

        public Category Category { get; set; }

        public bool Approved { get; set; } = false;

        public virtual ICollection<BookUser> BookUsers { get; set; }

        public string ImageUrl { get; set; }

        public string ImageName { get; set; }

        public bool Donated()
            => BookUsers.Any(x => x.Status == DonationStatus.Donated);

        public BookStatus Status() {
            BookStatus response = BookStatus.Unknow;

            if (this.BookUsers == null) 
                return response;

            bool visible        = this.Approved;
            int totalInterested = this.TotalInterested();
            bool donated        = this.Donated();

            if (!visible && totalInterested == 0) {
                response = BookStatus.WaitingApproval;
            } else if (visible && !donated) {
                response = BookStatus.Available;
            } else if (!visible && !donated && totalInterested > 0) {
                response = BookStatus.Invisible;
            } else if (donated) {
                response = BookStatus.Donated;
            }

            return response;
        }
        
        public int TotalInterested()
        {
            return this.BookUsers?.Count ?? 0;
        }

        public int DaysInShowcase() 
        {
            TimeSpan diff = (TimeSpan) (DateTime.Now - this.CreationDate);
            return diff.Days;
        }
    }
}
