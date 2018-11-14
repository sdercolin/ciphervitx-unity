using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public interface IRequestUIController
{
    Task<List<T>> RequestChoose<T>(List<T> choices, int min, int max, Request.RequestFlags flags, string description) where T : IChoosable;
    Task<bool> RequestAskIf(Request.RequestFlags flags, string description);
}
