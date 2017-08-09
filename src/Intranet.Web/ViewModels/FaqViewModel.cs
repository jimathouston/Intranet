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
        private string _tags;

        public FaqViewModel()
        {
            // Empty, for model binding
        }

        public FaqViewModel(Faq faq)
        {
            Answer = faq.Answer;
            Category = faq.Category;
            FaqTags = faq.FaqTags;
            Id = faq.Id;
            Question = faq.Question;
            Url = faq.Url;
        }

        /// <summary>
        /// This property is only for either returning <see cref="FaqTag"/> as a string OR to be set by the model binder. Not both.
        /// </summary>
        public string Tags
        {
            get
            {
                if (_tags.IsNull())
                {
                    return this.FaqTags.IsNull()
                        ? null
                        : String.Join(",", this.FaqTags.Select(k => k.Tag?.Name)?.Where(k => !String.IsNullOrWhiteSpace(k)));
                }

                return _tags;
            }

            set
            {
                _tags = value;
            }
        }

        // Workaround to not get an excetion due to the required attribute
        public new string Url { get; set; }
    }
}
