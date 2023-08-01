using System;

namespace Evo.Scm.Dtos;

public interface IColorAndSize
{
    public Guid ColorId { get; }
    public string ColorName { get; }
    public Guid SizeId { get; }
    public string SizeName { get;  }
}