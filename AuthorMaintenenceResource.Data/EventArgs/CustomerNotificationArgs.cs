namespace AuthorMaintenenceResource.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CustomerNotificationArgs : EventArgs
    {
        public string Name { get; private set; }

        public CustomerNotificationArgs(string name)
        {
            Name = name;
        }
    }
}
