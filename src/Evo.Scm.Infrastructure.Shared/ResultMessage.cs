﻿
namespace Evo.Scm
{
    public class ResultMessage<T>
    {
        public string Code { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
    }
}
