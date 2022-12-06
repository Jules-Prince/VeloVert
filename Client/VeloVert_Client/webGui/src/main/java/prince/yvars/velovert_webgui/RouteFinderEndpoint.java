package prince.yvars.velovert_webgui;



import prince.yvars.routefinder.RouteFinder;

import javax.ws.rs.*;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;


@Path("/route")
public class RouteFinderEndpoint {



    @GET
    @Path("/test")
    public String test(){
        return "test";
    }

    @POST
    @Consumes(MediaType.APPLICATION_JSON)
    @Path("/itinerary")
    public Response route(Itineary itineary) {
        String res = RouteFinder.getItineary(itineary.departure, itineary.arrival);
        System.out.println(res);
        return Response.ok(res).build();
    }
}
