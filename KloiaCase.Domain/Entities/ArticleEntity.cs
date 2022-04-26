using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KloiaCase.Domain.Entities
{
    public class ArticleEntity : EntityBase
    {
        public string Title { get; set; }
        
        public string Author { get; set; }

        public string ArticleContent { get; set; }

        public DateTime PublishDate { get; set; }

        public int StarCount { get; set; }

        [ForeignKey("ArticleId")]
        public virtual ICollection<ReviewEntity> Reviews { get; set; }
    }
}
