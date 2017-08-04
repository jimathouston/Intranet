using Intranet.Web.Common.Extensions;
using Intranet.Web.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.Web.ViewModels
{
    public class FaqViewModel : Faq
    {
        private string _keywords;

        public FaqViewModel()
        {
            // Empty, for model binding
        }

        public FaqViewModel(Faq faq)
        {
            Answer = faq.Answer;
            Category = faq.Category;
            FaqKeywords = faq.FaqKeywords;
            Id = faq.Id;
            Question = faq.Question;
            Url = faq.Url;
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
                    return this.FaqKeywords.IsNull()
                        ? null
                        : String.Join(",", this.FaqKeywords.Select(k => k.Keyword?.Name)?.Where(k => !String.IsNullOrWhiteSpace(k)));
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
    }
}
