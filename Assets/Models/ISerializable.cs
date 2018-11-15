using System;

public interface ISerializable
{
    string Guid { get; set; }
    string Serialize();
}

