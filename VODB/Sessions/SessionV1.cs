﻿using VODB.Core;
using VODB.DbLayer;

namespace VODB.Sessions
{
    class SessionV1 : SessionBase
    {
        public SessionV1()
            : base((IInternalSession)Engine.Get<ISession>())
        { }

        public SessionV1(IDbConnectionCreator connectionCreator)
            : base((IInternalSession)Engine.Get<ISession>("creator", connectionCreator))
        { }

        public override string ToString()
        {
            return "Session V1.0";
        }
    }
}