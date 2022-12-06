package prince.yvars.routefinder;

public enum TransportType {
    FOOT("foot"),
    BIKE("bike");

    private final String name;

    TransportType(String name) {
        this.name = name;
    }

    public String getName() {
        return name;
    }
}