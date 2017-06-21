using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Intranet.API.Common.Enums;

namespace Intranet.API.Domain.Models.Entities
{
    public class Image
    {
        public int Id { get; set; }

        [Required]
        public String FileName { get; set; }

        // TODO: Validate all urls!
        public IEnumerable<String> Urls
        {
            get
            {
                if (String.IsNullOrWhiteSpace(FileName))
                {
                    yield return null;
                }
                else
                {
                    foreach (var variant in Enum.GetValues(typeof(ImageVariantType)))
                    {
                        var size = variant.ToString().Split('_')?.Last()?.Split('x');
                        var width = size?.First();
                        var height = size?.Last();

                        if (int.TryParse(width, out int w) && int.TryParse(height, out int h))
                        {
                            yield return $"/api/v1/image/{w}/{h}/{FileName.ToLower()}";
                        }
                        else
                        {
                            yield return $"/api/v1/image/{FileName.ToLower()}";
                        }
                    }
                }
            }
        }
    }
}
