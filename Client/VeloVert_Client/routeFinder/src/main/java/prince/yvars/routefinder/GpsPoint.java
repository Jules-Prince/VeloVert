package prince.yvars.routefinder;


public class GpsPoint {

    private String latitude;
    private String longitude;
    private TransportType transportType;

    public GpsPoint(String latitude, String longitude, TransportType transportType) {
        this.latitude = latitude;
        this.longitude = longitude;
        this.transportType = transportType;
    }
}
