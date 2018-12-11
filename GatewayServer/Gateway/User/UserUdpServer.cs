﻿using System.Diagnostics;

using Share.Net.Server;
using Share.Net.Sessions;

namespace GatewayServer.Gateway.User
{
    public sealed class UserUdpServer : UdpServer
    {
        public UserUdpServer()
            :base(12000)
        { }


        public override void Start()
        {
            base.Start();
        }

        public override void Stop()
        {
            base.Stop();
        }


        protected override Session AllocateSession()
        {
            Session user_sess = SessionManager.Instance.AllocateUdpUserSession();
            Debug.Assert(null != user_sess);
            Debug.Assert(user_sess is UdpSession);

            return user_sess;
        }

        protected override void FreeSession(Session sess)
        {
            Debug.Assert(null != sess);
            SessionManager.Instance.FreeUdpUserSession(sess);
        }

        protected override SOCK_SERV_ERROR AddToRelevantManager(Session sess)
        {
            SOCK_SERV_ERROR ret = SOCK_SERV_ERROR.SUCCESS;
            Debug.Assert(null != sess);
            Debug.Assert(sess is UdpSession);

            User user = UserManager.Instance.AllocateUser();

            if (null != user)
            {
                UdpSession user_sess = (UdpSession)sess;
                user_sess.SetObject(user);

                user.SetUserSession(user_sess);

                UserManager.Instance.AddConnectlessUser(user);
            }
            else
            {
                ret = SOCK_SERV_ERROR.E_USER_SESSION_IS_EMPTY;
            }

            return ret;
        }
    }
}
