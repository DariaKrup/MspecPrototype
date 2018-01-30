namespace AuthorMaintenenceResource.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IRepository<T>
    {
        bool ContextInUse { get; set; }

        event EventHandler<CustomerNotificationArgs> CustomerNotificationEvent;

        T Save(T t);
    }
}
