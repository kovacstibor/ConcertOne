using System;
using System.Collections.Generic;

namespace ConcertOne.Web.Services
{
    public class SessionIdService
    {
        private readonly Dictionary<string, bool> _sessions;
        private readonly Dictionary<string, Guid> _userIds;

        public SessionIdService()
        {
            _sessions = new Dictionary<string, bool>();
            _userIds = new Dictionary<string, Guid>();
        }

        public string CreatePriviledgedSession( Guid userId )
        {
            var sessionId = Guid.NewGuid().ToString( "N" );
            _sessions.Add( sessionId, true );
            _userIds.Add( sessionId, userId );
            return sessionId;
        }

        public string CreateNormalSession( Guid userId )
        {
            var sessionId = Guid.NewGuid().ToString( "N" );
            _sessions.Add( sessionId, false );
            _userIds.Add( sessionId, userId );
            return sessionId;
        }

        public bool IsPriviledged( string sessionId )
        {
            return _sessions.ContainsKey( sessionId ) && _sessions[sessionId];
        }

        public Guid GetUserId( string sessionId )
        {
            return _userIds[sessionId];
        }

        public void RemoveSessionId( string sessionId )
        {
            _sessions.Remove( sessionId );
            _userIds.Remove( sessionId );
        }

        public bool IsValidSessionId( string sessionId )
        {
            return _sessions.ContainsKey( sessionId );
        }
    }
}
