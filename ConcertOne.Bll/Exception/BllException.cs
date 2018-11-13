using System;

namespace ConcertOne.Bll.Exception
{
    [Serializable]
    public class BllException : System.Exception
    {
        public BllException()
            : base()
        {

        }

        public BllException( string message )
            : base( message )
        {

        }

        public BllException( string message, System.Exception innerException )
            : base( message, innerException )
        {

        }
    }
}
