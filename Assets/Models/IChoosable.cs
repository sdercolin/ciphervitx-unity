using System;

public interface IChoosable
{
    string GetDescription(DescriptionPattern descriptionOption = DescriptionPattern.Default);
    string GetImagePath();
}

public enum DescriptionPattern
{
    Default,
    Full
}