namespace KloiaCase.Domain.Entities
{
    public class ReviewEntity : EntityBase
    {
        public int ArticleId { get; set; }

        public string Reviewer { get; set; }

        public string ReviewContent { get; set; }
    }
}