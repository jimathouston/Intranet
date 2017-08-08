using Intranet.Web.Common.Extensions;
using Intranet.Web.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.Web.ViewModels
{
    public class PolicyViewModel : Policy
    {
        private string _keywords;

        public PolicyViewModel()
        {
            // Empty, for model binding
        }

        public PolicyViewModel(Policy policy)
        {
            Category = policy.Category;
            Description = policy.Description;
            FileUrl = policy.FileUrl;
            Id = policy.Id;
            PolicyKeywords = policy.PolicyKeywords;
            Title = policy.Title;
            Url = policy.Url;
        }

        /// <summary>
        /// This property is only for either returning <see cref="PolicyKeywords"/> as a string OR to be set by the model binder. Not both.
        /// </summary>
        public string Keywords
        {
            get
            {
                if (_keywords.IsNull())
                {
                    return this.PolicyKeywords.IsNull()
                        ? null
                        : String.Join(",", this.PolicyKeywords.Select(k => k.Keyword?.Name)?.Where(k => !String.IsNullOrWhiteSpace(k)));
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
