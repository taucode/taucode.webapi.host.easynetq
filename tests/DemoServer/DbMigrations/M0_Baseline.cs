using FluentMigrator;

namespace DbMigrations
{
    [Migration(0)]
    public class M0_Baseline : AutoReversingMigration
    {
        public override void Up()
        {
            // email message
            this.Create.Table("msg")
                .WithColumn("id")
                    .AsGuid()
                    .NotNullable()
                    .PrimaryKey("PK_msg")
                .WithColumn("correlation_id")
                    .AsString(100)
                    .NotNullable()
                    .Indexed("IX_msg_correlationId")
                .WithColumn("sent_at")
                    .AsDateTime()
                    .NotNullable()
                    .Indexed("IX_msg_sentAt")
                .WithColumn("sender")
                    .AsString(100)
                    .NotNullable()
                    .Indexed("IX_msg_sender")
                .WithColumn("recipient")
                    .AsString(100)
                    .NotNullable()
                    .Indexed("IX_msg_recipient")
                .WithColumn("subject")
                    .AsString(1000)
                    .NotNullable()
                .WithColumn("content")
                    .AsString(int.MaxValue)
                    .NotNullable();
        }
    }
}
