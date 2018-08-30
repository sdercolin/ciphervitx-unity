using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IAttachable
{
    string Guid { get; set; }
    Card Owner { get; set; }
    bool OnlyAvailableWhenFrontShown { get; set; }

    void Attached();
    void Detach();
    bool Equals(IAttachable item);
    void Read(Message message);
    bool Try(Message message, ref Message substitute);
}
