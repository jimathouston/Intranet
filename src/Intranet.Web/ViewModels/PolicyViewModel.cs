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
        private string _tags;

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
            PolicyTags = policy.PolicyTags;
            Title = policy.Title;
            Url = policy.Url;
        }

        /// <summary>
        /// This property is only for either returning <see cref="PolicyTag"/> as a string OR to be set by the model binder. Not both.
        /// </summary>
        public string Tags
        {
            get
            {
                if (_tags.IsNull())
                {
                    return this.PolicyTags.IsNull()
                        ? null
                        : String.Join(",", this.PolicyTags.Select(k => k.Tag?.Name)?.Where(k => !String.IsNullOrWhiteSpace(k)));
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
