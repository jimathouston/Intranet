using Intranet.Web.Common.Extensions;
using Intranet.Web.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.Web.ViewModels
{
    public class NewsViewModel : News
    {
        private string _keywords;

        public NewsViewModel()
        {
            // Empty, for model binding
        }

        public NewsViewModel(News news)
        {
            Created = news.Created;
            HeaderImage = news.HeaderImage;
            Id = news.Id;
            NewsKeywords = news.NewsKeywords;
            Published = news.Published;
            Text = news.Text;
            Title = news.Title;
            Updated = news.Updated;
            User = news.User;
            UserId = news.UserId;
            Url = news.Url;
        }

        /// <summary>
        /// This property is only for either returning <see cref="NewsKeywords"/> as a string OR to be set by the model binder. Not both.
        /// </summary>
        public string Keywords
        {
            get
            {
                if (_keywords.IsNull())
                {
                    return this.NewsKeywords.IsNull()
                        ? null
                        : String.Join(",", this.NewsKeywords.Select(k => k.Keyword?.Name)?.Where(k => !String.IsNullOrWhiteSpace(k)));
                }

                return _keywords;
            }

            set
            {
                _keywords = value;
            }
        }

        // Workaround to not get an excetion due to the required attribute
        public new string Url { get; set; }
        public new string UserId { get; set; }

        public string UrlWithDate => $"{this.Created.Year}/{this.Created.Month}/{this.Created.Day}/{Url}";
    }
}
