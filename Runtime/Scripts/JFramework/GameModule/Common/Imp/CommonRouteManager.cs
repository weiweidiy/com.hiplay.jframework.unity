using System;

namespace JFramework
{

    public class CommonRouteManager :  BaseRunable , IRouter
    {

        public RouteResponse GetHotFixAddress(RouteRequest routeInfo)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnStart(RunableExtraData extraData)
        {
            var routeRequest = (RouteRequest)extraData.Data;

            var routeReponse = GetHotFixAddress(routeRequest);

            Stop();
        }
    }

}

