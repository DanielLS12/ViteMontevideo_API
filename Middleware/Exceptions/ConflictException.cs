﻿namespace ViteMontevideo_API.Middleware.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}
