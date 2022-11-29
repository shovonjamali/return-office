namespace WFO.RTO_CLV.RERWeb.Entities
{
    public class EmailObject
    {
        public EmailAddress Recipients { get; set; }
        public EmailContent Content { get; set; }

        public EmailObject() { }

        public EmailObject(EmailAddress recipients, EmailContent content)
        {
            Recipients = recipients;
            Content = content;
        }
    }
}