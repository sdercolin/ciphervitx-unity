using System;
using System.Collections.Generic;

public interface IForbidPosition
{
    List<Type> ForbiddenAreaTypes { get; }
}

public interface IUserForbidActionSkill
{
    string ForbiddenSkillName { get; }
}