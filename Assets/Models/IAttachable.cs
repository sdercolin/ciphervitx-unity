using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IAttachable
{
    Card Owner { get; set; }
    bool OnlyAvailableWhenFrontShown { get; set; }
    List<Area> AvailableAreas { get; set; }

    void Detach();
    void Read(Message message);
    bool Try(Message message, ref Message substitute);
}
