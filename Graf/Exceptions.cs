using System;

namespace Graf
{
    class GraphParameterException : Exception
    {
        public GraphParameterException()
        {

        }

        public GraphParameterException(string message) : base(message)
        {

        }

        public GraphParameterException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
