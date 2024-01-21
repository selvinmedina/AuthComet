﻿namespace AuthComet.Domain.Response
{
    public class Response<T>
    {
        public T? Data { get; set; }
        public bool Ok { get; set; }
        public string Message { get; set; } = null!;

        public static Response<T> Success(T data)
        {
            return new Response<T>
            {
                Ok = true,
                Message = string.Empty,
                Data = data
            };
        }

        public static Response<T> Fail(string message)
        {
            return new Response<T>
            {
                Ok = false,
                Message = message,
                Data = default(T)!
            };
        }
    }
}
