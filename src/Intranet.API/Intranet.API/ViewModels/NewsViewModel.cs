using Intranet.API.Domain.Models.Entities;
using Intranet.Shared.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.API.ViewModels
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

        [JsonIgnore]
        public new ICollection<NewsKeyword> NewsKeywords { get; set; }

        public new string Url { get; set; }
    }
}
